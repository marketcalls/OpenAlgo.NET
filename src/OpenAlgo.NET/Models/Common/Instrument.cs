using System.Text.Json.Serialization;

namespace OpenAlgo.NET.Models.Common;

/// <summary>
/// Instrument for WebSocket subscription.
/// </summary>
public class Instrument
{
    /// <summary>
    /// Exchange code (e.g., NSE, BSE, NFO).
    /// </summary>
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Trading symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Exchange token (optional).
    /// </summary>
    [JsonPropertyName("exchange_token")]
    public string? ExchangeToken { get; set; }
}

/// <summary>
/// Symbol-exchange pair for multi quotes.
/// </summary>
public class SymbolExchangePair
{
    /// <summary>
    /// Trading symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Exchange code.
    /// </summary>
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = string.Empty;
}

/// <summary>
/// Basket order item.
/// </summary>
public class BasketOrderItem
{
    /// <summary>
    /// Trading symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Exchange code.
    /// </summary>
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Action (BUY or SELL).
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Quantity to trade.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    /// <summary>
    /// Price type (MARKET, LIMIT, SL, SL-M).
    /// </summary>
    [JsonPropertyName("pricetype")]
    public string PriceType { get; set; } = "MARKET";

    /// <summary>
    /// Product type (MIS, NRML, CNC).
    /// </summary>
    [JsonPropertyName("product")]
    public string Product { get; set; } = "MIS";

    /// <summary>
    /// Price for LIMIT orders.
    /// </summary>
    [JsonPropertyName("price")]
    public string? Price { get; set; }

    /// <summary>
    /// Trigger price for SL orders.
    /// </summary>
    [JsonPropertyName("trigger_price")]
    public string? TriggerPrice { get; set; }

    /// <summary>
    /// Disclosed quantity.
    /// </summary>
    [JsonPropertyName("disclosed_quantity")]
    public string? DisclosedQuantity { get; set; }
}

/// <summary>
/// Option leg for multi-leg orders.
/// </summary>
public class OptionLeg
{
    /// <summary>
    /// Strike offset (ATM, ITM1-ITM50, OTM1-OTM50).
    /// </summary>
    [JsonPropertyName("offset")]
    public string Offset { get; set; } = string.Empty;

    /// <summary>
    /// Option type (CE or PE).
    /// </summary>
    [JsonPropertyName("option_type")]
    public string OptionType { get; set; } = string.Empty;

    /// <summary>
    /// Action (BUY or SELL).
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Quantity (must be multiple of lot size).
    /// </summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    /// <summary>
    /// Expiry date in DDMMMYY format (optional, for diagonal/calendar spreads).
    /// </summary>
    [JsonPropertyName("expiry_date")]
    public string? ExpiryDate { get; set; }

    /// <summary>
    /// Price type (MARKET, LIMIT, SL, SL-M).
    /// </summary>
    [JsonPropertyName("pricetype")]
    public string? PriceType { get; set; }

    /// <summary>
    /// Product type (MIS, NRML).
    /// </summary>
    [JsonPropertyName("product")]
    public string? Product { get; set; }

    /// <summary>
    /// Limit price for LIMIT orders.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Trigger price for SL orders.
    /// </summary>
    [JsonPropertyName("trigger_price")]
    public decimal? TriggerPrice { get; set; }

    /// <summary>
    /// Disclosed quantity.
    /// </summary>
    [JsonPropertyName("disclosed_quantity")]
    public int? DisclosedQuantity { get; set; }
}

/// <summary>
/// Smart order parameters, expressed as named/optional properties so every field
/// (including the required position size) can be set by name in any order — mirroring
/// the Python SDK's keyword-only
/// <c>placesmartorder(*, strategy="Python", symbol, action, exchange, price_type="MARKET",
/// product="MIS", quantity=1, position_size, **kwargs)</c> signature.
/// </summary>
public class PlaceSmartOrderRequest
{
    /// <summary>
    /// Trading symbol. Required.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// BUY or SELL. Required.
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Exchange code. Required.
    /// </summary>
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Required target position size. Required (no default, matching Python).
    /// </summary>
    [JsonPropertyName("position_size")]
    public int? PositionSize { get; set; }

    /// <summary>
    /// The trading strategy name. Defaults to "Python".
    /// </summary>
    [JsonPropertyName("strategy")]
    public string Strategy { get; set; } = "Python";

    /// <summary>
    /// Type of price. Defaults to "MARKET".
    /// </summary>
    [JsonPropertyName("pricetype")]
    public string PriceType { get; set; } = "MARKET";

    /// <summary>
    /// Product type. Defaults to "MIS".
    /// </summary>
    [JsonPropertyName("product")]
    public string Product { get; set; } = "MIS";

    /// <summary>
    /// Quantity to trade. Defaults to 1.
    /// </summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Required for LIMIT orders.
    /// </summary>
    [JsonPropertyName("price")]
    public string? Price { get; set; }

    /// <summary>
    /// Required for SL and SL-M orders.
    /// </summary>
    [JsonPropertyName("trigger_price")]
    public string? TriggerPrice { get; set; }

    /// <summary>
    /// Disclosed quantity.
    /// </summary>
    [JsonPropertyName("disclosed_quantity")]
    public string? DisclosedQuantity { get; set; }

    /// <summary>
    /// Target price.
    /// </summary>
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    /// <summary>
    /// Stoploss price.
    /// </summary>
    [JsonPropertyName("stoploss")]
    public string? Stoploss { get; set; }

    /// <summary>
    /// Trailing stoploss points.
    /// </summary>
    [JsonPropertyName("trailing_sl")]
    public string? TrailingSl { get; set; }
}

/// <summary>
/// Position for margin calculation.
/// </summary>
public class MarginPosition
{
    /// <summary>
    /// Trading symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Exchange code.
    /// </summary>
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Action (BUY or SELL).
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Product type (CNC, MIS, NRML).
    /// </summary>
    [JsonPropertyName("product")]
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Price type (MARKET, LIMIT, SL, SL-M).
    /// </summary>
    [JsonPropertyName("pricetype")]
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// Quantity to trade.
    /// </summary>
    [JsonPropertyName("quantity")]
    public string Quantity { get; set; } = string.Empty;

    /// <summary>
    /// Price for LIMIT orders.
    /// </summary>
    [JsonPropertyName("price")]
    public string? Price { get; set; }

    /// <summary>
    /// Trigger price for SL orders.
    /// </summary>
    [JsonPropertyName("trigger_price")]
    public string? TriggerPrice { get; set; }
}
