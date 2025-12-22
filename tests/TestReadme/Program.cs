using OpenAlgo.NET;

var client = new Api(
    apiKey: "a4565e952022cbde7142a1f8c005e60d6d18ec868b1aba76911e95ec18cc737a",
    host: "http://127.0.0.1:5000"
);

// Test 1: PlaceOrder CNC
Console.WriteLine("=== PlaceOrder CNC ===\n");
var orderResponse = client.PlaceOrder(
    strategy: "Test",
    symbol: "YESBANK",
    action: "BUY",
    exchange: "NSE",
    priceType: "MARKET",
    product: "CNC",
    quantity: 1
);
Console.WriteLine($"Status: {orderResponse.Status}");
Console.WriteLine($"OrderId: {orderResponse.OrderId}");
Console.WriteLine($"IsSuccess: {orderResponse.IsSuccess}");
Console.WriteLine();

// Test 2: Quotes
Console.WriteLine("=== Quotes ===\n");
var quotesResponse = client.Quotes(symbol: "RELIANCE", exchange: "NSE");
Console.WriteLine($"Status: {quotesResponse.Status}");
if (quotesResponse.IsSuccess && quotesResponse.Data != null)
{
    Console.WriteLine($"Open: {quotesResponse.Data.Open}");
    Console.WriteLine($"High: {quotesResponse.Data.High}");
    Console.WriteLine($"Low: {quotesResponse.Data.Low}");
    Console.WriteLine($"LTP: {quotesResponse.Data.Ltp}");
    Console.WriteLine($"Ask: {quotesResponse.Data.Ask}");
    Console.WriteLine($"Bid: {quotesResponse.Data.Bid}");
    Console.WriteLine($"Prev Close: {quotesResponse.Data.PrevClose}");
    Console.WriteLine($"Volume: {quotesResponse.Data.Volume}");
}
Console.WriteLine();

// Test 3: History
Console.WriteLine("=== History ===\n");
var historyResponse = client.History(
    symbol: "SBIN",
    exchange: "NSE",
    interval: "5m",
    startDate: "2025-12-20",
    endDate: "2025-12-22"
);
Console.WriteLine($"Status: {historyResponse.Status}");
if (historyResponse.IsSuccess && historyResponse.Data != null)
{
    Console.WriteLine($"Total Candles: {historyResponse.Data.Count}\n");
    Console.WriteLine($"{"Timestamp",-22} {"Open",10} {"High",10} {"Low",10} {"Close",10} {"Volume",12}");
    Console.WriteLine(new string('-', 80));
    foreach (var candle in historyResponse.Data.Take(5))
    {
        Console.WriteLine($"{candle.Timestamp,-22} {candle.Open,10:F2} {candle.High,10:F2} {candle.Low,10:F2} {candle.Close,10:F2} {candle.Volume,12}");
    }
    if (historyResponse.Data.Count > 5)
    {
        Console.WriteLine("...");
    }
}
Console.WriteLine();

// Test 4: Holidays
Console.WriteLine("=== Holidays 2025 ===\n");
var holidaysResponse = client.Holidays(year: 2025);
Console.WriteLine($"Status: {holidaysResponse.Status}");
if (holidaysResponse.IsSuccess && holidaysResponse.Data != null)
{
    Console.WriteLine($"Total Holidays: {holidaysResponse.Data.Count}\n");
    Console.WriteLine($"{"Date",-12} {"Description",-35} {"Type",-20}");
    Console.WriteLine(new string('-', 70));
    foreach (var holiday in holidaysResponse.Data.Take(5))
    {
        Console.WriteLine($"{holiday.Date,-12} {holiday.Description,-35} {holiday.HolidayType,-20}");
    }
    if (holidaysResponse.Data.Count > 5)
    {
        Console.WriteLine("...");
    }
}
