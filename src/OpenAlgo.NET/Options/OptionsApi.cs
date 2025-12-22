using OpenAlgo.NET.Models.Common;
using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.Options;

/// <summary>
/// Options API methods for OpenAlgo.
/// </summary>
public abstract class OptionsApi : Account.AccountApi
{
    /// <summary>
    /// Initializes a new instance of the OptionsApi class.
    /// </summary>
    protected OptionsApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
        : base(apiKey, host, version, timeout)
    {
    }

    #region OptionsOrder

    /// <summary>
    /// Place Option Orders by Auto-Resolving Symbol based on Underlying and Offset (async).
    /// </summary>
    /// <param name="underlying">Underlying symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="offset">Strike offset (ATM, ITM1-ITM50, OTM1-OTM50). Required.</param>
    /// <param name="optionType">Option type (CE or PE). Required.</param>
    /// <param name="action">BUY or SELL. Required.</param>
    /// <param name="quantity">Quantity (must be multiple of lot size). Required.</param>
    /// <param name="strategy">Strategy name. Defaults to "Python".</param>
    /// <param name="expiryDate">Expiry date in DDMMMYY format. Optional if underlying includes expiry.</param>
    /// <param name="priceType">Price type (MARKET/LIMIT/SL/SL-M). Defaults to "MARKET".</param>
    /// <param name="product">Product type (MIS/NRML). Defaults to "MIS".</param>
    /// <param name="strikeInt">DEPRECATED - Strike interval. Will be removed in future versions.</param>
    /// <param name="price">Required for LIMIT orders.</param>
    /// <param name="triggerPrice">Required for SL and SL-M orders.</param>
    /// <param name="disclosedQuantity">Disclosed quantity.</param>
    /// <param name="splitsize">Split size for large orders. Defaults to 0.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Options order response.</returns>
    public async Task<OptionsOrderResponse> OptionsOrderAsync(
        string underlying,
        string exchange,
        string offset,
        string optionType,
        string action,
        int quantity,
        string strategy = "Python",
        string? expiryDate = null,
        string priceType = "MARKET",
        string product = "MIS",
        int? strikeInt = null,
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        int splitsize = 0,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;
        payload["underlying"] = underlying;
        payload["exchange"] = exchange;
        payload["offset"] = offset;
        payload["option_type"] = optionType;
        payload["action"] = action;
        payload["quantity"] = quantity.ToString();
        payload["pricetype"] = priceType;
        payload["product"] = product;

        if (!string.IsNullOrEmpty(expiryDate))
        {
            payload["expiry_date"] = expiryDate;
        }

        if (strikeInt.HasValue)
        {
            payload["strike_int"] = strikeInt.Value.ToString();
        }

        if (splitsize > 0)
        {
            payload["splitsize"] = splitsize.ToString();
        }

        AddIfNotNull(payload, "price", price);
        AddIfNotNull(payload, "trigger_price", triggerPrice);
        AddIfNotNull(payload, "disclosed_quantity", disclosedQuantity);

        return await MakeRequestAsync<OptionsOrderResponse>("optionsorder", payload, cancellationToken);
    }

    /// <summary>
    /// Place Option Orders by Auto-Resolving Symbol based on Underlying and Offset (sync).
    /// </summary>
    public OptionsOrderResponse OptionsOrder(
        string underlying,
        string exchange,
        string offset,
        string optionType,
        string action,
        int quantity,
        string strategy = "Python",
        string? expiryDate = null,
        string priceType = "MARKET",
        string product = "MIS",
        int? strikeInt = null,
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        int splitsize = 0)
    {
        return OptionsOrderAsync(underlying, exchange, offset, optionType, action, quantity,
            strategy, expiryDate, priceType, product, strikeInt, price, triggerPrice,
            disclosedQuantity, splitsize).GetAwaiter().GetResult();
    }

    #endregion

    #region OptionsMultiOrder

    /// <summary>
    /// Place Multiple Option Legs with Common Underlying by Auto-Resolving Symbols (async).
    /// BUY legs are executed first for margin efficiency, then SELL legs.
    /// </summary>
    /// <param name="strategy">Strategy name. Required.</param>
    /// <param name="underlying">Underlying symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="legs">Array of leg objects (1-20 legs). Required.</param>
    /// <param name="expiryDate">Expiry date in DDMMMYY format. Optional if underlying includes expiry.</param>
    /// <param name="strikeInt">DEPRECATED - Strike interval. Will be removed in future versions.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Options multi order response.</returns>
    public async Task<OptionsMultiOrderResponse> OptionsMultiOrderAsync(
        string strategy,
        string underlying,
        string exchange,
        List<OptionLeg> legs,
        string? expiryDate = null,
        int? strikeInt = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;
        payload["underlying"] = underlying;
        payload["exchange"] = exchange;

        // Process legs
        var processedLegs = legs.Select(leg =>
        {
            var processedLeg = new Dictionary<string, object?>
            {
                ["offset"] = leg.Offset,
                ["option_type"] = leg.OptionType,
                ["action"] = leg.Action,
                ["quantity"] = leg.Quantity
            };

            if (!string.IsNullOrEmpty(leg.ExpiryDate))
                processedLeg["expiry_date"] = leg.ExpiryDate;
            if (!string.IsNullOrEmpty(leg.PriceType))
                processedLeg["pricetype"] = leg.PriceType;
            if (!string.IsNullOrEmpty(leg.Product))
                processedLeg["product"] = leg.Product;
            if (leg.Price.HasValue)
                processedLeg["price"] = leg.Price.Value;
            if (leg.TriggerPrice.HasValue)
                processedLeg["trigger_price"] = leg.TriggerPrice.Value;
            if (leg.DisclosedQuantity.HasValue)
                processedLeg["disclosed_quantity"] = leg.DisclosedQuantity.Value;

            return processedLeg;
        }).ToList();

        payload["legs"] = processedLegs;

        if (!string.IsNullOrEmpty(expiryDate))
        {
            payload["expiry_date"] = expiryDate;
        }

        if (strikeInt.HasValue)
        {
            payload["strike_int"] = strikeInt.Value;
        }

        return await MakeRequestAsync<OptionsMultiOrderResponse>("optionsmultiorder", payload, cancellationToken);
    }

    /// <summary>
    /// Place Multiple Option Legs with Common Underlying by Auto-Resolving Symbols (sync).
    /// </summary>
    public OptionsMultiOrderResponse OptionsMultiOrder(
        string strategy,
        string underlying,
        string exchange,
        List<OptionLeg> legs,
        string? expiryDate = null,
        int? strikeInt = null)
    {
        return OptionsMultiOrderAsync(strategy, underlying, exchange, legs, expiryDate, strikeInt).GetAwaiter().GetResult();
    }

    #endregion

    #region OptionSymbol

    /// <summary>
    /// Returns Option Symbol Details based on Underlying and Offset (async).
    /// </summary>
    /// <param name="underlying">Underlying symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="offset">Strike offset (ATM, ITM1-ITM50, OTM1-OTM50). Required.</param>
    /// <param name="optionType">Option type (CE or PE). Required.</param>
    /// <param name="expiryDate">Expiry date in DDMMMYY format. Optional if underlying includes expiry.</param>
    /// <param name="strategy">DEPRECATED - Strategy name. Will be removed in future versions.</param>
    /// <param name="strikeInt">DEPRECATED - Strike interval. Will be removed in future versions.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Option symbol response.</returns>
    public async Task<OptionSymbolResponse> OptionSymbolAsync(
        string underlying,
        string exchange,
        string offset,
        string optionType,
        string? expiryDate = null,
        string? strategy = null,
        int? strikeInt = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["underlying"] = underlying;
        payload["exchange"] = exchange;
        payload["offset"] = offset;
        payload["option_type"] = optionType;

        if (!string.IsNullOrEmpty(expiryDate))
        {
            payload["expiry_date"] = expiryDate;
        }

        if (!string.IsNullOrEmpty(strategy))
        {
            payload["strategy"] = strategy;
        }

        if (strikeInt.HasValue)
        {
            payload["strike_int"] = strikeInt.Value.ToString();
        }

        return await MakeRequestAsync<OptionSymbolResponse>("optionsymbol", payload, cancellationToken);
    }

    /// <summary>
    /// Returns Option Symbol Details based on Underlying and Offset (sync).
    /// </summary>
    public OptionSymbolResponse OptionSymbol(
        string underlying,
        string exchange,
        string offset,
        string optionType,
        string? expiryDate = null,
        string? strategy = null,
        int? strikeInt = null)
    {
        return OptionSymbolAsync(underlying, exchange, offset, optionType, expiryDate, strategy, strikeInt).GetAwaiter().GetResult();
    }

    #endregion

    #region OptionChain

    /// <summary>
    /// Fetch Option Chain Data with Real-time Quotes for All Strikes (async).
    /// </summary>
    /// <param name="underlying">Underlying symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="expiryDate">Expiry date in DDMMMYY format. Required unless underlying includes expiry.</param>
    /// <param name="strikeCount">Number of strikes above and below ATM (1-100). Returns all strikes if not specified.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Option chain response.</returns>
    public async Task<OptionChainResponse> OptionChainAsync(
        string underlying,
        string exchange,
        string? expiryDate = null,
        int? strikeCount = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["underlying"] = underlying;
        payload["exchange"] = exchange;

        if (!string.IsNullOrEmpty(expiryDate))
        {
            payload["expiry_date"] = expiryDate;
        }

        if (strikeCount.HasValue)
        {
            payload["strike_count"] = strikeCount.Value;
        }

        return await MakeRequestAsync<OptionChainResponse>("optionchain", payload, cancellationToken);
    }

    /// <summary>
    /// Fetch Option Chain Data with Real-time Quotes for All Strikes (sync).
    /// </summary>
    public OptionChainResponse OptionChain(
        string underlying,
        string exchange,
        string? expiryDate = null,
        int? strikeCount = null)
    {
        return OptionChainAsync(underlying, exchange, expiryDate, strikeCount).GetAwaiter().GetResult();
    }

    #endregion

    #region OptionGreeks

    /// <summary>
    /// Calculate Option Greeks (Delta, Gamma, Theta, Vega, Rho) and Implied Volatility using Black-76 Model (async).
    /// </summary>
    /// <param name="symbol">Option symbol. Required.</param>
    /// <param name="exchange">Exchange code (NFO, BFO, CDS, MCX). Required.</param>
    /// <param name="interestRate">Risk-free interest rate (annualized %). Default is 0.</param>
    /// <param name="forwardPrice">Custom forward/synthetic futures price. Optional.</param>
    /// <param name="underlyingSymbol">Custom underlying symbol. Auto-detected if not specified.</param>
    /// <param name="underlyingExchange">Custom underlying exchange. Auto-detected if not specified.</param>
    /// <param name="expiryTime">Custom expiry time in HH:MM format. Required for MCX contracts.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Option greeks response.</returns>
    public async Task<OptionGreeksResponse> OptionGreeksAsync(
        string symbol,
        string exchange,
        decimal? interestRate = null,
        decimal? forwardPrice = null,
        string? underlyingSymbol = null,
        string? underlyingExchange = null,
        string? expiryTime = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;

        if (interestRate.HasValue)
        {
            payload["interest_rate"] = interestRate.Value;
        }

        if (forwardPrice.HasValue)
        {
            payload["forward_price"] = forwardPrice.Value;
        }

        if (!string.IsNullOrEmpty(underlyingSymbol))
        {
            payload["underlying_symbol"] = underlyingSymbol;
        }

        if (!string.IsNullOrEmpty(underlyingExchange))
        {
            payload["underlying_exchange"] = underlyingExchange;
        }

        if (!string.IsNullOrEmpty(expiryTime))
        {
            payload["expiry_time"] = expiryTime;
        }

        return await MakeRequestAsync<OptionGreeksResponse>("optiongreeks", payload, cancellationToken);
    }

    /// <summary>
    /// Calculate Option Greeks (Delta, Gamma, Theta, Vega, Rho) and Implied Volatility using Black-76 Model (sync).
    /// </summary>
    public OptionGreeksResponse OptionGreeks(
        string symbol,
        string exchange,
        decimal? interestRate = null,
        decimal? forwardPrice = null,
        string? underlyingSymbol = null,
        string? underlyingExchange = null,
        string? expiryTime = null)
    {
        return OptionGreeksAsync(symbol, exchange, interestRate, forwardPrice,
            underlyingSymbol, underlyingExchange, expiryTime).GetAwaiter().GetResult();
    }

    #endregion
}
