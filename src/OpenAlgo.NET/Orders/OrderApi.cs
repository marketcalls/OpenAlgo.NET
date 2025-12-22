using OpenAlgo.NET.Models.Common;
using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.Orders;

/// <summary>
/// Order management API methods for OpenAlgo.
/// </summary>
public abstract class OrderApi : BaseApi
{
    /// <summary>
    /// Initializes a new instance of the OrderApi class.
    /// </summary>
    protected OrderApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
        : base(apiKey, host, version, timeout)
    {
    }

    #region PlaceOrder

    /// <summary>
    /// Place an order with the given parameters (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="action">BUY or SELL. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="priceType">Type of price. Defaults to "MARKET".</param>
    /// <param name="product">Product type. Defaults to "MIS".</param>
    /// <param name="quantity">Quantity to trade. Defaults to 1.</param>
    /// <param name="price">Required for LIMIT orders.</param>
    /// <param name="triggerPrice">Required for SL and SL-M orders.</param>
    /// <param name="disclosedQuantity">Disclosed quantity.</param>
    /// <param name="target">Target price.</param>
    /// <param name="stoploss">Stoploss price.</param>
    /// <param name="trailingSl">Trailing stoploss points.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order response.</returns>
    public async Task<OrderResponse> PlaceOrderAsync(
        string symbol,
        string action,
        string exchange,
        string strategy = "Python",
        string priceType = "MARKET",
        string product = "MIS",
        int quantity = 1,
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        string? target = null,
        string? stoploss = null,
        string? trailingSl = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;
        payload["symbol"] = symbol;
        payload["action"] = action;
        payload["exchange"] = exchange;
        payload["pricetype"] = priceType;
        payload["product"] = product;
        payload["quantity"] = quantity.ToString();

        AddIfNotNull(payload, "price", price);
        AddIfNotNull(payload, "trigger_price", triggerPrice);
        AddIfNotNull(payload, "disclosed_quantity", disclosedQuantity);
        AddIfNotNull(payload, "target", target);
        AddIfNotNull(payload, "stoploss", stoploss);
        AddIfNotNull(payload, "trailing_sl", trailingSl);

        return await MakeRequestAsync<OrderResponse>("placeorder", payload, cancellationToken);
    }

    /// <summary>
    /// Place an order with the given parameters (sync).
    /// </summary>
    public OrderResponse PlaceOrder(
        string symbol,
        string action,
        string exchange,
        string strategy = "Python",
        string priceType = "MARKET",
        string product = "MIS",
        int quantity = 1,
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        string? target = null,
        string? stoploss = null,
        string? trailingSl = null)
    {
        return PlaceOrderAsync(symbol, action, exchange, strategy, priceType, product, quantity,
            price, triggerPrice, disclosedQuantity, target, stoploss, trailingSl).GetAwaiter().GetResult();
    }

    #endregion

    #region PlaceSmartOrder

    /// <summary>
    /// Place a smart order that considers the current position size (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="action">BUY or SELL. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="positionSize">Required position size. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="priceType">Type of price. Defaults to "MARKET".</param>
    /// <param name="product">Product type. Defaults to "MIS".</param>
    /// <param name="quantity">Quantity to trade. Defaults to 1.</param>
    /// <param name="price">Required for LIMIT orders.</param>
    /// <param name="triggerPrice">Required for SL and SL-M orders.</param>
    /// <param name="disclosedQuantity">Disclosed quantity.</param>
    /// <param name="target">Target price.</param>
    /// <param name="stoploss">Stoploss price.</param>
    /// <param name="trailingSl">Trailing stoploss points.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order response.</returns>
    public async Task<OrderResponse> PlaceSmartOrderAsync(
        string symbol,
        string action,
        string exchange,
        int positionSize,
        string strategy = "Python",
        string priceType = "MARKET",
        string product = "MIS",
        int quantity = 1,
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        string? target = null,
        string? stoploss = null,
        string? trailingSl = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;
        payload["symbol"] = symbol;
        payload["action"] = action;
        payload["exchange"] = exchange;
        payload["pricetype"] = priceType;
        payload["product"] = product;
        payload["quantity"] = quantity.ToString();
        payload["position_size"] = positionSize.ToString();

        AddIfNotNull(payload, "price", price);
        AddIfNotNull(payload, "trigger_price", triggerPrice);
        AddIfNotNull(payload, "disclosed_quantity", disclosedQuantity);
        AddIfNotNull(payload, "target", target);
        AddIfNotNull(payload, "stoploss", stoploss);
        AddIfNotNull(payload, "trailing_sl", trailingSl);

        return await MakeRequestAsync<OrderResponse>("placesmartorder", payload, cancellationToken);
    }

    /// <summary>
    /// Place a smart order that considers the current position size (sync).
    /// </summary>
    public OrderResponse PlaceSmartOrder(
        string symbol,
        string action,
        string exchange,
        int positionSize,
        string strategy = "Python",
        string priceType = "MARKET",
        string product = "MIS",
        int quantity = 1,
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        string? target = null,
        string? stoploss = null,
        string? trailingSl = null)
    {
        return PlaceSmartOrderAsync(symbol, action, exchange, positionSize, strategy, priceType, product,
            quantity, price, triggerPrice, disclosedQuantity, target, stoploss, trailingSl).GetAwaiter().GetResult();
    }

    #endregion

    #region BasketOrder

    /// <summary>
    /// Place multiple orders simultaneously (async).
    /// </summary>
    /// <param name="orders">List of order items. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Basket order response.</returns>
    public async Task<BasketOrderResponse> BasketOrderAsync(
        List<BasketOrderItem> orders,
        string strategy = "Python",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;

        // Process orders to ensure all numeric values are strings
        var processedOrders = orders.Select(order => new Dictionary<string, object?>
        {
            ["symbol"] = order.Symbol,
            ["exchange"] = order.Exchange,
            ["action"] = order.Action,
            ["quantity"] = order.Quantity.ToString(),
            ["pricetype"] = order.PriceType,
            ["product"] = order.Product,
            ["price"] = order.Price,
            ["trigger_price"] = order.TriggerPrice,
            ["disclosed_quantity"] = order.DisclosedQuantity
        }.Where(kv => kv.Value != null).ToDictionary(kv => kv.Key, kv => kv.Value)).ToList();

        payload["orders"] = processedOrders;

        return await MakeRequestAsync<BasketOrderResponse>("basketorder", payload, cancellationToken);
    }

    /// <summary>
    /// Place multiple orders simultaneously (sync).
    /// </summary>
    public BasketOrderResponse BasketOrder(List<BasketOrderItem> orders, string strategy = "Python")
    {
        return BasketOrderAsync(orders, strategy).GetAwaiter().GetResult();
    }

    #endregion

    #region SplitOrder

    /// <summary>
    /// Split a large order into multiple smaller orders (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="action">BUY or SELL. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="quantity">Total quantity to trade. Required.</param>
    /// <param name="splitsize">Size of each split order. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="priceType">Type of price. Defaults to "MARKET".</param>
    /// <param name="product">Product type. Defaults to "MIS".</param>
    /// <param name="price">Required for LIMIT orders.</param>
    /// <param name="triggerPrice">Required for SL and SL-M orders.</param>
    /// <param name="disclosedQuantity">Disclosed quantity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Split order response.</returns>
    public async Task<SplitOrderResponse> SplitOrderAsync(
        string symbol,
        string action,
        string exchange,
        int quantity,
        int splitsize,
        string strategy = "Python",
        string priceType = "MARKET",
        string product = "MIS",
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;
        payload["symbol"] = symbol;
        payload["action"] = action;
        payload["exchange"] = exchange;
        payload["quantity"] = quantity.ToString();
        payload["splitsize"] = splitsize.ToString();
        payload["pricetype"] = priceType;
        payload["product"] = product;

        AddIfNotNull(payload, "price", price);
        AddIfNotNull(payload, "trigger_price", triggerPrice);
        AddIfNotNull(payload, "disclosed_quantity", disclosedQuantity);

        return await MakeRequestAsync<SplitOrderResponse>("splitorder", payload, cancellationToken);
    }

    /// <summary>
    /// Split a large order into multiple smaller orders (sync).
    /// </summary>
    public SplitOrderResponse SplitOrder(
        string symbol,
        string action,
        string exchange,
        int quantity,
        int splitsize,
        string strategy = "Python",
        string priceType = "MARKET",
        string product = "MIS",
        string? price = null,
        string? triggerPrice = null,
        string? disclosedQuantity = null)
    {
        return SplitOrderAsync(symbol, action, exchange, quantity, splitsize, strategy, priceType,
            product, price, triggerPrice, disclosedQuantity).GetAwaiter().GetResult();
    }

    #endregion

    #region ModifyOrder

    /// <summary>
    /// Modify an existing order (async).
    /// </summary>
    /// <param name="orderId">The ID of the order to modify. Required.</param>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="action">BUY or SELL. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="product">Product type. Required.</param>
    /// <param name="quantity">Quantity to trade. Required.</param>
    /// <param name="price">New price for the order. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="priceType">Type of price. Defaults to "LIMIT".</param>
    /// <param name="disclosedQuantity">Disclosed quantity. Defaults to "0".</param>
    /// <param name="triggerPrice">Trigger price. Defaults to "0".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order response.</returns>
    public async Task<OrderResponse> ModifyOrderAsync(
        string orderId,
        string symbol,
        string action,
        string exchange,
        string product,
        int quantity,
        string price,
        string strategy = "Python",
        string priceType = "LIMIT",
        string disclosedQuantity = "0",
        string triggerPrice = "0",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["orderid"] = orderId;
        payload["strategy"] = strategy;
        payload["symbol"] = symbol;
        payload["action"] = action;
        payload["exchange"] = exchange;
        payload["pricetype"] = priceType;
        payload["product"] = product;
        payload["quantity"] = quantity.ToString();
        payload["price"] = price;
        payload["disclosed_quantity"] = disclosedQuantity;
        payload["trigger_price"] = triggerPrice;

        return await MakeRequestAsync<OrderResponse>("modifyorder", payload, cancellationToken);
    }

    /// <summary>
    /// Modify an existing order (sync).
    /// </summary>
    public OrderResponse ModifyOrder(
        string orderId,
        string symbol,
        string action,
        string exchange,
        string product,
        int quantity,
        string price,
        string strategy = "Python",
        string priceType = "LIMIT",
        string disclosedQuantity = "0",
        string triggerPrice = "0")
    {
        return ModifyOrderAsync(orderId, symbol, action, exchange, product, quantity, price,
            strategy, priceType, disclosedQuantity, triggerPrice).GetAwaiter().GetResult();
    }

    #endregion

    #region CancelOrder

    /// <summary>
    /// Cancel an existing order (async).
    /// </summary>
    /// <param name="orderId">The ID of the order to cancel. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order response.</returns>
    public async Task<OrderResponse> CancelOrderAsync(
        string orderId,
        string strategy = "Python",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["orderid"] = orderId;
        payload["strategy"] = strategy;

        return await MakeRequestAsync<OrderResponse>("cancelorder", payload, cancellationToken);
    }

    /// <summary>
    /// Cancel an existing order (sync).
    /// </summary>
    public OrderResponse CancelOrder(string orderId, string strategy = "Python")
    {
        return CancelOrderAsync(orderId, strategy).GetAwaiter().GetResult();
    }

    #endregion

    #region CancelAllOrder

    /// <summary>
    /// Cancel all orders for a given strategy (async).
    /// </summary>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cancel all order response.</returns>
    public async Task<CancelAllOrderResponse> CancelAllOrderAsync(
        string strategy = "Python",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;

        return await MakeRequestAsync<CancelAllOrderResponse>("cancelallorder", payload, cancellationToken);
    }

    /// <summary>
    /// Cancel all orders for a given strategy (sync).
    /// </summary>
    public CancelAllOrderResponse CancelAllOrder(string strategy = "Python")
    {
        return CancelAllOrderAsync(strategy).GetAwaiter().GetResult();
    }

    #endregion

    #region ClosePosition

    /// <summary>
    /// Close all open positions for a given strategy (async).
    /// </summary>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Close position response.</returns>
    public async Task<ClosePositionResponse> ClosePositionAsync(
        string strategy = "Python",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;

        return await MakeRequestAsync<ClosePositionResponse>("closeposition", payload, cancellationToken);
    }

    /// <summary>
    /// Close all open positions for a given strategy (sync).
    /// </summary>
    public ClosePositionResponse ClosePosition(string strategy = "Python")
    {
        return ClosePositionAsync(strategy).GetAwaiter().GetResult();
    }

    #endregion

    #region OrderStatus

    /// <summary>
    /// Get the current status of an order (async).
    /// </summary>
    /// <param name="orderId">The ID of the order to check. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order status response.</returns>
    public async Task<OrderStatusResponse> OrderStatusAsync(
        string orderId,
        string strategy = "Python",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["orderid"] = orderId;
        payload["strategy"] = strategy;

        return await MakeRequestAsync<OrderStatusResponse>("orderstatus", payload, cancellationToken);
    }

    /// <summary>
    /// Get the current status of an order (sync).
    /// </summary>
    public OrderStatusResponse OrderStatus(string orderId, string strategy = "Python")
    {
        return OrderStatusAsync(orderId, strategy).GetAwaiter().GetResult();
    }

    #endregion

    #region OpenPosition

    /// <summary>
    /// Get the current open position for a specific symbol (async).
    /// </summary>
    /// <param name="symbol">Trading symbol. Required.</param>
    /// <param name="exchange">Exchange code. Required.</param>
    /// <param name="product">Product type. Required.</param>
    /// <param name="strategy">The trading strategy name. Defaults to "Python".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Open position response.</returns>
    public async Task<OpenPositionResponse> OpenPositionAsync(
        string symbol,
        string exchange,
        string product,
        string strategy = "Python",
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["strategy"] = strategy;
        payload["symbol"] = symbol;
        payload["exchange"] = exchange;
        payload["product"] = product;

        return await MakeRequestAsync<OpenPositionResponse>("openposition", payload, cancellationToken);
    }

    /// <summary>
    /// Get the current open position for a specific symbol (sync).
    /// </summary>
    public OpenPositionResponse OpenPosition(
        string symbol,
        string exchange,
        string product,
        string strategy = "Python")
    {
        return OpenPositionAsync(symbol, exchange, product, strategy).GetAwaiter().GetResult();
    }

    #endregion
}
