using System.Net.Http.Json;
using System.Text.Json;

namespace OpenAlgo.NET;

/// <summary>
/// Standalone TradingView-style webhook poster for OpenAlgo strategies.
/// </summary>
/// <remarks>
/// Unlike <see cref="Api"/>, <see cref="Strategy"/> does not talk to the REST
/// <c>/api/v1/</c> surface or require an API key. It posts directly to a
/// strategy's webhook URL (<c>{host}/strategy/webhook/{webhookId}</c>) that you
/// configure on the OpenAlgo "Strategies" page. The strategy mode
/// (LONG_ONLY, SHORT_ONLY, BOTH) is configured on that page, not in this client.
///
/// Example usage:
/// <code>
/// using var strategy = new Strategy(hostUrl: "http://127.0.0.1:5000", webhookId: "your-webhook-id");
/// var response = await strategy.StrategyOrderAsync(symbol: "RELIANCE", action: "BUY");
/// </code>
/// </remarks>
public sealed class Strategy : IDisposable
{
    private readonly string _hostUrl;
    private readonly string _webhookId;
    private readonly HttpClient _client;
    private string? _webhookUrl;

    /// <summary>
    /// Initializes a new instance of the Strategy class.
    /// </summary>
    /// <param name="hostUrl">OpenAlgo server URL (e.g. "http://127.0.0.1:5000").</param>
    /// <param name="webhookId">Strategy's webhook ID from OpenAlgo.</param>
    public Strategy(string hostUrl, string webhookId)
    {
        if (hostUrl == null) throw new ArgumentNullException(nameof(hostUrl));
        _hostUrl = hostUrl.TrimEnd('/');
        _webhookId = webhookId ?? throw new ArgumentNullException(nameof(webhookId));
        // Reuse one keep-alive connection for repeated webhook posts instead of
        // opening a fresh socket per signal.
        _client = new HttpClient();
    }

    /// <summary>
    /// Cached webhook URL, built once from the host URL and webhook ID.
    /// </summary>
    public string WebhookUrl => _webhookUrl ??= $"{_hostUrl}/strategy/webhook/{_webhookId}";

    /// <summary>
    /// Send a strategy order via webhook to OpenAlgo (async).
    /// The strategy mode (LONG_ONLY, SHORT_ONLY, BOTH) is configured in OpenAlgo.
    /// </summary>
    /// <param name="symbol">Trading symbol (e.g. "RELIANCE", "NIFTY"). Required.</param>
    /// <param name="action">Order action ("BUY" or "SELL"). Required.</param>
    /// <param name="positionSize">Position size, required for BOTH mode. Optional.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The JSON response from the webhook request, as a <see cref="JsonElement"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown if the webhook request fails or returns a non-success status code.</exception>
    public async Task<JsonElement> StrategyOrderAsync(
        string symbol,
        string action,
        int? positionSize = null,
        CancellationToken cancellationToken = default)
    {
        var postMessage = new Dictionary<string, object?>
        {
            ["symbol"] = symbol,
            ["action"] = action.ToUpperInvariant()
        };

        if (positionSize.HasValue)
        {
            postMessage["position_size"] = positionSize.Value.ToString();
        }

        var response = await _client.PostAsJsonAsync(WebhookUrl, postMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(content);
        return doc.RootElement.Clone();
    }

    /// <summary>
    /// Send a strategy order via webhook to OpenAlgo (sync).
    /// </summary>
    public JsonElement StrategyOrder(string symbol, string action, int? positionSize = null)
    {
        return StrategyOrderAsync(symbol, action, positionSize).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Close the underlying HTTP client and release pooled connections.
    /// </summary>
    public void Dispose()
    {
        _client.Dispose();
    }
}
