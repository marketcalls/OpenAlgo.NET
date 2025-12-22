namespace OpenAlgo.NET;

/// <summary>
/// OpenAlgo .NET SDK client for algorithmic trading.
/// Provides access to all OpenAlgo API endpoints and WebSocket streaming.
/// </summary>
/// <remarks>
/// This is the main entry point for the OpenAlgo .NET SDK. Create an instance of this class
/// to access all trading, market data, account, options, and streaming functionality.
///
/// Example usage:
/// <code>
/// var client = new Api(apiKey: "your_api_key");
///
/// // Place an order
/// var response = client.PlaceOrder(
///     strategy: "CSharp",
///     symbol: "RELIANCE",
///     action: "BUY",
///     exchange: "NSE",
///     priceType: "MARKET",
///     product: "MIS",
///     quantity: 1
/// );
///
/// // Get quotes
/// var quotes = client.Quotes("RELIANCE", "NSE");
///
/// // WebSocket streaming
/// client.Connect();
/// client.SubscribeLtp(new List&lt;Instrument&gt; { new() { Symbol = "RELIANCE", Exchange = "NSE" } });
/// </code>
/// </remarks>
public sealed class Api : Feed.FeedApi
{
    /// <summary>
    /// Initializes a new instance of the OpenAlgo API client.
    /// </summary>
    /// <param name="apiKey">OpenAlgo API key. Required.</param>
    /// <param name="host">OpenAlgo server URL. Defaults to "http://127.0.0.1:5000".</param>
    /// <param name="version">API version. Defaults to "v1".</param>
    /// <param name="timeout">HTTP request timeout in seconds. Defaults to 120.0.</param>
    /// <param name="wsPort">WebSocket port. Defaults to 8765.</param>
    /// <param name="wsUrl">Custom WebSocket URL. Optional, auto-generated from host if not specified.</param>
    /// <param name="verbose">Verbosity level (0=silent, 1=basic, 2=debug). Defaults to 0.</param>
    /// <example>
    /// <code>
    /// // Basic initialization
    /// var client = new Api(apiKey: "your_api_key");
    ///
    /// // Full initialization with all options
    /// var client = new Api(
    ///     apiKey: "your_api_key",
    ///     host: "http://127.0.0.1:5000",
    ///     version: "v1",
    ///     timeout: 120.0,
    ///     wsPort: 8765,
    ///     verbose: 1
    /// );
    /// </code>
    /// </example>
    public Api(
        string apiKey,
        string host = "http://127.0.0.1:5000",
        string version = "v1",
        double timeout = 120.0,
        int wsPort = 8765,
        string? wsUrl = null,
        int verbose = 0)
        : base(apiKey, host, version, timeout, wsPort, wsUrl, verbose)
    {
    }
}
