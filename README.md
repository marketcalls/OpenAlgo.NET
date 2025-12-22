# OpenAlgo.NET

Official .NET SDK for OpenAlgo - Algorithmic Trading Platform for Indian Markets.

## Installation

```bash
# .NET CLI
dotnet add package OpenAlgo.NET

# Package Manager
Install-Package OpenAlgo.NET
```

## Compatibility

| Framework | Version | Support |
|-----------|---------|---------|
| .NET | 6.0 | LTS (Long Term Support) |
| .NET | 7.0 | STS (Standard Term Support) |
| .NET | 8.0 | LTS (Long Term Support) - Recommended |

The SDK is built using only built-in .NET libraries with no external dependencies:
- `System.Net.Http` - HTTP client
- `System.Text.Json` - JSON serialization
- `System.Net.WebSockets.Client` - WebSocket streaming

## Version

```csharp
// Current Version: 1.0.0
```

## Getting Started

First, import the OpenAlgo namespace and initialize the client with your API key:

```csharp
using OpenAlgo.NET;

// Replace with your actual API key
// Specify the host URL with your hosted domain or ngrok domain
// If running locally in Windows then use the default host value
var client = new Api(apiKey: "your_api_key_here", host: "http://127.0.0.1:5000");
```

### Constructor Parameters

```csharp
var client = new Api(
    apiKey: "your_api_key",           // Required - OpenAlgo API key
    host: "http://127.0.0.1:5000",    // Optional - API host URL
    version: "v1",                     // Optional - API version
    timeout: 120.0,                    // Optional - Request timeout in seconds
    wsPort: 8765,                      // Optional - WebSocket port
    wsUrl: null,                       // Optional - Custom WebSocket URL
    verbose: 0                         // Optional - Verbosity (0=silent, 1=basic, 2=debug)
);
```

---

# Async API Reference

All methods are available in async versions with the `Async` suffix. Async methods are recommended for better performance and scalability.

## PlaceOrderAsync

Place an order with the given parameters.

```csharp
var response = await client.PlaceOrderAsync(
    symbol: "NHPC",
    action: "BUY",
    exchange: "NSE",
    strategy: "CSharp",           // Optional, default: "Python"
    priceType: "MARKET",          // Optional, default: "MARKET"
    product: "MIS",               // Optional, default: "MIS"
    quantity: 1                   // Optional, default: 1
);
Console.WriteLine($"Order ID: {response.OrderId}, Status: {response.Status}");
```

**Response:**
```json
{"orderid": "250408000989443", "status": "success"}
```

### Limit Order Example

```csharp
var response = await client.PlaceOrderAsync(
    symbol: "YESBANK",
    action: "BUY",
    exchange: "NSE",
    priceType: "LIMIT",
    product: "MIS",
    quantity: 1,
    price: "16",
    triggerPrice: "0",
    disclosedQuantity: "0"
);
```

### PlaceOrderAsync Parameters

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| symbol | string | Yes | - | Trading symbol |
| action | string | Yes | - | BUY or SELL |
| exchange | string | Yes | - | Exchange code (NSE, BSE, NFO, etc.) |
| strategy | string | No | "Python" | Strategy name |
| priceType | string | No | "MARKET" | MARKET, LIMIT, SL, SL-M |
| product | string | No | "MIS" | MIS, NRML, CNC |
| quantity | int | No | 1 | Quantity to trade |
| price | string | No | null | Required for LIMIT orders |
| triggerPrice | string | No | null | Required for SL/SL-M orders |
| disclosedQuantity | string | No | null | Disclosed quantity |
| target | string | No | null | Target price |
| stoploss | string | No | null | Stoploss price |
| trailingSl | string | No | null | Trailing stoploss points |

---

## PlaceSmartOrderAsync

Place a smart order considering the current position size.

```csharp
var response = await client.PlaceSmartOrderAsync(
    symbol: "TATAMOTORS",
    action: "SELL",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1,
    positionSize: 5
);
```

**Response:**
```json
{"orderid": "250408000997543", "status": "success"}
```

---

## OptionsOrderAsync

Place ATM/ITM/OTM options orders.

```csharp
// ATM Option Order
var response = await client.OptionsOrderAsync(
    underlying: "NIFTY",
    exchange: "NFO",
    offset: "ATM",
    optionType: "CE",
    action: "BUY",
    quantity: 75,
    strategy: "CSharp",
    expiryDate: "28OCT25",
    priceType: "MARKET",
    product: "NRML"
);
```

**Response:**
```json
{
  "exchange": "NFO",
  "offset": "ATM",
  "option_type": "CE",
  "orderid": "25102800000006",
  "status": "success",
  "symbol": "NIFTY28OCT2525950CE",
  "underlying": "NIFTY28OCT25FUT",
  "underlying_ltp": 25966.05
}
```

### ITM Option Order

```csharp
var response = await client.OptionsOrderAsync(
    underlying: "NIFTY",
    exchange: "NFO",
    offset: "ITM4",
    optionType: "PE",
    action: "BUY",
    quantity: 75,
    expiryDate: "28OCT25",
    priceType: "MARKET",
    product: "NRML"
);
```

### OTM Option Order

```csharp
var response = await client.OptionsOrderAsync(
    underlying: "NIFTY",
    exchange: "NFO",
    offset: "OTM5",
    optionType: "CE",
    action: "BUY",
    quantity: 75,
    expiryDate: "28OCT25",
    priceType: "MARKET",
    product: "NRML"
);
```

### OptionsOrderAsync Parameters

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| underlying | string | Yes | - | Underlying symbol (NIFTY, BANKNIFTY) |
| exchange | string | Yes | - | NFO, BFO |
| offset | string | Yes | - | ATM, ITM1-ITM50, OTM1-OTM50 |
| optionType | string | Yes | - | CE or PE |
| action | string | Yes | - | BUY or SELL |
| quantity | int | Yes | - | Quantity (multiple of lot size) |
| strategy | string | No | "Python" | Strategy name |
| expiryDate | string | No | null | Expiry date (DDMMMYY format) |
| priceType | string | No | "MARKET" | MARKET, LIMIT, SL, SL-M |
| product | string | No | "MIS" | MIS, NRML |
| price | string | No | null | Required for LIMIT orders |
| triggerPrice | string | No | null | Required for SL orders |
| splitsize | int | No | 0 | Split size for large orders |

---

## OptionsMultiOrderAsync

Place multiple option legs with common underlying (Iron Condor, Spreads, etc.).

### Iron Condor Example (Same Expiry)

```csharp
var legs = new List<OptionLeg>
{
    new() { Offset = "OTM6", OptionType = "CE", Action = "BUY", Quantity = 75 },
    new() { Offset = "OTM6", OptionType = "PE", Action = "BUY", Quantity = 75 },
    new() { Offset = "OTM4", OptionType = "CE", Action = "SELL", Quantity = 75 },
    new() { Offset = "OTM4", OptionType = "PE", Action = "SELL", Quantity = 75 }
};

var response = await client.OptionsMultiOrderAsync(
    strategy: "Iron Condor Test",
    underlying: "NIFTY",
    exchange: "NFO",
    legs: legs,
    expiryDate: "25NOV25"
);
```

**Response:**
```json
{
  "status": "success",
  "underlying": "NIFTY",
  "underlying_ltp": 26050.45,
  "results": [
    {"action": "BUY", "leg": 1, "offset": "OTM6", "option_type": "CE", "orderid": "25111996859688", "status": "success", "symbol": "NIFTY25NOV2526350CE"},
    {"action": "BUY", "leg": 2, "offset": "OTM6", "option_type": "PE", "orderid": "25111996042210", "status": "success", "symbol": "NIFTY25NOV2525750PE"},
    {"action": "SELL", "leg": 3, "offset": "OTM4", "option_type": "CE", "orderid": "25111922189638", "status": "success", "symbol": "NIFTY25NOV2526250CE"},
    {"action": "SELL", "leg": 4, "offset": "OTM4", "option_type": "PE", "orderid": "25111919252668", "status": "success", "symbol": "NIFTY25NOV2525850PE"}
  ]
}
```

### Diagonal Spread Example (Different Expiry)

```csharp
var legs = new List<OptionLeg>
{
    new() { Offset = "ITM2", OptionType = "CE", Action = "BUY", Quantity = 75, ExpiryDate = "30DEC25" },
    new() { Offset = "OTM2", OptionType = "CE", Action = "SELL", Quantity = 75, ExpiryDate = "25NOV25" }
};

var response = await client.OptionsMultiOrderAsync(
    strategy: "Diagonal Spread Test",
    underlying: "NIFTY",
    exchange: "NFO",
    legs: legs
);
```

### OptionsMultiOrderAsync Parameters

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| strategy | string | Yes | - | Strategy name |
| underlying | string | Yes | - | Underlying symbol |
| exchange | string | Yes | - | NFO, BFO |
| legs | List\<OptionLeg\> | Yes | - | Array of leg objects (1-20 legs) |
| expiryDate | string | No | null | Common expiry date (DDMMMYY) |

### OptionLeg Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| Offset | string | Yes | ATM, ITM1-ITM50, OTM1-OTM50 |
| OptionType | string | Yes | CE or PE |
| Action | string | Yes | BUY or SELL |
| Quantity | int | Yes | Quantity (multiple of lot size) |
| ExpiryDate | string | No | For diagonal/calendar spreads |
| PriceType | string | No | MARKET, LIMIT, SL, SL-M |
| Product | string | No | MIS, NRML |
| Price | decimal? | No | Limit price |
| TriggerPrice | decimal? | No | For SL orders |
| DisclosedQuantity | int? | No | Disclosed quantity |

---

## BasketOrderAsync

Place multiple orders in a single request.

```csharp
var orders = new List<BasketOrderItem>
{
    new() { Symbol = "BHEL", Exchange = "NSE", Action = "BUY", Quantity = 1, PriceType = "MARKET", Product = "MIS" },
    new() { Symbol = "ZOMATO", Exchange = "NSE", Action = "SELL", Quantity = 1, PriceType = "MARKET", Product = "MIS" }
};

var response = await client.BasketOrderAsync(orders);
```

**Response:**
```json
{
  "status": "success",
  "results": [
    {"symbol": "BHEL", "status": "success", "orderid": "250408000999544"},
    {"symbol": "ZOMATO", "status": "success", "orderid": "250408000997545"}
  ]
}
```

---

## SplitOrderAsync

Split large orders into smaller chunks.

```csharp
var response = await client.SplitOrderAsync(
    symbol: "YESBANK",
    exchange: "NSE",
    action: "SELL",
    quantity: 105,
    splitSize: 20,
    priceType: "MARKET",
    product: "MIS"
);
```

**Response:**
```json
{
  "status": "success",
  "split_size": 20,
  "total_quantity": 105,
  "results": [
    {"order_num": 1, "orderid": "250408001021467", "quantity": 20, "status": "success"},
    {"order_num": 2, "orderid": "250408001021459", "quantity": 20, "status": "success"},
    {"order_num": 3, "orderid": "250408001021466", "quantity": 20, "status": "success"},
    {"order_num": 4, "orderid": "250408001021470", "quantity": 20, "status": "success"},
    {"order_num": 5, "orderid": "250408001021471", "quantity": 20, "status": "success"},
    {"order_num": 6, "orderid": "250408001021472", "quantity": 5, "status": "success"}
  ]
}
```

---

## ModifyOrderAsync

Modify an existing order.

```csharp
var response = await client.ModifyOrderAsync(
    orderId: "250408001002736",
    symbol: "YESBANK",
    action: "BUY",
    exchange: "NSE",
    priceType: "LIMIT",
    product: "CNC",
    quantity: 1,
    price: "16.5"
);
```

**Response:**
```json
{"orderid": "250408001002736", "status": "success"}
```

---

## CancelOrderAsync

Cancel an existing order.

```csharp
var response = await client.CancelOrderAsync(orderId: "250408001002736");
```

**Response:**
```json
{"orderid": "250408001002736", "status": "success"}
```

---

## CancelAllOrderAsync

Cancel all open orders and trigger pending orders.

```csharp
var response = await client.CancelAllOrderAsync(strategy: "CSharp");
```

**Response:**
```json
{
  "status": "success",
  "message": "Canceled 5 orders. Failed to cancel 0 orders.",
  "canceled_orders": ["250408001042620", "250408001042667", "250408001042642"],
  "failed_cancellations": []
}
```

---

## ClosePositionAsync

Close all open positions across various exchanges.

```csharp
var response = await client.ClosePositionAsync(strategy: "CSharp");
```

**Response:**
```json
{"message": "All Open Positions Squared Off", "status": "success"}
```

---

## OrderStatusAsync

Get the current order status.

```csharp
var response = await client.OrderStatusAsync(orderId: "250828000185002");
```

**Response:**
```json
{
  "data": {
    "action": "BUY",
    "average_price": 18.95,
    "exchange": "NSE",
    "order_status": "complete",
    "orderid": "250828000185002",
    "price": 0,
    "pricetype": "MARKET",
    "product": "MIS",
    "quantity": "1",
    "symbol": "YESBANK",
    "timestamp": "28-Aug-2025 09:59:10",
    "trigger_price": 0
  },
  "status": "success"
}
```

---

## OpenPositionAsync

Get the current open position for a symbol.

```csharp
var response = await client.OpenPositionAsync(
    strategy: "CSharp",
    symbol: "YESBANK",
    exchange: "NSE",
    product: "MIS"
);
```

**Response:**
```json
{"quantity": "-10", "status": "success"}
```

---

## QuotesAsync

Get real-time quotes for a symbol.

```csharp
var response = await client.QuotesAsync(symbol: "RELIANCE", exchange: "NSE");
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "open": 1172.0,
    "high": 1196.6,
    "low": 1163.3,
    "ltp": 1187.75,
    "ask": 1188.0,
    "bid": 1187.85,
    "prev_close": 1165.7,
    "volume": 14414545
  }
}
```

---

## MultiQuotesAsync

Get real-time quotes for multiple symbols.

```csharp
var symbols = new List<SymbolExchangePair>
{
    new() { Symbol = "RELIANCE", Exchange = "NSE" },
    new() { Symbol = "TCS", Exchange = "NSE" },
    new() { Symbol = "INFY", Exchange = "NSE" }
};

var response = await client.MultiQuotesAsync(symbols);
```

---

## DepthAsync

Get market depth (order book) for a symbol.

```csharp
var response = await client.DepthAsync(symbol: "SBIN", exchange: "NSE");
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "open": 760.0,
    "high": 774.0,
    "low": 758.15,
    "ltp": 769.6,
    "ltq": 205,
    "prev_close": 746.9,
    "volume": 9362799,
    "oi": 161265750,
    "totalbuyqty": 591351,
    "totalsellqty": 835701,
    "asks": [{"price": 769.6, "quantity": 767}, ...],
    "bids": [{"price": 769.4, "quantity": 886}, ...]
  }
}
```

---

## HistoryAsync

Get historical OHLCV data.

```csharp
var response = await client.HistoryAsync(
    symbol: "SBIN",
    exchange: "NSE",
    interval: "5m",
    startDate: "2025-04-01",
    endDate: "2025-04-08"
);
```

---

## IntervalsAsync

Get supported time intervals.

```csharp
var response = await client.IntervalsAsync();
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "months": [],
    "weeks": [],
    "days": ["D"],
    "hours": ["1h"],
    "minutes": ["10m", "15m", "1m", "30m", "3m", "5m"],
    "seconds": []
  }
}
```

---

## OptionChainAsync

Fetch option chain data with real-time quotes.

```csharp
var response = await client.OptionChainAsync(
    underlying: "NIFTY",
    exchange: "NFO",
    expiryDate: "30DEC25",
    strikeCount: 10  // Optional - number of strikes above/below ATM
);
```

---

## SymbolAsync

Get symbol details.

```csharp
var response = await client.SymbolAsync(symbol: "NIFTY30DEC25FUT", exchange: "NFO");
```

**Response:**
```json
{
  "data": {
    "brexchange": "NSE_FO",
    "brsymbol": "NIFTY FUT 30 DEC 25",
    "exchange": "NFO",
    "expiry": "30-DEC-25",
    "freeze_qty": 1800,
    "instrumenttype": "FUT",
    "lotsize": 75,
    "name": "NIFTY",
    "symbol": "NIFTY30DEC25FUT",
    "token": "NSE_FO|49543"
  },
  "status": "success"
}
```

---

## SearchAsync

Search for symbols across exchanges.

```csharp
var response = await client.SearchAsync(query: "NIFTY 26000 DEC CE", exchange: "NFO");
```

---

## OptionSymbolAsync

Get option symbol details based on underlying and offset.

```csharp
var response = await client.OptionSymbolAsync(
    underlying: "NIFTY",
    exchange: "NFO",
    offset: "ATM",
    optionType: "CE",
    expiryDate: "30DEC25"
);
```

**Response:**
```json
{
  "status": "success",
  "symbol": "NIFTY30DEC2525950CE",
  "exchange": "NFO",
  "lotsize": 75,
  "tick_size": 5,
  "freeze_qty": 1800,
  "underlying_ltp": 25966.4
}
```

---

## SyntheticFutureAsync

Calculate synthetic futures price using ATM options.

```csharp
var response = await client.SyntheticFutureAsync(
    underlying: "NIFTY",
    exchange: "NFO",
    expiryDate: "25NOV25"
);
```

**Response:**
```json
{
  "atm_strike": 25900.0,
  "expiry": "25NOV25",
  "status": "success",
  "synthetic_future_price": 25980.05,
  "underlying": "NIFTY",
  "underlying_ltp": 25910.05
}
```

---

## OptionGreeksAsync

Calculate option Greeks using Black-76 model.

```csharp
var response = await client.OptionGreeksAsync(
    symbol: "NIFTY25NOV2526000CE",
    exchange: "NFO",
    interestRate: 0.00m,
    underlyingSymbol: "NIFTY",
    underlyingExchange: "NSE_INDEX"
);
```

**Response:**
```json
{
  "days_to_expiry": 28.5071,
  "exchange": "NFO",
  "expiry_date": "25-Nov-2025",
  "greeks": {
    "delta": 0.4967,
    "gamma": 0.000352,
    "rho": 9.733994,
    "theta": -7.919,
    "vega": 28.9489
  },
  "implied_volatility": 15.6,
  "option_price": 435,
  "status": "success",
  "symbol": "NIFTY25NOV2526000CE"
}
```

---

## ExpiryAsync

Get expiry dates for a symbol.

```csharp
var response = await client.ExpiryAsync(
    symbol: "NIFTY",
    exchange: "NFO",
    instrumenttype: "options"
);
```

---

## InstrumentsAsync

Download all trading symbols and instruments.

```csharp
// Get instruments for specific exchange
var response = await client.InstrumentsAsync(exchange: "NSE");

// Get all instruments from all exchanges
var allResponse = await client.InstrumentsAsync();
```

---

## TelegramAsync

Send custom alert messages to Telegram.

```csharp
var response = await client.TelegramAsync(
    username: "your_openalgo_username",
    message: "NIFTY crossed 26000!",
    priority: 5  // Optional, 1-10
);
```

**Response:**
```json
{"message": "Notification sent successfully", "status": "success"}
```

---

## FundsAsync

Get funds and margin details.

```csharp
var response = await client.FundsAsync();
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "availablecash": "320.66",
    "collateral": "0.00",
    "m2mrealized": "3.27",
    "m2munrealized": "-7.88",
    "utiliseddebits": "679.34"
  }
}
```

---

## MarginAsync

Calculate margin requirements for positions.

```csharp
var positions = new List<MarginPosition>
{
    new() { Symbol = "NIFTY25NOV2525000CE", Exchange = "NFO", Action = "BUY", Product = "NRML", PriceType = "MARKET", Quantity = 75 },
    new() { Symbol = "NIFTY25NOV2525500CE", Exchange = "NFO", Action = "SELL", Product = "NRML", PriceType = "MARKET", Quantity = 75 }
};

var response = await client.MarginAsync(positions);
```

**Response:**
```json
{
  "status": "success",
  "data": {
    "total_margin_required": 91555.7625,
    "span_margin": 0.0,
    "exposure_margin": 91555.7625
  }
}
```

---

## OrderBookAsync

Get orderbook details.

```csharp
var response = await client.OrderBookAsync();
```

---

## TradeBookAsync

Get tradebook details.

```csharp
var response = await client.TradeBookAsync();
```

---

## PositionBookAsync

Get positionbook details.

```csharp
var response = await client.PositionBookAsync();
```

---

## HoldingsAsync

Get stock holdings.

```csharp
var response = await client.HoldingsAsync();
```

---

## HolidaysAsync

Get trading holidays for a year.

```csharp
var response = await client.HolidaysAsync(year: 2026);
```

**Response:**
```json
{
  "data": [
    {"closed_exchanges": ["NSE", "BSE", "NFO", "BFO", "CDS", "BCD", "MCX"], "date": "2026-01-26", "description": "Republic Day", "holiday_type": "TRADING_HOLIDAY", "open_exchanges": []},
    {"closed_exchanges": ["NSE", "BSE", "NFO", "BFO", "CDS", "BCD"], "date": "2026-03-10", "description": "Holi", "holiday_type": "TRADING_HOLIDAY", "open_exchanges": [{"exchange": "MCX", "start_time": 1741624200000, "end_time": 1741677900000}]}
  ],
  "status": "success",
  "timezone": "Asia/Kolkata",
  "year": 2026
}
```

---

## TimingsAsync

Get exchange timings for a date.

```csharp
var response = await client.TimingsAsync(date: "2025-12-19");
```

---

## AnalyzerStatusAsync

Get analyzer status information.

```csharp
var response = await client.AnalyzerStatusAsync();
```

**Response:**
```json
{"data": {"analyze_mode": true, "mode": "analyze", "total_logs": 2}, "status": "success"}
```

---

## AnalyzerToggleAsync

Toggle analyzer mode.

```csharp
// Switch to analyze mode (simulated responses)
var response = await client.AnalyzerToggleAsync(mode: true);

// Switch to live mode
var response = await client.AnalyzerToggleAsync(mode: false);
```

---

# WebSocket Streaming (Async)

## ConnectAsync

Connect to the WebSocket server.

```csharp
var connected = await client.ConnectAsync();
```

## SubscribeLtpAsync

Subscribe to LTP (Last Traded Price) updates.

```csharp
var instruments = new List<Instrument>
{
    new() { Exchange = "NSE", Symbol = "RELIANCE" },
    new() { Exchange = "NSE", Symbol = "INFY" }
};

await client.SubscribeLtpAsync(instruments, data =>
{
    Console.WriteLine($"LTP Update: {data.Exchange}:{data.Symbol} = {data.Price}");
});
```

## SubscribeQuoteAsync

Subscribe to Quote updates.

```csharp
await client.SubscribeQuoteAsync(instruments, data =>
{
    Console.WriteLine($"Quote: {data.Symbol} O:{data.Open} H:{data.High} L:{data.Low} LTP:{data.Ltp}");
});
```

## SubscribeDepthAsync

Subscribe to Market Depth updates.

```csharp
await client.SubscribeDepthAsync(instruments, data =>
{
    Console.WriteLine($"Depth: {data.Symbol} LTP:{data.Ltp}");
    Console.WriteLine($"  Best Bid: {data.Buy[0].Price} x {data.Buy[0].Quantity}");
    Console.WriteLine($"  Best Ask: {data.Sell[0].Price} x {data.Sell[0].Quantity}");
});
```

## UnsubscribeAsync

```csharp
await client.UnsubscribeLtpAsync(instruments);
await client.UnsubscribeQuoteAsync(instruments);
await client.UnsubscribeDepthAsync(instruments);
```

## DisconnectAsync

```csharp
await client.DisconnectAsync();
```

## Get Stored Data

```csharp
// Get all LTP data
var ltpData = client.GetLtp();

// Filter by exchange
var nseLtp = client.GetLtp(exchange: "NSE");

// Filter by exchange and symbol
var relianceLtp = client.GetLtp(exchange: "NSE", symbol: "RELIANCE");

// Same for quotes and depth
var quotes = client.GetQuotes();
var depth = client.GetDepth();
```

---

# Sync API Reference

All async methods have synchronous equivalents without the `Async` suffix. Sync methods internally call the async versions.

## PlaceOrder

```csharp
var response = client.PlaceOrder(
    symbol: "NHPC",
    action: "BUY",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1
);
```

## PlaceSmartOrder

```csharp
var response = client.PlaceSmartOrder(
    symbol: "TATAMOTORS",
    action: "SELL",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1,
    positionSize: 5
);
```

## OptionsOrder

```csharp
var response = client.OptionsOrder(
    underlying: "NIFTY",
    exchange: "NFO",
    offset: "ATM",
    optionType: "CE",
    action: "BUY",
    quantity: 75,
    expiryDate: "28OCT25",
    priceType: "MARKET",
    product: "NRML"
);
```

## OptionsMultiOrder

```csharp
var legs = new List<OptionLeg>
{
    new() { Offset = "OTM6", OptionType = "CE", Action = "BUY", Quantity = 75 },
    new() { Offset = "OTM6", OptionType = "PE", Action = "BUY", Quantity = 75 }
};

var response = client.OptionsMultiOrder(
    strategy: "Iron Condor",
    underlying: "NIFTY",
    exchange: "NFO",
    legs: legs,
    expiryDate: "25NOV25"
);
```

## BasketOrder

```csharp
var orders = new List<BasketOrderItem>
{
    new() { Symbol = "BHEL", Exchange = "NSE", Action = "BUY", Quantity = 1, PriceType = "MARKET", Product = "MIS" },
    new() { Symbol = "ZOMATO", Exchange = "NSE", Action = "SELL", Quantity = 1, PriceType = "MARKET", Product = "MIS" }
};

var response = client.BasketOrder(orders);
```

## SplitOrder

```csharp
var response = client.SplitOrder(
    symbol: "YESBANK",
    exchange: "NSE",
    action: "SELL",
    quantity: 105,
    splitSize: 20,
    priceType: "MARKET",
    product: "MIS"
);
```

## ModifyOrder

```csharp
var response = client.ModifyOrder(
    orderId: "250408001002736",
    symbol: "YESBANK",
    action: "BUY",
    exchange: "NSE",
    priceType: "LIMIT",
    product: "CNC",
    quantity: 1,
    price: "16.5"
);
```

## CancelOrder

```csharp
var response = client.CancelOrder(orderId: "250408001002736");
```

## CancelAllOrder

```csharp
var response = client.CancelAllOrder(strategy: "CSharp");
```

## ClosePosition

```csharp
var response = client.ClosePosition(strategy: "CSharp");
```

## OrderStatus

```csharp
var response = client.OrderStatus(orderId: "250828000185002");
```

## OpenPosition

```csharp
var response = client.OpenPosition(
    strategy: "CSharp",
    symbol: "YESBANK",
    exchange: "NSE",
    product: "MIS"
);
```

## Quotes

```csharp
var response = client.Quotes(symbol: "RELIANCE", exchange: "NSE");
```

## MultiQuotes

```csharp
var symbols = new List<SymbolExchangePair>
{
    new() { Symbol = "RELIANCE", Exchange = "NSE" },
    new() { Symbol = "TCS", Exchange = "NSE" }
};

var response = client.MultiQuotes(symbols);
```

## Depth

```csharp
var response = client.Depth(symbol: "SBIN", exchange: "NSE");
```

## History

```csharp
var response = client.History(
    symbol: "SBIN",
    exchange: "NSE",
    interval: "5m",
    startDate: "2025-04-01",
    endDate: "2025-04-08"
);
```

## Intervals

```csharp
var response = client.Intervals();
```

## OptionChain

```csharp
var response = client.OptionChain(
    underlying: "NIFTY",
    exchange: "NFO",
    expiryDate: "30DEC25",
    strikeCount: 10
);
```

## Symbol

```csharp
var response = client.Symbol(symbol: "NIFTY30DEC25FUT", exchange: "NFO");
```

## Search

```csharp
var response = client.Search(query: "NIFTY 26000 DEC CE", exchange: "NFO");
```

## OptionSymbol

```csharp
var response = client.OptionSymbol(
    underlying: "NIFTY",
    exchange: "NFO",
    offset: "ATM",
    optionType: "CE",
    expiryDate: "30DEC25"
);
```

## SyntheticFuture

```csharp
var response = client.SyntheticFuture(
    underlying: "NIFTY",
    exchange: "NFO",
    expiryDate: "25NOV25"
);
```

## OptionGreeks

```csharp
var response = client.OptionGreeks(
    symbol: "NIFTY25NOV2526000CE",
    exchange: "NFO"
);
```

## Expiry

```csharp
var response = client.Expiry(
    symbol: "NIFTY",
    exchange: "NFO",
    instrumenttype: "options"
);
```

## Instruments

```csharp
var response = client.Instruments(exchange: "NSE");
```

## Telegram

```csharp
var response = client.Telegram(
    username: "your_openalgo_username",
    message: "Alert message!"
);
```

## Funds

```csharp
var response = client.Funds();
```

## Margin

```csharp
var positions = new List<MarginPosition>
{
    new() { Symbol = "NIFTY25NOV2525000CE", Exchange = "NFO", Action = "BUY", Product = "NRML", PriceType = "MARKET", Quantity = 75 }
};

var response = client.Margin(positions);
```

## OrderBook

```csharp
var response = client.OrderBook();
```

## TradeBook

```csharp
var response = client.TradeBook();
```

## PositionBook

```csharp
var response = client.PositionBook();
```

## Holdings

```csharp
var response = client.Holdings();
```

## Holidays

```csharp
var response = client.Holidays(year: 2026);
```

## Timings

```csharp
var response = client.Timings(date: "2025-12-19");
```

## AnalyzerStatus

```csharp
var response = client.AnalyzerStatus();
```

## AnalyzerToggle

```csharp
var response = client.AnalyzerToggle(mode: true);
```

---

# WebSocket Streaming (Sync)

## Connect

```csharp
var connected = client.Connect();
```

## SubscribeLtp

```csharp
var instruments = new List<Instrument>
{
    new() { Exchange = "NSE", Symbol = "RELIANCE" },
    new() { Exchange = "NSE", Symbol = "INFY" }
};

client.SubscribeLtp(instruments, data =>
{
    Console.WriteLine($"LTP: {data.Symbol} = {data.Price}");
});
```

## SubscribeQuote

```csharp
client.SubscribeQuote(instruments, data =>
{
    Console.WriteLine($"Quote: {data.Symbol} LTP:{data.Ltp}");
});
```

## SubscribeDepth

```csharp
client.SubscribeDepth(instruments, data =>
{
    Console.WriteLine($"Depth: {data.Symbol} LTP:{data.Ltp}");
});
```

## Unsubscribe

```csharp
client.UnsubscribeLtp(instruments);
client.UnsubscribeQuote(instruments);
client.UnsubscribeDepth(instruments);
```

## Disconnect

```csharp
client.Disconnect();
```

---

# Error Handling

```csharp
var response = client.PlaceOrder(
    symbol: "RELIANCE",
    action: "BUY",
    exchange: "NSE"
);

if (response.IsSuccess)
{
    Console.WriteLine($"Order placed: {response.OrderId}");
}
else
{
    Console.WriteLine($"Error: {response.Message}");
    Console.WriteLine($"Error Type: {response.ErrorType}");
}
```

---

# Complete Example

```csharp
using OpenAlgo.NET;
using OpenAlgo.NET.Models.Common;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize client
        var client = new Api(
            apiKey: "your_api_key",
            verbose: 1
        );

        // Check funds
        var funds = await client.FundsAsync();
        Console.WriteLine($"Available funds: {funds.Data?.AvailableCash}");

        // Get quote
        var quote = await client.QuotesAsync("RELIANCE", "NSE");
        Console.WriteLine($"RELIANCE LTP: {quote.Data?.Ltp}");

        // Place order
        var order = await client.PlaceOrderAsync(
            symbol: "RELIANCE",
            action: "BUY",
            exchange: "NSE",
            priceType: "MARKET",
            product: "MIS",
            quantity: 1
        );

        if (order.IsSuccess)
        {
            Console.WriteLine($"Order placed: {order.OrderId}");
        }

        // WebSocket streaming
        if (await client.ConnectAsync())
        {
            var instruments = new List<Instrument>
            {
                new() { Symbol = "RELIANCE", Exchange = "NSE" }
            };

            await client.SubscribeLtpAsync(instruments, data =>
            {
                Console.WriteLine($"LTP: {data.Price}");
            });

            await Task.Delay(30000);
            await client.DisconnectAsync();
        }
    }
}
```

---

# License

MIT License

# Links

- [OpenAlgo Platform](https://openalgo.in/)
- [API Documentation](https://docs.openalgo.in/api-documentation/v1)
- [Order Constants](https://docs.openalgo.in/api-documentation/v1/order-constants)
