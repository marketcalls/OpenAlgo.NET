using System.Text.Json.Serialization;

namespace OpenAlgo.NET.Models.Responses;

/// <summary>
/// Response for options order operations.
/// </summary>
public class OptionsOrderResponse : BaseResponse
{
    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("underlying")]
    public string? Underlying { get; set; }

    [JsonPropertyName("underlying_ltp")]
    public decimal UnderlyingLtp { get; set; }

    [JsonPropertyName("offset")]
    public string? Offset { get; set; }

    [JsonPropertyName("option_type")]
    public string? OptionType { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }
}

/// <summary>
/// Response for options multi order operations.
/// </summary>
public class OptionsMultiOrderResponse : BaseResponse
{
    [JsonPropertyName("underlying")]
    public string? Underlying { get; set; }

    [JsonPropertyName("underlying_ltp")]
    public decimal UnderlyingLtp { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("results")]
    public List<OptionsMultiOrderResult>? Results { get; set; }
}

/// <summary>
/// Individual result for options multi order.
/// </summary>
public class OptionsMultiOrderResult
{
    [JsonPropertyName("leg")]
    public int Leg { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("offset")]
    public string? Offset { get; set; }

    [JsonPropertyName("option_type")]
    public string? OptionType { get; set; }

    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>
/// Response for option symbol operations.
/// </summary>
public class OptionSymbolResponse : BaseResponse
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("lotsize")]
    public int LotSize { get; set; }

    [JsonPropertyName("tick_size")]
    public decimal TickSize { get; set; }

    [JsonPropertyName("freeze_qty")]
    public int FreezeQty { get; set; }

    [JsonPropertyName("underlying_ltp")]
    public decimal UnderlyingLtp { get; set; }
}

/// <summary>
/// Response for option chain operations.
/// </summary>
public class OptionChainResponse : BaseResponse
{
    [JsonPropertyName("underlying")]
    public string? Underlying { get; set; }

    [JsonPropertyName("underlying_ltp")]
    public decimal UnderlyingLtp { get; set; }

    [JsonPropertyName("expiry_date")]
    public string? ExpiryDate { get; set; }

    [JsonPropertyName("atm_strike")]
    public decimal AtmStrike { get; set; }

    [JsonPropertyName("chain")]
    public List<OptionChainStrike>? Chain { get; set; }
}

/// <summary>
/// Option chain strike data.
/// </summary>
public class OptionChainStrike
{
    [JsonPropertyName("strike")]
    public decimal Strike { get; set; }

    [JsonPropertyName("ce")]
    public OptionChainData? Ce { get; set; }

    [JsonPropertyName("pe")]
    public OptionChainData? Pe { get; set; }
}

/// <summary>
/// Option chain data for a single option.
/// </summary>
public class OptionChainData
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("ltp")]
    public decimal Ltp { get; set; }

    [JsonPropertyName("bid")]
    public decimal Bid { get; set; }

    [JsonPropertyName("ask")]
    public decimal Ask { get; set; }

    [JsonPropertyName("open")]
    public decimal Open { get; set; }

    [JsonPropertyName("high")]
    public decimal High { get; set; }

    [JsonPropertyName("low")]
    public decimal Low { get; set; }

    [JsonPropertyName("prev_close")]
    public decimal PrevClose { get; set; }

    [JsonPropertyName("volume")]
    public long Volume { get; set; }

    [JsonPropertyName("oi")]
    public long Oi { get; set; }

    [JsonPropertyName("lotsize")]
    public int LotSize { get; set; }

    [JsonPropertyName("tick_size")]
    public decimal TickSize { get; set; }
}

/// <summary>
/// Response for option greeks operations.
/// </summary>
public class OptionGreeksResponse : BaseResponse
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("underlying")]
    public string? Underlying { get; set; }

    [JsonPropertyName("strike")]
    public decimal Strike { get; set; }

    [JsonPropertyName("option_type")]
    public string? OptionType { get; set; }

    [JsonPropertyName("expiry_date")]
    public string? ExpiryDate { get; set; }

    [JsonPropertyName("days_to_expiry")]
    public decimal DaysToExpiry { get; set; }

    [JsonPropertyName("spot_price")]
    public decimal SpotPrice { get; set; }

    [JsonPropertyName("option_price")]
    public decimal OptionPrice { get; set; }

    [JsonPropertyName("interest_rate")]
    public decimal InterestRate { get; set; }

    [JsonPropertyName("implied_volatility")]
    public decimal ImpliedVolatility { get; set; }

    [JsonPropertyName("greeks")]
    public GreeksData? Greeks { get; set; }
}

/// <summary>
/// Greeks data.
/// </summary>
public class GreeksData
{
    [JsonPropertyName("delta")]
    public decimal Delta { get; set; }

    [JsonPropertyName("gamma")]
    public decimal Gamma { get; set; }

    [JsonPropertyName("theta")]
    public decimal Theta { get; set; }

    [JsonPropertyName("vega")]
    public decimal Vega { get; set; }

    [JsonPropertyName("rho")]
    public decimal Rho { get; set; }
}
