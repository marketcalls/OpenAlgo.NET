using OpenAlgo.NET.Models.Common;
using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.Account;

/// <summary>
/// Account management API methods for OpenAlgo.
/// </summary>
public abstract class AccountApi : Data.DataApi
{
    /// <summary>
    /// Initializes a new instance of the AccountApi class.
    /// </summary>
    protected AccountApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
        : base(apiKey, host, version, timeout)
    {
    }

    #region Funds

    /// <summary>
    /// Get funds and margin details of the connected trading account (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Funds response.</returns>
    public async Task<FundsResponse> FundsAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<FundsResponse>("funds", payload, cancellationToken);
    }

    /// <summary>
    /// Get funds and margin details of the connected trading account (sync).
    /// </summary>
    public FundsResponse Funds()
    {
        return FundsAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region OrderBook

    /// <summary>
    /// Get orderbook details from the broker (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order book response.</returns>
    public async Task<OrderBookResponse> OrderBookAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<OrderBookResponse>("orderbook", payload, cancellationToken);
    }

    /// <summary>
    /// Get orderbook details from the broker (sync).
    /// </summary>
    public OrderBookResponse OrderBook()
    {
        return OrderBookAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region TradeBook

    /// <summary>
    /// Get tradebook details from the broker (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Trade book response.</returns>
    public async Task<TradeBookResponse> TradeBookAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<TradeBookResponse>("tradebook", payload, cancellationToken);
    }

    /// <summary>
    /// Get tradebook details from the broker (sync).
    /// </summary>
    public TradeBookResponse TradeBook()
    {
        return TradeBookAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region PositionBook

    /// <summary>
    /// Get positionbook details from the broker (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Position book response.</returns>
    public async Task<PositionBookResponse> PositionBookAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<PositionBookResponse>("positionbook", payload, cancellationToken);
    }

    /// <summary>
    /// Get positionbook details from the broker (sync).
    /// </summary>
    public PositionBookResponse PositionBook()
    {
        return PositionBookAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region Holdings

    /// <summary>
    /// Get stock holdings details from the broker (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Holdings response.</returns>
    public async Task<HoldingsResponse> HoldingsAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<HoldingsResponse>("holdings", payload, cancellationToken);
    }

    /// <summary>
    /// Get stock holdings details from the broker (sync).
    /// </summary>
    public HoldingsResponse Holdings()
    {
        return HoldingsAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region AnalyzerStatus

    /// <summary>
    /// Get analyzer status information (async).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Analyzer status response.</returns>
    public async Task<AnalyzerStatusResponse> AnalyzerStatusAsync(CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        return await MakeRequestAsync<AnalyzerStatusResponse>("analyzer", payload, cancellationToken);
    }

    /// <summary>
    /// Get analyzer status information (sync).
    /// </summary>
    public AnalyzerStatusResponse AnalyzerStatus()
    {
        return AnalyzerStatusAsync().GetAwaiter().GetResult();
    }

    #endregion

    #region AnalyzerToggle

    /// <summary>
    /// Toggle analyzer mode between analyze and live modes (async).
    /// </summary>
    /// <param name="mode">True for analyze mode (simulated), False for live mode.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Analyzer status response.</returns>
    public async Task<AnalyzerStatusResponse> AnalyzerToggleAsync(
        bool mode,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["mode"] = mode;

        return await MakeRequestAsync<AnalyzerStatusResponse>("analyzer/toggle", payload, cancellationToken);
    }

    /// <summary>
    /// Toggle analyzer mode between analyze and live modes (sync).
    /// </summary>
    public AnalyzerStatusResponse AnalyzerToggle(bool mode)
    {
        return AnalyzerToggleAsync(mode).GetAwaiter().GetResult();
    }

    #endregion

    #region Margin

    /// <summary>
    /// Calculate margin requirements for a basket of positions (async).
    /// </summary>
    /// <param name="positions">List of position dictionaries (max: 50 positions). Required.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Margin response.</returns>
    public async Task<MarginResponse> MarginAsync(
        List<MarginPosition> positions,
        CancellationToken cancellationToken = default)
    {
        // Validate positions
        if (positions == null || positions.Count == 0)
        {
            return new MarginResponse
            {
                Status = "error",
                Message = "Positions array cannot be empty",
                ErrorType = "validation_error"
            };
        }

        if (positions.Count > 50)
        {
            return new MarginResponse
            {
                Status = "error",
                Message = "Maximum 50 positions allowed",
                ErrorType = "validation_error"
            };
        }

        var payload = CreatePayload();

        // Process positions
        var processedPositions = positions.Select(p => new Dictionary<string, object?>
        {
            ["symbol"] = p.Symbol,
            ["exchange"] = p.Exchange,
            ["action"] = p.Action,
            ["product"] = p.Product,
            ["pricetype"] = p.PriceType,
            ["quantity"] = p.Quantity,
            ["price"] = p.Price ?? "0",
            ["trigger_price"] = p.TriggerPrice ?? "0"
        }).ToList();

        payload["positions"] = processedPositions;

        return await MakeRequestAsync<MarginResponse>("margin", payload, cancellationToken);
    }

    /// <summary>
    /// Calculate margin requirements for a basket of positions (sync).
    /// </summary>
    public MarginResponse Margin(List<MarginPosition> positions)
    {
        return MarginAsync(positions).GetAwaiter().GetResult();
    }

    #endregion
}
