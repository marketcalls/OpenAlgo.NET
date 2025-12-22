using System.Text.Json.Serialization;

namespace OpenAlgo.NET.Models.Responses;

/// <summary>
/// Response for quotes operations.
/// </summary>
public class QuotesResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public QuoteData? Data { get; set; }
}

/// <summary>
/// Quote data.
/// </summary>
public class QuoteData
{
    [JsonPropertyName("open")]
    public decimal Open { get; set; }

    [JsonPropertyName("high")]
    public decimal High { get; set; }

    [JsonPropertyName("low")]
    public decimal Low { get; set; }

    [JsonPropertyName("ltp")]
    public decimal Ltp { get; set; }

    [JsonPropertyName("ask")]
    public decimal Ask { get; set; }

    [JsonPropertyName("bid")]
    public decimal Bid { get; set; }

    [JsonPropertyName("prev_close")]
    public decimal PrevClose { get; set; }

    [JsonPropertyName("volume")]
    public long Volume { get; set; }

    [JsonPropertyName("oi")]
    public long Oi { get; set; }
}

/// <summary>
/// Response for multi quotes operations.
/// </summary>
public class MultiQuotesResponse : BaseResponse
{
    [JsonPropertyName("results")]
    public List<MultiQuoteResult>? Results { get; set; }
}

/// <summary>
/// Individual result for multi quotes.
/// </summary>
public class MultiQuoteResult
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("data")]
    public QuoteData? Data { get; set; }
}

/// <summary>
/// Response for market depth operations.
/// </summary>
public class DepthResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public DepthData? Data { get; set; }
}

/// <summary>
/// Market depth data.
/// </summary>
public class DepthData
{
    [JsonPropertyName("open")]
    public decimal Open { get; set; }

    [JsonPropertyName("high")]
    public decimal High { get; set; }

    [JsonPropertyName("low")]
    public decimal Low { get; set; }

    [JsonPropertyName("ltp")]
    public decimal Ltp { get; set; }

    [JsonPropertyName("ltq")]
    public int Ltq { get; set; }

    [JsonPropertyName("prev_close")]
    public decimal PrevClose { get; set; }

    [JsonPropertyName("volume")]
    public long Volume { get; set; }

    [JsonPropertyName("oi")]
    public long Oi { get; set; }

    [JsonPropertyName("totalbuyqty")]
    public long TotalBuyQty { get; set; }

    [JsonPropertyName("totalsellqty")]
    public long TotalSellQty { get; set; }

    [JsonPropertyName("asks")]
    public List<DepthLevel>? Asks { get; set; }

    [JsonPropertyName("bids")]
    public List<DepthLevel>? Bids { get; set; }
}

/// <summary>
/// Individual depth level.
/// </summary>
public class DepthLevel
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("orders")]
    public int Orders { get; set; }
}

/// <summary>
/// Response for history operations.
/// </summary>
public class HistoryResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<HistoryCandle>? Data { get; set; }
}

/// <summary>
/// Historical candle data.
/// </summary>
public class HistoryCandle
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("open")]
    public decimal Open { get; set; }

    [JsonPropertyName("high")]
    public decimal High { get; set; }

    [JsonPropertyName("low")]
    public decimal Low { get; set; }

    [JsonPropertyName("close")]
    public decimal Close { get; set; }

    [JsonPropertyName("volume")]
    public long Volume { get; set; }

    /// <summary>
    /// Converts the Unix timestamp to DateTime.
    /// </summary>
    [JsonIgnore]
    public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
}

/// <summary>
/// Response for intervals operations.
/// </summary>
public class IntervalsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public IntervalData? Data { get; set; }
}

/// <summary>
/// Interval data.
/// </summary>
public class IntervalData
{
    [JsonPropertyName("months")]
    public List<string>? Months { get; set; }

    [JsonPropertyName("weeks")]
    public List<string>? Weeks { get; set; }

    [JsonPropertyName("days")]
    public List<string>? Days { get; set; }

    [JsonPropertyName("hours")]
    public List<string>? Hours { get; set; }

    [JsonPropertyName("minutes")]
    public List<string>? Minutes { get; set; }

    [JsonPropertyName("seconds")]
    public List<string>? Seconds { get; set; }
}

/// <summary>
/// Response for symbol operations.
/// </summary>
public class SymbolResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public SymbolData? Data { get; set; }
}

/// <summary>
/// Symbol data.
/// </summary>
public class SymbolData
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("brsymbol")]
    public string? BrSymbol { get; set; }

    [JsonPropertyName("brexchange")]
    public string? BrExchange { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("expiry")]
    public string? Expiry { get; set; }

    [JsonPropertyName("freeze_qty")]
    public int FreezeQty { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("instrumenttype")]
    public string? InstrumentType { get; set; }

    [JsonPropertyName("lotsize")]
    public int LotSize { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("strike")]
    public decimal Strike { get; set; }

    [JsonPropertyName("tick_size")]
    public decimal TickSize { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}

/// <summary>
/// Response for search operations.
/// </summary>
public class SearchResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<SymbolData>? Data { get; set; }
}

/// <summary>
/// Response for expiry operations.
/// </summary>
public class ExpiryResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<string>? Data { get; set; }
}

/// <summary>
/// Response for instruments operations.
/// </summary>
public class InstrumentsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<InstrumentData>? Data { get; set; }
}

/// <summary>
/// Instrument data.
/// </summary>
public class InstrumentData
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("brsymbol")]
    public string? BrSymbol { get; set; }

    [JsonPropertyName("brexchange")]
    public string? BrExchange { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("expiry")]
    public string? Expiry { get; set; }

    [JsonPropertyName("instrumenttype")]
    public string? InstrumentType { get; set; }

    [JsonPropertyName("lotsize")]
    public int LotSize { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("strike")]
    public decimal Strike { get; set; }

    [JsonPropertyName("tick_size")]
    public decimal TickSize { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}

/// <summary>
/// Response for synthetic future operations.
/// </summary>
public class SyntheticFutureResponse : BaseResponse
{
    [JsonPropertyName("underlying")]
    public string? Underlying { get; set; }

    [JsonPropertyName("underlying_ltp")]
    public decimal UnderlyingLtp { get; set; }

    [JsonPropertyName("expiry")]
    public string? Expiry { get; set; }

    [JsonPropertyName("atm_strike")]
    public decimal AtmStrike { get; set; }

    [JsonPropertyName("synthetic_future_price")]
    public decimal SyntheticFuturePrice { get; set; }
}
