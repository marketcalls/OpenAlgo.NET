# OpenAlgo.NET

OpenAlgo.NET SDK for algorithmic trading - C# client for OpenAlgo API

## Installation

To install the OpenAlgo.NET library, use NuGet:

```bash
dotnet add package OpenAlgo.NET
```

Or via Package Manager:

```powershell
Install-Package OpenAlgo.NET
```

## Compatibility

| Framework | Version | Support |
|-----------|---------|---------|
| .NET | 6.0 | LTS (Long Term Support) |
| .NET | 7.0 | STS (Standard Term Support) |
| .NET | 8.0 | LTS (Long Term Support) - Recommended |

The SDK is built using only built-in .NET libraries with no external dependencies.

## Get the OpenAlgo apikey

Make Sure that your OpenAlgo Application is running. Login to OpenAlgo Application with valid credentials and get the OpenAlgo apikey

For detailed function parameters refer to the [API Documentation](https://docs.openalgo.in/api-documentation/v1)

## Getting Started with OpenAlgo.NET

First, import the `Api` class from the OpenAlgo.NET library and initialize it with your API key:

```csharp
using OpenAlgo.NET;

// Replace 'your_api_key_here' with your actual API key
// Specify the host URL with your hosted domain or ngrok domain.
// If running locally in windows then use the default host value.
var client = new Api(apiKey: "your_api_key_here", host: "http://127.0.0.1:5000");
```

## Examples

Please refer to the documentation on [order constants](https://docs.openalgo.in/api-documentation/v1/order-constants), and consult the API reference for details on optional parameters

---

# Sync API Reference

## PlaceOrder example

To place a new market order:

```csharp
var response = client.PlaceOrder(
    strategy: "Python",
    symbol: "NHPC",
    action: "BUY",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1
);
Console.WriteLine(response);
```

Place Market Order Response

```json
{"orderid": "250408000989443", "status": "success"}
```

To place a new limit order:

```csharp
var response = client.PlaceOrder(
    strategy: "Python",
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
Console.WriteLine(response);
```

Place Limit Order Response

```json
{"orderid": "250408001003813", "status": "success"}
```

## PlaceSmartOrder Example

To place a smart order considering the current position size:

```csharp
var response = client.PlaceSmartOrder(
    strategy: "Python",
    symbol: "TATAMOTORS",
    action: "SELL",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1,
    positionSize: 5
);
Console.WriteLine(response);
```

Place Smart Market Order Response

```json
{"orderid": "250408000997543", "status": "success"}
```

## OptionsOrder Example

To place ATM options order

```csharp
var response = client.OptionsOrder(
    strategy: "python",
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "28OCT25",
    offset: "ATM",
    optionType: "CE",
    action: "BUY",
    quantity: 75,
    priceType: "MARKET",
    product: "NRML",
    splitSize: 0
);
Console.WriteLine(response);
```

Place Options Order Response

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

To place ITM options order

```csharp
var response = client.OptionsOrder(
    strategy: "python",
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "28OCT25",
    offset: "ITM4",
    optionType: "PE",
    action: "BUY",
    quantity: 75,
    priceType: "MARKET",
    product: "NRML",
    splitSize: 0
);
Console.WriteLine(response);
```

Place Options Order Response

```json
{
  "exchange": "NFO",
  "offset": "ITM4",
  "option_type": "PE",
  "orderid": "25102800000007",
  "status": "success",
  "symbol": "NIFTY28OCT2526150PE",
  "underlying": "NIFTY28OCT25FUT",
  "underlying_ltp": 25966.05
}
```

To place OTM options order

```csharp
var response = client.OptionsOrder(
    strategy: "python",
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "28OCT25",
    offset: "OTM5",
    optionType: "CE",
    action: "BUY",
    quantity: 75,
    priceType: "MARKET",
    product: "NRML",
    splitSize: 0
);
Console.WriteLine(response);
```

Place Options Order Response

```json
{
  "exchange": "NFO",
  "mode": "analyze",
  "offset": "OTM5",
  "option_type": "CE",
  "orderid": "25102800000008",
  "status": "success",
  "symbol": "NIFTY28OCT2526200CE",
  "underlying": "NIFTY28OCT25FUT",
  "underlying_ltp": 25966.05
}
```

## OptionsMultiOrder Example

To place Iron Condor options order (Same Expiry)

```csharp
var response = client.OptionsMultiOrder(
    strategy: "Iron Condor Test",
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "25NOV25",
    legs: new List<OptionLeg>
    {
        new OptionLeg { Offset = "OTM6", OptionType = "CE", Action = "BUY", Quantity = 75 },
        new OptionLeg { Offset = "OTM6", OptionType = "PE", Action = "BUY", Quantity = 75 },
        new OptionLeg { Offset = "OTM4", OptionType = "CE", Action = "SELL", Quantity = 75 },
        new OptionLeg { Offset = "OTM4", OptionType = "PE", Action = "SELL", Quantity = 75 }
    }
);
Console.WriteLine(response);
```

Place OptionsMultiOrder Response

```json
{
    "status": "success",
    "underlying": "NIFTY",
    "underlying_ltp": 26050.45,
    "results": [
        {
            "action": "BUY",
            "leg": 1,
            "mode": "analyze",
            "offset": "OTM6",
            "option_type": "CE",
            "orderid": "25111996859688",
            "status": "success",
            "symbol": "NIFTY25NOV2526350CE"
        },
        {
            "action": "BUY",
            "leg": 2,
            "mode": "analyze",
            "offset": "OTM6",
            "option_type": "PE",
            "orderid": "25111996042210",
            "status": "success",
            "symbol": "NIFTY25NOV2525750PE"
        },
        {
            "action": "SELL",
            "leg": 3,
            "mode": "analyze",
            "offset": "OTM4",
            "option_type": "CE",
            "orderid": "25111922189638",
            "status": "success",
            "symbol": "NIFTY25NOV2526250CE"
        },
        {
            "action": "SELL",
            "leg": 4,
            "mode": "analyze",
            "offset": "OTM4",
            "option_type": "PE",
            "orderid": "25111919252668",
            "status": "success",
            "symbol": "NIFTY25NOV2525850PE"
        }
    ]
}
```

To place Diagonal Spread options order (Different Expiry)

```csharp
var response = client.OptionsMultiOrder(
    strategy: "Diagonal Spread Test",
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    legs: new List<OptionLeg>
    {
        new OptionLeg { Offset = "ITM2", OptionType = "CE", Action = "BUY", Quantity = 75, ExpiryDate = "30DEC25" },
        new OptionLeg { Offset = "OTM2", OptionType = "CE", Action = "SELL", Quantity = 75, ExpiryDate = "25NOV25" }
    }
);
Console.WriteLine(response);
```

Place OptionsMultiOrder Response

```json
{
    "results": [
        {
            "action": "BUY",
            "leg": 1,
            "mode": "analyze",
            "offset": "ITM2",
            "option_type": "CE",
            "orderid": "25111933337854",
            "status": "success",
            "symbol": "NIFTY30DEC2525950CE"
        },
        {
            "action": "SELL",
            "leg": 2,
            "mode": "analyze",
            "offset": "OTM2",
            "option_type": "CE",
            "orderid": "25111957475473",
            "status": "success",
            "symbol": "NIFTY25NOV2526150CE"
        }
    ],
    "status": "success",
    "underlying": "NIFTY",
    "underlying_ltp": 26052.65
}
```

## BasketOrder example

To place a new basket order:

```csharp
var basketOrders = new List<BasketOrderItem>
{
    new BasketOrderItem
    {
        Symbol = "BHEL",
        Exchange = "NSE",
        Action = "BUY",
        Quantity = 1,
        PriceType = "MARKET",
        Product = "MIS"
    },
    new BasketOrderItem
    {
        Symbol = "ZOMATO",
        Exchange = "NSE",
        Action = "SELL",
        Quantity = 1,
        PriceType = "MARKET",
        Product = "MIS"
    }
};
var response = client.BasketOrder(orders: basketOrders);
Console.WriteLine(response);
```

**Basket Order Response**

```json
{
  "status": "success",
  "results": [
    {
      "symbol": "BHEL",
      "status": "success",
      "orderid": "250408000999544"
    },
    {
      "symbol": "ZOMATO",
      "status": "success",
      "orderid": "250408000997545"
    }
  ]
}
```

## SplitOrder example

To place a new split order:

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
Console.WriteLine(response);
```

**SplitOrder Response**

```json
{
  "status": "success",
  "split_size": 20,
  "total_quantity": 105,
  "results": [
    {
      "order_num": 1,
      "orderid": "250408001021467",
      "quantity": 20,
      "status": "success"
    },
    {
      "order_num": 2,
      "orderid": "250408001021459",
      "quantity": 20,
      "status": "success"
    },
    {
      "order_num": 3,
      "orderid": "250408001021466",
      "quantity": 20,
      "status": "success"
    },
    {
      "order_num": 4,
      "orderid": "250408001021470",
      "quantity": 20,
      "status": "success"
    },
    {
      "order_num": 5,
      "orderid": "250408001021471",
      "quantity": 20,
      "status": "success"
    },
    {
      "order_num": 6,
      "orderid": "250408001021472",
      "quantity": 5,
      "status": "success"
    }
  ]
}
```

## ModifyOrder Example

To modify an existing order:

```csharp
var response = client.ModifyOrder(
    orderId: "250408001002736",
    strategy: "Python",
    symbol: "YESBANK",
    action: "BUY",
    exchange: "NSE",
    priceType: "LIMIT",
    product: "CNC",
    quantity: 1,
    price: "16.5"
);
Console.WriteLine(response);
```

**Modify Order Response**

```json
{"orderid": "250408001002736", "status": "success"}
```

## CancelOrder Example

To cancel an existing order:

```csharp
var response = client.CancelOrder(
    orderId: "250408001002736",
    strategy: "Python"
);
Console.WriteLine(response);
```

**Cancelorder Response**

```json
{"orderid": "250408001002736", "status": "success"}
```

## CancelAllOrder Example

To cancel all open orders and trigger pending orders

```csharp
var response = client.CancelAllOrder(
    strategy: "Python"
);
Console.WriteLine(response);
```

**Cancelallorder Response**

```json
{
  "status": "success",
  "message": "Canceled 5 orders. Failed to cancel 0 orders.",
  "canceled_orders": [
    "250408001042620",
    "250408001042667",
    "250408001042642",
    "250408001043015",
    "250408001043386"
  ],
  "failed_cancellations": []
}
```

## ClosePosition Example

To close all open positions across various exchanges

```csharp
var response = client.ClosePosition(
    strategy: "Python"
);
Console.WriteLine(response);
```

**ClosePosition Response**

```json
{"message": "All Open Positions Squared Off", "status": "success"}
```

## OrderStatus Example

To Get the Current OrderStatus

```csharp
var response = client.OrderStatus(
    orderId: "250828000185002",
    strategy: "Test Strategy"
);
Console.WriteLine(response);
```

**Orderstatus Response**

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

## OpenPosition Example

To Get the Current OpenPosition

```csharp
var response = client.OpenPosition(
    strategy: "Test Strategy",
    symbol: "YESBANK",
    exchange: "NSE",
    product: "MIS"
);
Console.WriteLine(response);
```

OpenPosition Response

```json
{"quantity": "-10", "status": "success"}
```

## Quotes Example

```csharp
var response = client.Quotes(symbol: "RELIANCE", exchange: "NSE");
Console.WriteLine(response);
```

**Quotes response**

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

## MultiQuotes Example

```csharp
var response = client.MultiQuotes(symbols: new List<SymbolExchange>
{
    new SymbolExchange { Symbol = "RELIANCE", Exchange = "NSE" },
    new SymbolExchange { Symbol = "TCS", Exchange = "NSE" },
    new SymbolExchange { Symbol = "INFY", Exchange = "NSE" }
});
Console.WriteLine(response);
```

**MultiQuotes response**

```json
{
  "status": "success",
  "results": [
    {
      "symbol": "RELIANCE",
      "exchange": "NSE",
      "data": {
        "open": 1542.3,
        "high": 1571.6,
        "low": 1540.5,
        "ltp": 1569.9,
        "prev_close": 1539.7,
        "ask": 1569.9,
        "bid": 0,
        "oi": 0,
        "volume": 14054299
      }
    },
    {
      "symbol": "TCS",
      "exchange": "NSE",
      "data": {
        "open": 3118.8,
        "high": 3178,
        "low": 3117,
        "ltp": 3162.9,
        "prev_close": 3119.2,
        "ask": 0,
        "bid": 3162.9,
        "oi": 0,
        "volume": 2508527
      }
    },
    {
      "symbol": "INFY",
      "exchange": "NSE",
      "data": {
        "open": 1532.1,
        "high": 1560.3,
        "low": 1532.1,
        "ltp": 1557.9,
        "prev_close": 1530.6,
        "ask": 0,
        "bid": 1557.9,
        "oi": 0,
        "volume": 7575038
      }
    }
  ]
}
```

## Depth Example

```csharp
var response = client.Depth(symbol: "SBIN", exchange: "NSE");
Console.WriteLine(response);
```

**Depth Response**

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
    "asks": [
      {
        "price": 769.6,
        "quantity": 767
      },
      {
        "price": 769.65,
        "quantity": 115
      },
      {
        "price": 769.7,
        "quantity": 162
      },
      {
        "price": 769.75,
        "quantity": 1121
      },
      {
        "price": 769.8,
        "quantity": 430
      }
    ],
    "bids": [
      {
        "price": 769.4,
        "quantity": 886
      },
      {
        "price": 769.35,
        "quantity": 212
      },
      {
        "price": 769.3,
        "quantity": 351
      },
      {
        "price": 769.25,
        "quantity": 343
      },
      {
        "price": 769.2,
        "quantity": 399
      }
    ]
  }
}
```

## History Example

```csharp
var response = client.History(
    symbol: "SBIN",
    exchange: "NSE",
    interval: "5m",
    startDate: "2025-04-01",
    endDate: "2025-04-08"
);
Console.WriteLine(response);
```

**History Response**

```
                            close    high     low    open  volume
timestamp
2025-04-01 09:15:00+05:30  772.50  774.00  763.20  766.50  318625
2025-04-01 09:20:00+05:30  773.20  774.95  772.10  772.45  197189
2025-04-01 09:25:00+05:30  775.15  775.60  772.60  773.20  227544
2025-04-01 09:30:00+05:30  777.35  777.50  774.85  775.15  134596
2025-04-01 09:35:00+05:30  778.00  778.00  776.25  777.50  145385
...                           ...     ...     ...     ...     ...
2025-04-08 14:00:00+05:30  768.25  770.70  767.85  768.50  142478
2025-04-08 14:05:00+05:30  769.10  769.80  766.60  768.15  128283
2025-04-08 14:10:00+05:30  769.05  769.85  768.40  769.10  119084
2025-04-08 14:15:00+05:30  770.05  770.50  769.05  769.05  158299
2025-04-08 14:20:00+05:30  769.95  770.50  769.40  770.05  125485

[437 rows x 5 columns]
```

## Intervals Example

```csharp
var response = client.Intervals();
Console.WriteLine(response);
```

**Intervals response**

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

## OptionChain Example

Note : To fetch entire option chain for a expiry remove the strikeCount (optional) parameter

```csharp
var chain = client.OptionChain(
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "30DEC25",
    strikeCount: 10
);
Console.WriteLine(chain);
```

**OptionChain Response**

```json
{
    "status": "success",
    "underlying": "NIFTY",
    "underlying_ltp": 26215.55,
    "expiry_date": "30DEC25",
    "atm_strike": 26200.0,
    "chain": [
        {
            "strike": 26100.0,
            "ce": {
                "symbol": "NIFTY30DEC2526100CE",
                "label": "ITM2",
                "ltp": 490,
                "bid": 490,
                "ask": 491,
                "open": 540,
                "high": 571,
                "low": 444.75,
                "prev_close": 496.8,
                "volume": 1195800,
                "oi": 0,
                "lotsize": 75,
                "tick_size": 0.05
            },
            "pe": {
                "symbol": "NIFTY30DEC2526100PE",
                "label": "OTM2",
                "ltp": 193,
                "bid": 191.2,
                "ask": 193,
                "open": 204.1,
                "high": 229.95,
                "low": 175.6,
                "prev_close": 215.95,
                "volume": 1832700,
                "oi": 0,
                "lotsize": 75,
                "tick_size": 0.05
            }
        },
        {
            "strike": 26200.0,
            "ce": {
                "symbol": "NIFTY30DEC2526200CE",
                "label": "ATM",
                "ltp": 427,
                "bid": 425.05,
                "ask": 427,
                "open": 449.95,
                "high": 503.5,
                "low": 384,
                "prev_close": 433.2,
                "volume": 2994000,
                "oi": 0,
                "lotsize": 75,
                "tick_size": 0.05
            },
            "pe": {
                "symbol": "NIFTY30DEC2526200PE",
                "label": "ATM",
                "ltp": 227.4,
                "bid": 227.35,
                "ask": 228.5,
                "open": 251.9,
                "high": 269.15,
                "low": 205.95,
                "prev_close": 251.9,
                "volume": 3745350,
                "oi": 0,
                "lotsize": 75,
                "tick_size": 0.05
            }
        }
    ]
}
```

## Symbol Example

```csharp
var response = client.Symbol(
    symbol: "NIFTY30DEC25FUT",
    exchange: "NFO"
);
Console.WriteLine(response);
```

**Symbols Response**

```json
{
  "data": {
    "brexchange": "NSE_FO",
    "brsymbol": "NIFTY FUT 30 DEC 25",
    "exchange": "NFO",
    "expiry": "30-DEC-25",
    "freeze_qty": 1800,
    "id": 57900,
    "instrumenttype": "FUT",
    "lotsize": 75,
    "name": "NIFTY",
    "strike": 0,
    "symbol": "NIFTY30DEC25FUT",
    "tick_size": 10,
    "token": "NSE_FO|49543"
  },
  "status": "success"
}
```

## Search Example

```csharp
var response = client.Search(query: "NIFTY 26000 DEC CE", exchange: "NFO");
Console.WriteLine(response);
```

**Search Response**

```json
{
  "data": [
    {
      "brexchange": "NSE_FO",
      "brsymbol": "NIFTY 26000 CE 30 DEC 25",
      "exchange": "NFO",
      "expiry": "30-DEC-25",
      "freeze_qty": 1800,
      "instrumenttype": "CE",
      "lotsize": 75,
      "name": "NIFTY",
      "strike": 26000,
      "symbol": "NIFTY30DEC2526000CE",
      "tick_size": 5,
      "token": "NSE_FO|71399"
    },
    {
      "brexchange": "NSE_FO",
      "brsymbol": "NIFTY 26000 CE 29 DEC 26",
      "exchange": "NFO",
      "expiry": "29-DEC-26",
      "freeze_qty": 1800,
      "instrumenttype": "CE",
      "lotsize": 75,
      "name": "NIFTY",
      "strike": 26000,
      "symbol": "NIFTY29DEC2626000CE",
      "tick_size": 5,
      "token": "NSE_FO|71505"
    }
  ],
  "message": "Found 7 matching symbols",
  "status": "success"
}
```

## OptionSymbol Example

ATM Option

```csharp
var response = client.OptionSymbol(
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "30DEC25",
    offset: "ATM",
    optionType: "CE"
);
Console.WriteLine(response);
```

**OptionSymbol Response**

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

ITM Option

```csharp
var response = client.OptionSymbol(
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "30DEC25",
    offset: "ITM3",
    optionType: "PE"
);
Console.WriteLine(response);
```

**OptionSymbol Response**

```json
{
  "status": "success",
  "symbol": "NIFTY30DEC2526100PE",
  "exchange": "NFO",
  "lotsize": 75,
  "tick_size": 5,
  "freeze_qty": 1800,
  "underlying_ltp": 25966.4
}
```

OTM Option

```csharp
var response = client.OptionSymbol(
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "30DEC25",
    offset: "OTM4",
    optionType: "CE"
);
Console.WriteLine(response);
```

**OptionSymbol Response**

```json
{
  "status": "success",
  "symbol": "NIFTY30DEC2526150CE",
  "exchange": "NFO",
  "lotsize": 75,
  "tick_size": 5,
  "freeze_qty": 1800,
  "underlying_ltp": 25966.4
}
```

## SyntheticFuture Example

```csharp
var response = client.SyntheticFuture(
    underlying: "NIFTY",
    exchange: "NSE_INDEX",
    expiryDate: "25NOV25"
);
Console.WriteLine(response);
```

SyntheticFuture **Response**

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

## OptionGreeks Example

```csharp
var response = client.OptionGreeks(
    symbol: "NIFTY25NOV2526000CE",
    exchange: "NFO",
    interestRate: 0.00,
    underlyingSymbol: "NIFTY",
    underlyingExchange: "NSE_INDEX"
);
Console.WriteLine(response);
```

OptionGreeks **Response**

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
  "interest_rate": 0.0,
  "option_price": 435,
  "option_type": "CE",
  "spot_price": 25966.05,
  "status": "success",
  "strike": 26000.0,
  "symbol": "NIFTY25NOV2526000CE",
  "underlying": "NIFTY"
}
```

## Expiry Example

```csharp
var response = client.Expiry(
    symbol: "NIFTY",
    exchange: "NFO",
    instrumentType: "options"
);
Console.WriteLine(response);
```

**Expiry Response**

```json
{
  "data": [
    "10-JUL-25",
    "17-JUL-25",
    "24-JUL-25",
    "31-JUL-25",
    "07-AUG-25",
    "28-AUG-25",
    "25-SEP-25",
    "24-DEC-25",
    "26-MAR-26",
    "25-JUN-26",
    "31-DEC-26",
    "24-JUN-27",
    "30-DEC-27",
    "29-JUN-28",
    "28-DEC-28",
    "28-JUN-29",
    "27-DEC-29",
    "25-JUN-30"
  ],
  "message": "Found 18 expiry dates for NIFTY options in NFO",
  "status": "success"
}
```

## Instruments Example

```csharp
var response = client.Instruments(exchange: "NSE");
Console.WriteLine(response);
```

Instruments **Response**

```
     brexchange           brsymbol exchange expiry instrumenttype  lotsize  \
3041        NSE      NSE:NEOGEN-EQ      NSE   None             EQ        1
3042        NSE     NSE:ALANKIT-EQ      NSE   None             EQ        1
3043        NSE  NSE:EVERESTIND-EQ      NSE   None             EQ        1
3044        NSE   NSE:VIKASLIFE-EQ      NSE   None             EQ        1
3045        NSE    NSE:ONEPOINT-EQ      NSE   None             EQ        1

                          name  strike      symbol  tick_size           token
3041  NEOGEN CHEMICALS LIMITED    -1.0      NEOGEN       0.10  10100000009917
3042           ALANKIT LIMITED    -1.0     ALANKIT       0.01  10100000009921
3043    EVEREST INDUSTRIES LTD    -1.0  EVERESTIND       0.05   1010000000993
3044    VIKAS LIFECARE LIMITED    -1.0   VIKASLIFE       0.01  10100000009931
3045     ONE POINT ONE SOL LTD    -1.0    ONEPOINT       0.01  10100000009939
```

## Telegram Alert Example

```csharp
var response = client.Telegram(
    username: "<openalgo_loginid>",
    message: "NIFTY crossed 26000!"
);
Console.WriteLine(response);
```

**Telegram Alert Response**

```json
{
  "message": "Notification sent successfully",
  "status": "success"
}
```

## Funds Example

```csharp
var response = client.Funds();
Console.WriteLine(response);
```

**Funds Response**

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

## Margin Example

```csharp
var response = client.Margin(positions: new List<MarginPosition>
{
    new MarginPosition
    {
        Symbol = "NIFTY25NOV2525000CE",
        Exchange = "NFO",
        Action = "BUY",
        Product = "NRML",
        PriceType = "MARKET",
        Quantity = 75
    },
    new MarginPosition
    {
        Symbol = "NIFTY25NOV2525500CE",
        Exchange = "NFO",
        Action = "SELL",
        Product = "NRML",
        PriceType = "MARKET",
        Quantity = 75
    }
});
Console.WriteLine(response);
```

**Margin Response**

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

## OrderBook Example

```csharp
var response = client.OrderBook();
Console.WriteLine(response);
```

**OrderBook Response**

```json
{
  "status": "success",
  "data": {
    "orders": [
      {
        "action": "BUY",
        "symbol": "RELIANCE",
        "exchange": "NSE",
        "orderid": "250408000989443",
        "product": "MIS",
        "quantity": "1",
        "price": 1186.0,
        "pricetype": "MARKET",
        "order_status": "complete",
        "trigger_price": 0.0,
        "timestamp": "08-Apr-2025 13:58:03"
      },
      {
        "action": "BUY",
        "symbol": "YESBANK",
        "exchange": "NSE",
        "orderid": "250408001002736",
        "product": "MIS",
        "quantity": "1",
        "price": 16.5,
        "pricetype": "LIMIT",
        "order_status": "cancelled",
        "trigger_price": 0.0,
        "timestamp": "08-Apr-2025 14:13:45"
      }
    ],
    "statistics": {
      "total_buy_orders": 2.0,
      "total_sell_orders": 0.0,
      "total_completed_orders": 1.0,
      "total_open_orders": 0.0,
      "total_rejected_orders": 0.0
    }
  }
}
```

## TradeBook Example

```csharp
var response = client.TradeBook();
Console.WriteLine(response);
```

**TradeBook Response**

```json
{
  "status": "success",
  "data": [
    {
      "action": "BUY",
      "symbol": "RELIANCE",
      "exchange": "NSE",
      "orderid": "250408000989443",
      "product": "MIS",
      "quantity": 0.0,
      "average_price": 1180.1,
      "timestamp": "13:58:03",
      "trade_value": 1180.1
    },
    {
      "action": "SELL",
      "symbol": "NHPC",
      "exchange": "NSE",
      "orderid": "250408001086129",
      "product": "MIS",
      "quantity": 0.0,
      "average_price": 83.74,
      "timestamp": "14:28:49",
      "trade_value": 83.74
    }
  ]
}
```

## PositionBook Example

```csharp
var response = client.PositionBook();
Console.WriteLine(response);
```

**PositionBook Response**

```json
{
  "status": "success",
  "data": [
    {
      "symbol": "NHPC",
      "exchange": "NSE",
      "product": "MIS",
      "quantity": "-1",
      "average_price": "83.74",
      "ltp": "83.72",
      "pnl": "0.02"
    },
    {
      "symbol": "RELIANCE",
      "exchange": "NSE",
      "product": "MIS",
      "quantity": "0",
      "average_price": "0.0",
      "ltp": "1189.9",
      "pnl": "5.90"
    },
    {
      "symbol": "YESBANK",
      "exchange": "NSE",
      "product": "MIS",
      "quantity": "-104",
      "average_price": "17.2",
      "ltp": "17.31",
      "pnl": "-10.44"
    }
  ]
}
```

## Holdings Example

```csharp
var response = client.Holdings();
Console.WriteLine(response);
```

**Holdings Response**

```json
{
  "status": "success",
  "data": {
    "holdings": [
      {
        "symbol": "RELIANCE",
        "exchange": "NSE",
        "product": "CNC",
        "quantity": 1,
        "pnl": -149.0,
        "pnlpercent": -11.1
      },
      {
        "symbol": "TATASTEEL",
        "exchange": "NSE",
        "product": "CNC",
        "quantity": 1,
        "pnl": -15.0,
        "pnlpercent": -10.41
      },
      {
        "symbol": "CANBK",
        "exchange": "NSE",
        "product": "CNC",
        "quantity": 5,
        "pnl": -69.0,
        "pnlpercent": -13.43
      }
    ],
    "statistics": {
      "totalholdingvalue": 1768.0,
      "totalinvvalue": 2001.0,
      "totalprofitandloss": -233.15,
      "totalpnlpercentage": -11.65
    }
  }
}
```

## Holidays Example

```csharp
var response = client.Holidays(year: 2026);
Console.WriteLine(response);
```

**Holidays Response**

```json
{
  "data": [
    {
      "closed_exchanges": ["NSE", "BSE", "NFO", "BFO", "CDS", "BCD", "MCX"],
      "date": "2026-01-26",
      "description": "Republic Day",
      "holiday_type": "TRADING_HOLIDAY",
      "open_exchanges": []
    },
    {
      "closed_exchanges": [],
      "date": "2026-02-19",
      "description": "Chhatrapati Shivaji Maharaj Jayanti",
      "holiday_type": "SETTLEMENT_HOLIDAY",
      "open_exchanges": []
    },
    {
      "closed_exchanges": ["NSE", "BSE", "NFO", "BFO", "CDS", "BCD"],
      "date": "2026-03-10",
      "description": "Holi",
      "holiday_type": "TRADING_HOLIDAY",
      "open_exchanges": [
        {
          "end_time": 1741677900000,
          "exchange": "MCX",
          "start_time": 1741624200000
        }
      ]
    },
    {
      "closed_exchanges": ["NSE", "BSE", "NFO", "BFO", "CDS", "BCD"],
      "date": "2026-03-20",
      "description": "Id-Ul-Fitr (Ramadan)",
      "holiday_type": "TRADING_HOLIDAY",
      "open_exchanges": [
        {
          "end_time": 1742541900000,
          "exchange": "MCX",
          "start_time": 1742488200000
        }
      ]
    },
    {
      "closed_exchanges": ["NSE", "BSE", "NFO", "BFO", "CDS", "BCD"],
      "date": "2026-03-25",
      "description": "Holi (Dhuleti)",
      "holiday_type": "TRADING_HOLIDAY",
      "open_exchanges": [
        {
          "end_time": 1742973900000,
          "exchange": "MCX",
          "start_time": 1742920200000
        }
      ]
    }
  ],
  "status": "success"
}
```

## Timings Example

```csharp
var response = client.Timings(date: "2025-12-19");
Console.WriteLine(response);
```

**Timings Response**

```json
{
  "data": [
    {
      "end_time": 1766138400000,
      "exchange": "NSE",
      "start_time": 1766115900000
    },
    {
      "end_time": 1766138400000,
      "exchange": "BSE",
      "start_time": 1766115900000
    },
    {
      "end_time": 1766138400000,
      "exchange": "NFO",
      "start_time": 1766115900000
    },
    {
      "end_time": 1766138400000,
      "exchange": "BFO",
      "start_time": 1766115900000
    },
    {
      "end_time": 1766168700000,
      "exchange": "MCX",
      "start_time": 1766115000000
    },
    {
      "end_time": 1766143800000,
      "exchange": "BCD",
      "start_time": 1766115000000
    },
    {
      "end_time": 1766143800000,
      "exchange": "CDS",
      "start_time": 1766115000000
    }
  ],
  "status": "success"
}
```

## Analyzer Status Example

```csharp
var response = client.AnalyzerStatus();
Console.WriteLine(response);
```

**Analyzer Status Response**

```json
{
  "data": {
    "analyze_mode": true,
    "mode": "analyze",
    "total_logs": 2
  },
  "status": "success"
}
```

## Analyzer Toggle Example

```csharp
// Switch to analyze mode (simulated responses)
var response = client.AnalyzerToggle(mode: true);
Console.WriteLine(response);
```

**Analyzer Toggle Response**

```json
{
  "data": {
    "analyze_mode": true,
    "message": "Analyzer mode switched to analyze",
    "mode": "analyze",
    "total_logs": 2
  },
  "status": "success"
}
```

---

# Async API Reference

All methods have async versions with the `Async` suffix. For example:

```csharp
// Async PlaceOrder
var response = await client.PlaceOrderAsync(
    strategy: "Python",
    symbol: "NHPC",
    action: "BUY",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1
);

// Async Quotes
var quotes = await client.QuotesAsync(symbol: "RELIANCE", exchange: "NSE");

// Async OrderBook
var orderBook = await client.OrderBookAsync();

// Async with CancellationToken
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var funds = await client.FundsAsync(cts.Token);
```

### Available Async Methods

| Sync Method | Async Method |
|------------|--------------|
| `PlaceOrder()` | `PlaceOrderAsync()` |
| `PlaceSmartOrder()` | `PlaceSmartOrderAsync()` |
| `OptionsOrder()` | `OptionsOrderAsync()` |
| `OptionsMultiOrder()` | `OptionsMultiOrderAsync()` |
| `BasketOrder()` | `BasketOrderAsync()` |
| `SplitOrder()` | `SplitOrderAsync()` |
| `ModifyOrder()` | `ModifyOrderAsync()` |
| `CancelOrder()` | `CancelOrderAsync()` |
| `CancelAllOrder()` | `CancelAllOrderAsync()` |
| `ClosePosition()` | `ClosePositionAsync()` |
| `OrderStatus()` | `OrderStatusAsync()` |
| `OpenPosition()` | `OpenPositionAsync()` |
| `Quotes()` | `QuotesAsync()` |
| `MultiQuotes()` | `MultiQuotesAsync()` |
| `Depth()` | `DepthAsync()` |
| `History()` | `HistoryAsync()` |
| `Intervals()` | `IntervalsAsync()` |
| `OptionChain()` | `OptionChainAsync()` |
| `Symbol()` | `SymbolAsync()` |
| `Search()` | `SearchAsync()` |
| `OptionSymbol()` | `OptionSymbolAsync()` |
| `SyntheticFuture()` | `SyntheticFutureAsync()` |
| `OptionGreeks()` | `OptionGreeksAsync()` |
| `Expiry()` | `ExpiryAsync()` |
| `Instruments()` | `InstrumentsAsync()` |
| `Telegram()` | `TelegramAsync()` |
| `Funds()` | `FundsAsync()` |
| `Margin()` | `MarginAsync()` |
| `OrderBook()` | `OrderBookAsync()` |
| `TradeBook()` | `TradeBookAsync()` |
| `PositionBook()` | `PositionBookAsync()` |
| `Holdings()` | `HoldingsAsync()` |
| `Holidays()` | `HolidaysAsync()` |
| `Timings()` | `TimingsAsync()` |
| `AnalyzerStatus()` | `AnalyzerStatusAsync()` |
| `AnalyzerToggle()` | `AnalyzerToggleAsync()` |

---

# WebSocket Streaming

## LTP Data (Streaming Websocket)

```csharp
using OpenAlgo.NET;

// Initialize OpenAlgo client
var client = new Api(
    apiKey: "your_api_key",                  // Replace with your actual OpenAlgo API key
    host: "http://127.0.0.1:5000",           // REST API host
    wsUrl: "ws://127.0.0.1:8765"             // WebSocket host
);

// Define instruments to subscribe for LTP
var instruments = new List<Instrument>
{
    new Instrument { Exchange = "NSE", Symbol = "RELIANCE" },
    new Instrument { Exchange = "NSE", Symbol = "INFY" }
};

// Callback function for LTP updates
void OnLtp(LtpData data)
{
    Console.WriteLine("LTP Update Received:");
    Console.WriteLine(data);
}

// Connect and subscribe
client.Connect();
client.SubscribeLtp(instruments, OnLtp);

// Run for a few seconds to receive data
try
{
    Thread.Sleep(10000);
}
finally
{
    client.UnsubscribeLtp(instruments);
    client.Disconnect();
}
```

## Quotes (Streaming Websocket)

```csharp
using OpenAlgo.NET;

// Initialize OpenAlgo client
var client = new Api(
    apiKey: "your_api_key",                  // Replace with your actual OpenAlgo API key
    host: "http://127.0.0.1:5000",           // REST API host
    wsUrl: "ws://127.0.0.1:8765"             // WebSocket host
);

// Instruments list
var instruments = new List<Instrument>
{
    new Instrument { Exchange = "NSE", Symbol = "RELIANCE" },
    new Instrument { Exchange = "NSE", Symbol = "INFY" }
};

// Callback for Quote updates
void OnQuote(QuoteData data)
{
    Console.WriteLine("Quote Update Received:");
    Console.WriteLine(data);
}

// Connect and subscribe to quote stream
client.Connect();
client.SubscribeQuote(instruments, OnQuote);

// Keep the script running to receive data
try
{
    Thread.Sleep(10000);
}
finally
{
    client.UnsubscribeQuote(instruments);
    client.Disconnect();
}
```

## Depth (Streaming Websocket)

```csharp
using OpenAlgo.NET;

// Initialize OpenAlgo client
var client = new Api(
    apiKey: "your_api_key",                  // Replace with your actual OpenAlgo API key
    host: "http://127.0.0.1:5000",           // REST API host
    wsUrl: "ws://127.0.0.1:8765"             // WebSocket host
);

// Instruments list for depth
var instruments = new List<Instrument>
{
    new Instrument { Exchange = "NSE", Symbol = "RELIANCE" },
    new Instrument { Exchange = "NSE", Symbol = "INFY" }
};

// Callback for market depth updates
void OnDepth(DepthData data)
{
    Console.WriteLine("Market Depth Update Received:");
    Console.WriteLine(data);
}

// Connect and subscribe to depth stream
client.Connect();
client.SubscribeDepth(instruments, OnDepth);

// Run for a few seconds to collect data
try
{
    Thread.Sleep(10000);
}
finally
{
    client.UnsubscribeDepth(instruments);
    client.Disconnect();
}
```

## WebSocket Methods Summary

| Method | Description |
|--------|-------------|
| `Connect()` / `ConnectAsync()` | Connect to WebSocket server |
| `Disconnect()` / `DisconnectAsync()` | Disconnect from WebSocket server |
| `SubscribeLtp(instruments, callback)` | Subscribe to LTP updates |
| `UnsubscribeLtp(instruments)` | Unsubscribe from LTP updates |
| `SubscribeQuote(instruments, callback)` | Subscribe to Quote updates |
| `UnsubscribeQuote(instruments)` | Unsubscribe from Quote updates |
| `SubscribeDepth(instruments, callback)` | Subscribe to Market Depth updates |
| `UnsubscribeDepth(instruments)` | Unsubscribe from Market Depth updates |
| `GetLtp(exchange, symbol)` | Get cached LTP data |
| `GetQuotes(exchange, symbol)` | Get cached Quote data |
| `GetDepth(exchange, symbol)` | Get cached Depth data |

---

## Error Handling

```csharp
var response = client.PlaceOrder(
    strategy: "Python",
    symbol: "INVALID",
    action: "BUY",
    exchange: "NSE",
    priceType: "MARKET",
    product: "MIS",
    quantity: 1
);

if (!response.IsSuccess)
{
    Console.WriteLine($"Error: {response.Message}");
    Console.WriteLine($"Error Type: {response.ErrorType}");
}
```

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [OpenAlgo Platform](https://openalgo.in/)
- [API Documentation](https://docs.openalgo.in/api-documentation/v1)
- [Order Constants](https://docs.openalgo.in/api-documentation/v1/order-constants)
