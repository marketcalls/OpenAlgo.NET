using System.Text.Json.Serialization;

namespace OpenAlgo.NET.Models.Responses;

/// <summary>
/// Response for funds operations.
/// </summary>
public class FundsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public FundsData? Data { get; set; }
}

/// <summary>
/// Funds data.
/// </summary>
public class FundsData
{
    [JsonPropertyName("availablecash")]
    public string? AvailableCash { get; set; }

    [JsonPropertyName("collateral")]
    public string? Collateral { get; set; }

    [JsonPropertyName("m2mrealized")]
    public string? M2MRealized { get; set; }

    [JsonPropertyName("m2munrealized")]
    public string? M2MUnrealized { get; set; }

    [JsonPropertyName("utiliseddebits")]
    public string? UtilisedDebits { get; set; }
}

/// <summary>
/// Response for order book operations.
/// </summary>
public class OrderBookResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public OrderBookData? Data { get; set; }
}

/// <summary>
/// Order book data.
/// </summary>
public class OrderBookData
{
    [JsonPropertyName("orders")]
    public List<OrderBookEntry>? Orders { get; set; }

    [JsonPropertyName("statistics")]
    public OrderBookStatistics? Statistics { get; set; }
}

/// <summary>
/// Order book entry.
/// </summary>
public class OrderBookEntry
{
    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("pricetype")]
    public string? PriceType { get; set; }

    [JsonPropertyName("order_status")]
    public string? OrderStatus { get; set; }

    [JsonPropertyName("trigger_price")]
    public decimal TriggerPrice { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }
}

/// <summary>
/// Order book statistics.
/// </summary>
public class OrderBookStatistics
{
    [JsonPropertyName("total_buy_orders")]
    public decimal TotalBuyOrders { get; set; }

    [JsonPropertyName("total_sell_orders")]
    public decimal TotalSellOrders { get; set; }

    [JsonPropertyName("total_completed_orders")]
    public decimal TotalCompletedOrders { get; set; }

    [JsonPropertyName("total_open_orders")]
    public decimal TotalOpenOrders { get; set; }

    [JsonPropertyName("total_rejected_orders")]
    public decimal TotalRejectedOrders { get; set; }
}

/// <summary>
/// Response for trade book operations.
/// </summary>
public class TradeBookResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<TradeBookEntry>? Data { get; set; }
}

/// <summary>
/// Trade book entry.
/// </summary>
public class TradeBookEntry
{
    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("average_price")]
    public decimal AveragePrice { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("trade_value")]
    public decimal TradeValue { get; set; }
}

/// <summary>
/// Response for position book operations.
/// </summary>
public class PositionBookResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<PositionBookEntry>? Data { get; set; }
}

/// <summary>
/// Position book entry.
/// </summary>
public class PositionBookEntry
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }

    [JsonPropertyName("average_price")]
    public string? AveragePrice { get; set; }

    [JsonPropertyName("ltp")]
    public string? Ltp { get; set; }

    [JsonPropertyName("pnl")]
    public string? Pnl { get; set; }
}

/// <summary>
/// Response for holdings operations.
/// </summary>
public class HoldingsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public HoldingsData? Data { get; set; }
}

/// <summary>
/// Holdings data.
/// </summary>
public class HoldingsData
{
    [JsonPropertyName("holdings")]
    public List<HoldingEntry>? Holdings { get; set; }

    [JsonPropertyName("statistics")]
    public HoldingsStatistics? Statistics { get; set; }
}

/// <summary>
/// Holding entry.
/// </summary>
public class HoldingEntry
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("pnl")]
    public decimal Pnl { get; set; }

    [JsonPropertyName("pnlpercent")]
    public decimal PnlPercent { get; set; }
}

/// <summary>
/// Holdings statistics.
/// </summary>
public class HoldingsStatistics
{
    [JsonPropertyName("totalholdingvalue")]
    public decimal TotalHoldingValue { get; set; }

    [JsonPropertyName("totalinvvalue")]
    public decimal TotalInvValue { get; set; }

    [JsonPropertyName("totalprofitandloss")]
    public decimal TotalProfitAndLoss { get; set; }

    [JsonPropertyName("totalpnlpercentage")]
    public decimal TotalPnlPercentage { get; set; }
}

/// <summary>
/// Response for analyzer status operations.
/// </summary>
public class AnalyzerStatusResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public AnalyzerData? Data { get; set; }
}

/// <summary>
/// Analyzer data.
/// </summary>
public class AnalyzerData
{
    [JsonPropertyName("analyze_mode")]
    public bool AnalyzeMode { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("total_logs")]
    public int TotalLogs { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>
/// Response for margin operations.
/// </summary>
public class MarginResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public MarginData? Data { get; set; }
}

/// <summary>
/// Margin data.
/// </summary>
public class MarginData
{
    [JsonPropertyName("total_margin_required")]
    public decimal TotalMarginRequired { get; set; }

    [JsonPropertyName("span_margin")]
    public decimal SpanMargin { get; set; }

    [JsonPropertyName("exposure_margin")]
    public decimal ExposureMargin { get; set; }
}

/// <summary>
/// Response for holidays operations.
/// </summary>
public class HolidaysResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<HolidayData>? Data { get; set; }
}

/// <summary>
/// Holiday data.
/// </summary>
public class HolidayData
{
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("holiday_type")]
    public string? HolidayType { get; set; }

    [JsonPropertyName("closed_exchanges")]
    public List<string>? ClosedExchanges { get; set; }

    [JsonPropertyName("open_exchanges")]
    public List<OpenExchangeData>? OpenExchanges { get; set; }
}

/// <summary>
/// Open exchange data for holidays.
/// </summary>
public class OpenExchangeData
{
    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }
}

/// <summary>
/// Response for timings operations.
/// </summary>
public class TimingsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<TimingData>? Data { get; set; }
}

/// <summary>
/// Timing data.
/// </summary>
public class TimingData
{
    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }
}
