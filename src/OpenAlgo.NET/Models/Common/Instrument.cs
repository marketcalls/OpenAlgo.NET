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
