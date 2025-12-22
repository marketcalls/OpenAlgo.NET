using System.Text.Json.Serialization;

namespace OpenAlgo.NET.Models.Responses;

/// <summary>
/// Response for order placement operations.
/// </summary>
public class OrderResponse : BaseResponse
{
    /// <summary>
    /// Order ID returned by the broker.
    /// </summary>
    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }
}

/// <summary>
/// Response for basket order operations.
/// </summary>
public class BasketOrderResponse : BaseResponse
{
    /// <summary>
    /// Results for each order in the basket.
    /// </summary>
    [JsonPropertyName("results")]
    public List<BasketOrderResult>? Results { get; set; }
}

/// <summary>
/// Individual result for a basket order.
/// </summary>
public class BasketOrderResult
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>
/// Response for split order operations.
/// </summary>
public class SplitOrderResponse : BaseResponse
{
    [JsonPropertyName("split_size")]
    public int SplitSize { get; set; }

    [JsonPropertyName("total_quantity")]
    public int TotalQuantity { get; set; }

    [JsonPropertyName("results")]
    public List<SplitOrderResult>? Results { get; set; }
}

/// <summary>
/// Individual result for a split order.
/// </summary>
public class SplitOrderResult
{
    [JsonPropertyName("order_num")]
    public int OrderNum { get; set; }

    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

/// <summary>
/// Response for order status operations.
/// </summary>
public class OrderStatusResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public OrderStatusData? Data { get; set; }
}

/// <summary>
/// Order status data.
/// </summary>
public class OrderStatusData
{
    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("average_price")]
    public decimal AveragePrice { get; set; }

    [JsonPropertyName("exchange")]
    public string? Exchange { get; set; }

    [JsonPropertyName("order_status")]
    public string? OrderStatus { get; set; }

    [JsonPropertyName("orderid")]
    public string? OrderId { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("pricetype")]
    public string? PriceType { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("trigger_price")]
    public decimal TriggerPrice { get; set; }
}

/// <summary>
/// Response for open position operations.
/// </summary>
public class OpenPositionResponse : BaseResponse
{
    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }
}

/// <summary>
/// Response for cancel all order operations.
/// </summary>
public class CancelAllOrderResponse : BaseResponse
{
    [JsonPropertyName("canceled_orders")]
    public List<string>? CanceledOrders { get; set; }

    [JsonPropertyName("failed_cancellations")]
    public List<string>? FailedCancellations { get; set; }
}

/// <summary>
/// Response for close position operations.
/// </summary>
public class ClosePositionResponse : BaseResponse
{
}
