using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using OpenAlgo.NET.Models.Common;

namespace OpenAlgo.NET.Feed;

/// <summary>
/// LTP data received from WebSocket.
/// </summary>
public class LtpData
{
    public string Exchange { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long Timestamp { get; set; }
}

/// <summary>
/// Quote data received from WebSocket.
/// </summary>
public class QuoteData
{
    public string Exchange { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Ltp { get; set; }
    public long Volume { get; set; }
    public long Timestamp { get; set; }
}

/// <summary>
/// Depth data received from WebSocket.
/// </summary>
public class DepthData
{
    public string Exchange { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal Ltp { get; set; }
    public long Timestamp { get; set; }
    public List<DepthLevel> Buy { get; set; } = new();
    public List<DepthLevel> Sell { get; set; } = new();
}

/// <summary>
/// Depth level data.
/// </summary>
public class DepthLevel
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Orders { get; set; }
}

/// <summary>
/// Market data feed API methods for OpenAlgo using WebSockets.
/// </summary>
public abstract class FeedApi : Utilities.UtilitiesApi
{
    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _receiveTask;

    /// <summary>
    /// WebSocket port.
    /// </summary>
    protected int WsPort;

    /// <summary>
    /// WebSocket URL.
    /// </summary>
    protected string WsUrl;

    /// <summary>
    /// Verbosity level (0=silent, 1=basic, 2=debug).
    /// </summary>
    protected int Verbose;

    /// <summary>
    /// Whether the WebSocket is connected.
    /// </summary>
    public bool Connected { get; private set; }

    /// <summary>
    /// Whether the WebSocket is authenticated.
    /// </summary>
    public bool Authenticated { get; private set; }

    // Data storage
    private readonly ConcurrentDictionary<string, LtpData> _ltpData = new();
    private readonly ConcurrentDictionary<string, QuoteData> _quotesData = new();
    private readonly ConcurrentDictionary<string, DepthData> _depthData = new();

    // Events for callbacks
    /// <summary>
    /// Event raised when LTP data is received.
    /// </summary>
    public event Action<LtpData>? OnLtpReceived;

    /// <summary>
    /// Event raised when Quote data is received.
    /// </summary>
    public event Action<QuoteData>? OnQuoteReceived;

    /// <summary>
    /// Event raised when Depth data is received.
    /// </summary>
    public event Action<DepthData>? OnDepthReceived;

    /// <summary>
    /// Initializes a new instance of the FeedApi class.
    /// </summary>
    protected FeedApi(
        string apiKey,
        string host = "http://127.0.0.1:5000",
        string version = "v1",
        double timeout = 120.0,
        int wsPort = 8765,
        string? wsUrl = null,
        int verbose = 0)
        : base(apiKey, host, version, timeout)
    {
        WsPort = wsPort;
        Verbose = verbose;

        if (!string.IsNullOrEmpty(wsUrl))
        {
            WsUrl = wsUrl;
        }
        else
        {
            // Extract host without protocol for WebSocket
            var wsHost = host;
            if (wsHost.StartsWith("http://"))
                wsHost = wsHost[7..];
            else if (wsHost.StartsWith("https://"))
                wsHost = wsHost[8..];

            // Remove any path component and port if present
            wsHost = wsHost.Split('/')[0].Split(':')[0];
            WsUrl = $"ws://{wsHost}:{wsPort}";
        }
    }

    private void Log(int level, string category, string message)
    {
        if (Verbose >= level)
        {
            Console.WriteLine($"[{category,-6}] {message}");
        }
    }

    #region Connect

    /// <summary>
    /// Connect to the WebSocket server and authenticate (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if connection and authentication are successful.</returns>
    public async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Log(1, "WS", $"Connecting to {WsUrl}...");
            await _webSocket.ConnectAsync(new Uri(WsUrl), _cancellationTokenSource.Token);
            Connected = true;
            Log(1, "WS", $"Connected to {WsUrl}");

            // Start receiving messages
            _receiveTask = ReceiveMessagesAsync(_cancellationTokenSource.Token);

            // Authenticate
            await AuthenticateAsync();

            // Wait for authentication
            var timeout = DateTime.UtcNow.AddSeconds(5);
            while (!Authenticated && DateTime.UtcNow < timeout)
            {
                await Task.Delay(100, cancellationToken);
            }

            return Authenticated;
        }
        catch (Exception ex)
        {
            Log(1, "ERROR", $"Error connecting to WebSocket: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Connect to the WebSocket server and authenticate (sync).
    /// </summary>
    public bool Connect()
    {
        return ConnectAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region Disconnect

    /// <summary>
    /// Disconnect from the WebSocket server (async).
    /// </summary>
    public async Task DisconnectAsync()
    {
        if (_webSocket != null)
        {
            try
            {
                _cancellationTokenSource?.Cancel();

                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }

                _webSocket.Dispose();
                _webSocket = null;
            }
            catch (Exception ex)
            {
                Log(1, "ERROR", $"Error disconnecting: {ex.Message}");
            }

            Connected = false;
            Authenticated = false;
            Log(1, "WS", $"Disconnected from {WsUrl}");
        }
    }

    /// <summary>
    /// Disconnect from the WebSocket server (sync).
    /// </summary>
    public void Disconnect()
    {
        DisconnectAsync().GetAwaiter().GetResult();
    }

    #endregion

    private async Task AuthenticateAsync()
    {
        var authMsg = new { action = "authenticate", api_key = ApiKey };
        var json = JsonSerializer.Serialize(authMsg);
        var bytes = Encoding.UTF8.GetBytes(json);

        Log(1, "AUTH", $"Authenticating with API key: {ApiKey[..8]}...{ApiKey[^8..]}");
        await _webSocket!.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[8192];
        var messageBuilder = new StringBuilder();

        try
        {
            while (_webSocket?.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.EndOfMessage)
                {
                    var message = messageBuilder.ToString();
                    messageBuilder.Clear();
                    ProcessMessage(message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
        }
        catch (Exception ex)
        {
            Log(1, "ERROR", $"WebSocket receive error: {ex.Message}");
        }
    }

    private void ProcessMessage(string messageStr)
    {
        try
        {
            using var doc = JsonDocument.Parse(messageStr);
            var root = doc.RootElement;

            var type = root.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : null;

            // Handle authentication response
            if (type == "auth")
            {
                var status = root.TryGetProperty("status", out var statusElement) ? statusElement.GetString() : null;
                if (status == "success")
                {
                    Authenticated = true;
                    var broker = root.TryGetProperty("broker", out var brokerElement) ? brokerElement.GetString() : "unknown";
                    var userId = root.TryGetProperty("user_id", out var userElement) ? userElement.GetString() : "unknown";
                    Log(1, "AUTH", $"Success | Broker: {broker} | User: {userId}");
                }
                else
                {
                    var errorMessage = root.TryGetProperty("message", out var msgElement) ? msgElement.GetString() : "Unknown error";
                    Log(1, "ERROR", $"Authentication failed: {errorMessage}");
                }
                return;
            }

            // Handle subscription response
            if (type == "subscribe")
            {
                if (root.TryGetProperty("subscriptions", out var subs))
                {
                    foreach (var sub in subs.EnumerateArray())
                    {
                        var sym = sub.TryGetProperty("symbol", out var symElement) ? symElement.GetString() : "?";
                        var exch = sub.TryGetProperty("exchange", out var exchElement) ? exchElement.GetString() : "?";
                        var subStatus = sub.TryGetProperty("status", out var subStatusElement) ? subStatusElement.GetString() : "?";
                        var mode = sub.TryGetProperty("mode", out var modeElement) ? modeElement.GetInt32() : 0;
                        var modeName = mode switch { 1 => "LTP", 2 => "Quote", 3 => "Depth", _ => "Unknown" };
                        Log(1, "SUB", $"{exch}:{sym} | Mode: {modeName} | Status: {subStatus}");
                    }
                }
                return;
            }

            // Handle market data
            if (type == "market_data")
            {
                var exchange = root.TryGetProperty("exchange", out var exchElement) ? exchElement.GetString() : null;
                var symbol = root.TryGetProperty("symbol", out var symElement) ? symElement.GetString() : null;
                var mode = root.TryGetProperty("mode", out var modeElement) ? modeElement.GetInt32() : 0;

                if (exchange != null && symbol != null && root.TryGetProperty("data", out var dataElement))
                {
                    var symbolKey = $"{exchange}:{symbol}";

                    switch (mode)
                    {
                        case 1: // LTP
                            HandleLtpData(exchange, symbol, symbolKey, dataElement);
                            break;
                        case 2: // Quote
                            HandleQuoteData(exchange, symbol, symbolKey, dataElement);
                            break;
                        case 3: // Depth
                            HandleDepthData(exchange, symbol, symbolKey, dataElement);
                            break;
                    }
                }
            }
        }
        catch (JsonException ex)
        {
            Log(1, "ERROR", $"Invalid JSON message: {messageStr[..Math.Min(100, messageStr.Length)]}... {ex.Message}");
        }
        catch (Exception ex)
        {
            Log(1, "ERROR", $"Error handling message: {ex.Message}");
        }
    }

    private void HandleLtpData(string exchange, string symbol, string symbolKey, JsonElement dataElement)
    {
        var ltp = dataElement.TryGetProperty("ltp", out var ltpElement) ? ltpElement.GetDecimal() : 0;
        var timestamp = dataElement.TryGetProperty("timestamp", out var tsElement) ? tsElement.GetInt64() : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var ltpData = new LtpData
        {
            Exchange = exchange,
            Symbol = symbol,
            Price = ltp,
            Timestamp = timestamp
        };

        _ltpData[symbolKey] = ltpData;
        Log(2, "LTP", $"{symbolKey,-20} | LTP: {ltp}");
        OnLtpReceived?.Invoke(ltpData);
    }

    private void HandleQuoteData(string exchange, string symbol, string symbolKey, JsonElement dataElement)
    {
        var quoteData = new QuoteData
        {
            Exchange = exchange,
            Symbol = symbol,
            Open = dataElement.TryGetProperty("open", out var openElement) ? openElement.GetDecimal() : 0,
            High = dataElement.TryGetProperty("high", out var highElement) ? highElement.GetDecimal() : 0,
            Low = dataElement.TryGetProperty("low", out var lowElement) ? lowElement.GetDecimal() : 0,
            Close = dataElement.TryGetProperty("close", out var closeElement) ? closeElement.GetDecimal() : 0,
            Ltp = dataElement.TryGetProperty("ltp", out var ltpElement) ? ltpElement.GetDecimal() : 0,
            Volume = dataElement.TryGetProperty("volume", out var volElement) ? volElement.GetInt64() : 0,
            Timestamp = dataElement.TryGetProperty("timestamp", out var tsElement) ? tsElement.GetInt64() : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        _quotesData[symbolKey] = quoteData;
        Log(2, "QUOTE", $"{symbolKey,-20} | O: {quoteData.Open,-10} H: {quoteData.High,-10} L: {quoteData.Low,-10} LTP: {quoteData.Ltp}");
        OnQuoteReceived?.Invoke(quoteData);
    }

    private void HandleDepthData(string exchange, string symbol, string symbolKey, JsonElement dataElement)
    {
        var depthData = new DepthData
        {
            Exchange = exchange,
            Symbol = symbol,
            Ltp = dataElement.TryGetProperty("ltp", out var ltpElement) ? ltpElement.GetDecimal() : 0,
            Timestamp = dataElement.TryGetProperty("timestamp", out var tsElement) ? tsElement.GetInt64() : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        if (dataElement.TryGetProperty("depth", out var depthElement))
        {
            if (depthElement.TryGetProperty("buy", out var buyElement))
            {
                foreach (var level in buyElement.EnumerateArray())
                {
                    depthData.Buy.Add(new DepthLevel
                    {
                        Price = level.TryGetProperty("price", out var priceElement) ? priceElement.GetDecimal() : 0,
                        Quantity = level.TryGetProperty("quantity", out var qtyElement) ? qtyElement.GetInt32() : 0,
                        Orders = level.TryGetProperty("orders", out var ordersElement) ? ordersElement.GetInt32() : 0
                    });
                }
            }

            if (depthElement.TryGetProperty("sell", out var sellElement))
            {
                foreach (var level in sellElement.EnumerateArray())
                {
                    depthData.Sell.Add(new DepthLevel
                    {
                        Price = level.TryGetProperty("price", out var priceElement) ? priceElement.GetDecimal() : 0,
                        Quantity = level.TryGetProperty("quantity", out var qtyElement) ? qtyElement.GetInt32() : 0,
                        Orders = level.TryGetProperty("orders", out var ordersElement) ? ordersElement.GetInt32() : 0
                    });
                }
            }
        }

        _depthData[symbolKey] = depthData;
        Log(2, "DEPTH", $"{symbolKey,-20} | LTP: {depthData.Ltp}");
        OnDepthReceived?.Invoke(depthData);
    }

    #region Subscribe/Unsubscribe Methods

    private async Task<bool> SubscribeAsync(List<Instrument> instruments, int mode, CancellationToken cancellationToken)
    {
        if (!Connected)
        {
            Log(1, "ERROR", "Not connected to WebSocket server");
            return false;
        }

        if (!Authenticated)
        {
            Log(1, "ERROR", "Not authenticated with WebSocket server");
            return false;
        }

        foreach (var instrument in instruments)
        {
            var subscribeMsg = new
            {
                action = "subscribe",
                symbol = instrument.Symbol,
                exchange = instrument.Exchange,
                mode = mode,
                depth = 5
            };

            var json = JsonSerializer.Serialize(subscribeMsg);
            var bytes = Encoding.UTF8.GetBytes(json);

            var modeName = mode switch { 1 => "LTP", 2 => "Quote", 3 => "Depth", _ => "Unknown" };
            Log(1, "SUB", $"Subscribing {instrument.Exchange}:{instrument.Symbol} {modeName}...");

            try
            {
                await _webSocket!.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
                await Task.Delay(100, cancellationToken);
            }
            catch (Exception ex)
            {
                Log(1, "ERROR", $"Error subscribing to {instrument.Exchange}:{instrument.Symbol}: {ex.Message}");
                return false;
            }
        }

        return true;
    }

    private async Task<bool> UnsubscribeAsync(List<Instrument> instruments, int mode, CancellationToken cancellationToken)
    {
        if (!Connected || !Authenticated)
        {
            return false;
        }

        foreach (var instrument in instruments)
        {
            var unsubscribeMsg = new
            {
                action = "unsubscribe",
                symbol = instrument.Symbol,
                exchange = instrument.Exchange,
                mode = mode
            };

            var json = JsonSerializer.Serialize(unsubscribeMsg);
            var bytes = Encoding.UTF8.GetBytes(json);

            var modeName = mode switch { 1 => "LTP", 2 => "Quote", 3 => "Depth", _ => "Unknown" };
            Log(1, "UNSUB", $"Unsubscribing {instrument.Exchange}:{instrument.Symbol} {modeName}");

            try
            {
                await _webSocket!.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);

                var symbolKey = $"{instrument.Exchange}:{instrument.Symbol}";
                switch (mode)
                {
                    case 1: _ltpData.TryRemove(symbolKey, out _); break;
                    case 2: _quotesData.TryRemove(symbolKey, out _); break;
                    case 3: _depthData.TryRemove(symbolKey, out _); break;
                }

                await Task.Delay(100, cancellationToken);
            }
            catch (Exception ex)
            {
                Log(1, "ERROR", $"Error unsubscribing {instrument.Exchange}:{instrument.Symbol}: {ex.Message}");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Subscribe to LTP updates for instruments (async).
    /// </summary>
    /// <param name="instruments">List of instruments to subscribe to.</param>
    /// <param name="onDataReceived">Callback function for data updates.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if subscription successful.</returns>
    public async Task<bool> SubscribeLtpAsync(
        List<Instrument> instruments,
        Action<LtpData>? onDataReceived = null,
        CancellationToken cancellationToken = default)
    {
        if (onDataReceived != null)
        {
            OnLtpReceived += onDataReceived;
        }
        return await SubscribeAsync(instruments, 1, cancellationToken);
    }

    /// <summary>
    /// Subscribe to LTP updates for instruments (sync).
    /// </summary>
    public bool SubscribeLtp(List<Instrument> instruments, Action<LtpData>? onDataReceived = null)
    {
        return SubscribeLtpAsync(instruments, onDataReceived).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Unsubscribe from LTP updates for instruments (async).
    /// </summary>
    public async Task<bool> UnsubscribeLtpAsync(List<Instrument> instruments, CancellationToken cancellationToken = default)
    {
        return await UnsubscribeAsync(instruments, 1, cancellationToken);
    }

    /// <summary>
    /// Unsubscribe from LTP updates for instruments (sync).
    /// </summary>
    public bool UnsubscribeLtp(List<Instrument> instruments)
    {
        return UnsubscribeLtpAsync(instruments).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Subscribe to Quote updates for instruments (async).
    /// </summary>
    public async Task<bool> SubscribeQuoteAsync(
        List<Instrument> instruments,
        Action<QuoteData>? onDataReceived = null,
        CancellationToken cancellationToken = default)
    {
        if (onDataReceived != null)
        {
            OnQuoteReceived += onDataReceived;
        }
        return await SubscribeAsync(instruments, 2, cancellationToken);
    }

    /// <summary>
    /// Subscribe to Quote updates for instruments (sync).
    /// </summary>
    public bool SubscribeQuote(List<Instrument> instruments, Action<QuoteData>? onDataReceived = null)
    {
        return SubscribeQuoteAsync(instruments, onDataReceived).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Unsubscribe from Quote updates for instruments (async).
    /// </summary>
    public async Task<bool> UnsubscribeQuoteAsync(List<Instrument> instruments, CancellationToken cancellationToken = default)
    {
        return await UnsubscribeAsync(instruments, 2, cancellationToken);
    }

    /// <summary>
    /// Unsubscribe from Quote updates for instruments (sync).
    /// </summary>
    public bool UnsubscribeQuote(List<Instrument> instruments)
    {
        return UnsubscribeQuoteAsync(instruments).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Subscribe to Depth updates for instruments (async).
    /// </summary>
    public async Task<bool> SubscribeDepthAsync(
        List<Instrument> instruments,
        Action<DepthData>? onDataReceived = null,
        CancellationToken cancellationToken = default)
    {
        if (onDataReceived != null)
        {
            OnDepthReceived += onDataReceived;
        }
        return await SubscribeAsync(instruments, 3, cancellationToken);
    }

    /// <summary>
    /// Subscribe to Depth updates for instruments (sync).
    /// </summary>
    public bool SubscribeDepth(List<Instrument> instruments, Action<DepthData>? onDataReceived = null)
    {
        return SubscribeDepthAsync(instruments, onDataReceived).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Unsubscribe from Depth updates for instruments (async).
    /// </summary>
    public async Task<bool> UnsubscribeDepthAsync(List<Instrument> instruments, CancellationToken cancellationToken = default)
    {
        return await UnsubscribeAsync(instruments, 3, cancellationToken);
    }

    /// <summary>
    /// Unsubscribe from Depth updates for instruments (sync).
    /// </summary>
    public bool UnsubscribeDepth(List<Instrument> instruments)
    {
        return UnsubscribeDepthAsync(instruments).GetAwaiter().GetResult();
    }

    #endregion

    #region Get Data Methods

    /// <summary>
    /// Get the latest LTP data.
    /// </summary>
    /// <param name="exchange">Filter by exchange (optional).</param>
    /// <param name="symbol">Filter by symbol (optional, requires exchange).</param>
    /// <returns>Dictionary with LTP data.</returns>
    public Dictionary<string, Dictionary<string, LtpData>> GetLtp(string? exchange = null, string? symbol = null)
    {
        var result = new Dictionary<string, Dictionary<string, LtpData>>();

        foreach (var (key, data) in _ltpData)
        {
            var parts = key.Split(':');
            if (parts.Length != 2) continue;

            var ex = parts[0];
            var sym = parts[1];

            if (!string.IsNullOrEmpty(exchange) && ex != exchange) continue;
            if (!string.IsNullOrEmpty(symbol) && sym != symbol) continue;

            if (!result.ContainsKey(ex))
            {
                result[ex] = new Dictionary<string, LtpData>();
            }

            result[ex][sym] = data;
        }

        return result;
    }

    /// <summary>
    /// Get the latest Quote data.
    /// </summary>
    public Dictionary<string, Dictionary<string, QuoteData>> GetQuotes(string? exchange = null, string? symbol = null)
    {
        var result = new Dictionary<string, Dictionary<string, QuoteData>>();

        foreach (var (key, data) in _quotesData)
        {
            var parts = key.Split(':');
            if (parts.Length != 2) continue;

            var ex = parts[0];
            var sym = parts[1];

            if (!string.IsNullOrEmpty(exchange) && ex != exchange) continue;
            if (!string.IsNullOrEmpty(symbol) && sym != symbol) continue;

            if (!result.ContainsKey(ex))
            {
                result[ex] = new Dictionary<string, QuoteData>();
            }

            result[ex][sym] = data;
        }

        return result;
    }

    /// <summary>
    /// Get the latest Depth data.
    /// </summary>
    public Dictionary<string, Dictionary<string, DepthData>> GetDepth(string? exchange = null, string? symbol = null)
    {
        var result = new Dictionary<string, Dictionary<string, DepthData>>();

        foreach (var (key, data) in _depthData)
        {
            var parts = key.Split(':');
            if (parts.Length != 2) continue;

            var ex = parts[0];
            var sym = parts[1];

            if (!string.IsNullOrEmpty(exchange) && ex != exchange) continue;
            if (!string.IsNullOrEmpty(symbol) && sym != symbol) continue;

            if (!result.ContainsKey(ex))
            {
                result[ex] = new Dictionary<string, DepthData>();
            }

            result[ex][sym] = data;
        }

        return result;
    }

    #endregion
}
