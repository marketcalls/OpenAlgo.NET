using OpenAlgo.NET.Models.Common;
using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.Data;

/// <summary>
/// Data API methods for OpenAlgo.
/// </summary>
public abstract class DataApi : Orders.OrderApi
{
    /// <summary>
    /// Initializes a new instance of the DataApi class.
    /// </summary>
    protected DataApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
        : base(apiKey, host, version, timeout)
    {
    }

    #region Quotes

    /// <summary>
    /// Get real-time quotes for a symbol (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Quotes response.</returns>
    public async Task<QuotesResponse> QuotesAsync(
        string symbol,
        string exchange,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;

        return await MakeRequestAsync<QuotesResponse>("quotes", payload, cancellationToken);
    }

    /// <summary>
    /// Get real-time quotes for a symbol (sync).
    /// </summary>
    public QuotesResponse Quotes(string symbol, string exchange)
    {
        return QuotesAsync(symbol, exchange).GetAwaiter().GetResult();
    }

    #endregion

    #region MultiQuotes

    /// <summary>
    /// Get real-time quotes for multiple symbols in a single request (async).
    /// </summary>
    /// <param name="symbols">List of symbol-exchange pairs. Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Multi quotes response.</returns>
    public async Task<MultiQuotesResponse> MultiQuotesAsync(
        List<SymbolExchangePair> symbols,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbols"] = symbols.Select(s => new Dictionary<string, object>
        {
            ["symbol"] = s.Symbol,
            ["exchange"] = s.Exchange
        }).ToList();

        return await MakeRequestAsync<MultiQuotesResponse>("multiquotes", payload, cancellationToken);
    }

    /// <summary>
    /// Get real-time quotes for multiple symbols in a single request (sync).
    /// </summary>
    public MultiQuotesResponse MultiQuotes(List<SymbolExchangePair> symbols)
    {
        return MultiQuotesAsync(symbols).GetAwaiter().GetResult();
    }

    #endregion

    #region Depth

    /// <summary>
    /// Get market depth (order book) for a symbol (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Depth response.</returns>
    public async Task<DepthResponse> DepthAsync(
        string symbol,
        string exchange,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;

        return await MakeRequestAsync<DepthResponse>("depth", payload, cancellationToken);
    }

    /// <summary>
    /// Get market depth (order book) for a symbol (sync).
    /// </summary>
    public DepthResponse Depth(string symbol, string exchange)
    {
        return DepthAsync(symbol, exchange).GetAwaiter().GetResult();
    }

    #endregion

    #region History

    /// <summary>
    /// Get historical data for a symbol (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="interval">Time interval for the data. Required.</param>
    /// <param name="startDate">Start date in format 'YYYY-MM-DD'. Required.</param>
    /// <param name="endDate">End date in format 'YYYY-MM-DD'. Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>History response.</returns>
    public async Task<HistoryResponse> HistoryAsync(
        string symbol,
        string exchange,
        string interval,
        string startDate,
        string endDate,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;
        payload["interval"] = interval;
        payload["start_date"] = startDate;
        payload["end_date"] = endDate;

        return await MakeRequestAsync<HistoryResponse>("history", payload, cancellationToken);
    }

    /// <summary>
    /// Get historical data for a symbol (sync).
    /// </summary>
    public HistoryResponse History(
        string symbol,
        string exchange,
        string interval,
        string startDate,
        string endDate)
    {
        return HistoryAsync(symbol, exchange, interval, startDate, endDate).GetAwaiter().GetResult();
    }

    #endregion

    #region Intervals

    /// <summary>
    /// Get supported time intervals for historical data (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Intervals response.</returns>
    public async Task<IntervalsResponse> IntervalsAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<IntervalsResponse>("intervals", payload, cancellationToken);
    }

    /// <summary>
    /// Get supported time intervals for historical data (sync).
    /// </summary>
    public IntervalsResponse Intervals()
    {
        return IntervalsAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region Symbol

    /// <summary>
    /// Get symbol details from the API (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Symbol response.</returns>
    public async Task<SymbolResponse> SymbolAsync(
        string symbol,
        string exchange,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;

        return await MakeRequestAsync<SymbolResponse>("symbol", payload, cancellationToken);
    }

    /// <summary>
    /// Get symbol details from the API (sync).
    /// </summary>
    public SymbolResponse Symbol(string symbol, string exchange)
    {
        return SymbolAsync(symbol, exchange).GetAwaiter().GetResult();
    }

    #endregion

    #region Search

    /// <summary>
    /// Search for symbols across exchanges (async).
    /// </summary>
    /// <param name="query">Search query for symbol. Required.</param>
    /// <param name="exchange">Exchange filter. Optional.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Search response.</returns>
    public async Task<SearchResponse> SearchAsync(
        string query,
        string? exchange = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["query"] = query;

        if (!string.IsNullOrEmpty(exchange))
        {
            payload["exchange"] = exchange;
        }

        return await MakeRequestAsync<SearchResponse>("search", payload, cancellationToken);
    }

    /// <summary>
    /// Search for symbols across exchanges (sync).
    /// </summary>
    public SearchResponse Search(string query, string? exchange = null)
    {
        return SearchAsync(query, exchange).GetAwaiter().GetResult();
    }

    #endregion

    #region Expiry

    /// <summary>
    /// Get expiry dates for a symbol (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="instrumenttype">Instrument type (futures/options). Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Expiry response.</returns>
    public async Task<ExpiryResponse> ExpiryAsync(
        string symbol,
        string exchange,
        string instrumenttype,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;
        payload["instrumenttype"] = instrumenttype;

        return await MakeRequestAsync<ExpiryResponse>("expiry", payload, cancellationToken);
    }

    /// <summary>
    /// Get expiry dates for a symbol (sync).
    /// </summary>
    public ExpiryResponse Expiry(string symbol, string exchange, string instrumenttype)
    {
        return ExpiryAsync(symbol, exchange, instrumenttype).GetAwaiter().GetResult();
    }

    #endregion

    #region Instruments

    /// <summary>
    /// Download all trading symbols and instruments with optional exchange filtering (async).
    /// </summary>
    /// <param name="exchange">Exchange to filter instruments. Optional (downloads ALL exchanges if not specified).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Instruments response.</returns>
    public async Task<InstrumentsResponse> InstrumentsAsync(
        string? exchange = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(exchange))
        {
            // Fetch all exchanges and combine
            var allExchanges = new[] { "NSE", "BSE", "NFO", "BFO", "MCX", "CDS", "BCD", "NSE_INDEX", "BSE_INDEX" };
            var allData = new List<InstrumentData>();

            foreach (var exch in allExchanges)
            {
                try
                {
                    var result = await InstrumentsSingleExchangeAsync(exch, cancellationToken);
                    if (result.IsSuccess && result.Data != null)
                    {
                        allData.AddRange(result.Data);
                    }
                }
                catch
                {
                    // Skip exchanges that fail
                }
            }

            return new InstrumentsResponse
            {
                Status = allData.Count > 0 ? "success" : "error",
                Data = allData,
                Message = allData.Count == 0 ? "Failed to fetch instruments from any exchange" : null
            };
        }

        return await InstrumentsSingleExchangeAsync(exchange, cancellationToken);
    }

    private async Task<InstrumentsResponse> InstrumentsSingleExchangeAsync(
        string exchange,
        CancellationToken cancellationToken)
    {
        // Use GET request for instruments endpoint
        var url = $"{BaseUrl}instruments?apikey={ApiKey}&exchange={exchange}";

        try
        {
            var response = await HttpClient.GetAsync(url, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new InstrumentsResponse
                {
                    Status = "error",
                    Message = $"HTTP {(int)response.StatusCode}: {content}"
                };
            }

            var result = System.Text.Json.JsonSerializer.Deserialize<InstrumentsResponse>(content, JsonOptions);
            return result ?? new InstrumentsResponse { Status = "error", Message = "Empty response" };
        }
        catch (Exception ex)
        {
            return new InstrumentsResponse
            {
                Status = "error",
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Download all trading symbols and instruments with optional exchange filtering (sync).
    /// </summary>
    public InstrumentsResponse Instruments(string? exchange = null)
    {
        return InstrumentsAsync(exchange).GetAwaiter().GetResult();
    }

    #endregion

    #region SyntheticFuture

    /// <summary>
    /// Calculate synthetic futures price using ATM Call and Put options (async).
    /// </summary>
    /// <param name="underlying">Underlying symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="expiryDate">Expiry date in DDMMMYY format. Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Synthetic future response.</returns>
    public async Task<SyntheticFutureResponse> SyntheticFutureAsync(
        string underlying,
        string exchange,
        string expiryDate,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["underlying"] = underlying;
        payload["exchange"] = exchange;
        payload["expiry_date"] = expiryDate;

        return await MakeRequestAsync<SyntheticFutureResponse>("syntheticfuture", payload, cancellationToken);
    }

    /// <summary>
    /// Calculate synthetic futures price using ATM Call and Put options (sync).
    /// </summary>
    public SyntheticFutureResponse SyntheticFuture(string underlying, string exchange, string expiryDate)
    {
        return SyntheticFutureAsync(underlying, exchange, expiryDate).GetAwaiter().GetResult();
    }

    #endregion
}
