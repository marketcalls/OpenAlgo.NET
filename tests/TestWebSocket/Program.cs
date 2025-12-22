using OpenAlgo.NET;
using OpenAlgo.NET.Models.Common;

var client = new Api(
    apiKey: "a4565e952022cbde7142a1f8c005e60d6d18ec868b1aba76911e95ec18cc737a",
    host: "http://127.0.0.1:5000",
    wsUrl: "ws://127.0.0.1:8765"
);

// Define instruments to subscribe
var instruments = new List<Instrument>
{
    new Instrument { Exchange = "MCX", Symbol = "CRUDEOIL16JAN26FUT" }
};

Console.WriteLine("=== WebSocket Test for CRUDEOIL16JAN26FUT (MCX) ===\n");

// Connect to WebSocket
Console.WriteLine("Connecting to WebSocket...");
var connected = client.Connect();
Console.WriteLine($"Connected: {connected}\n");

if (!connected)
{
    Console.WriteLine("Failed to connect. Exiting.");
    return;
}

// Test 1: Subscribe to LTP
Console.WriteLine("=== LTP Subscription ===");
client.SubscribeLtp(instruments);
Console.WriteLine("Subscribed to LTP. Waiting for data...\n");

Thread.Sleep(3000);

// Get cached LTP data
var ltpData = client.GetLtp("MCX", "CRUDEOIL16JAN26FUT");

Console.WriteLine("LTP Data Received:");
if (ltpData.ContainsKey("MCX") && ltpData["MCX"].ContainsKey("CRUDEOIL16JAN26FUT"))
{
    var ltp = ltpData["MCX"]["CRUDEOIL16JAN26FUT"];
    Console.WriteLine($"  Exchange: {ltp.Exchange}");
    Console.WriteLine($"  Symbol: {ltp.Symbol}");
    Console.WriteLine($"  Price: {ltp.Price}");
}
else
{
    Console.WriteLine("  No LTP data received yet");
}
Console.WriteLine();

// Unsubscribe LTP
client.UnsubscribeLtp(instruments);

// Test 2: Subscribe to Quotes
Console.WriteLine("=== Quote Subscription ===");
client.SubscribeQuote(instruments);
Console.WriteLine("Subscribed to Quotes. Waiting for data...\n");

Thread.Sleep(3000);

// Get cached Quote data
var quoteData = client.GetQuotes("MCX", "CRUDEOIL16JAN26FUT");

Console.WriteLine("Quote Data Received:");
if (quoteData.ContainsKey("MCX") && quoteData["MCX"].ContainsKey("CRUDEOIL16JAN26FUT"))
{
    var quote = quoteData["MCX"]["CRUDEOIL16JAN26FUT"];
    Console.WriteLine($"  Exchange: {quote.Exchange}");
    Console.WriteLine($"  Symbol: {quote.Symbol}");
    Console.WriteLine($"  Open: {quote.Open}");
    Console.WriteLine($"  High: {quote.High}");
    Console.WriteLine($"  Low: {quote.Low}");
    Console.WriteLine($"  LTP: {quote.Ltp}");
    Console.WriteLine($"  Volume: {quote.Volume}");
}
else
{
    Console.WriteLine("  No Quote data received yet");
}
Console.WriteLine();

// Unsubscribe Quotes
client.UnsubscribeQuote(instruments);

// Test 3: Subscribe to Depth
Console.WriteLine("=== Depth Subscription ===");
client.SubscribeDepth(instruments);
Console.WriteLine("Subscribed to Depth. Waiting for data...\n");

Thread.Sleep(3000);

// Get cached Depth data
var depthData = client.GetDepth("MCX", "CRUDEOIL16JAN26FUT");

Console.WriteLine("Depth Data Received:");
if (depthData.ContainsKey("MCX") && depthData["MCX"].ContainsKey("CRUDEOIL16JAN26FUT"))
{
    var depth = depthData["MCX"]["CRUDEOIL16JAN26FUT"];
    Console.WriteLine($"  Exchange: {depth.Exchange}");
    Console.WriteLine($"  Symbol: {depth.Symbol}");
    Console.WriteLine($"  LTP: {depth.Ltp}");
    Console.WriteLine("\n  Buy Depth:");
    foreach (var level in depth.Buy.Take(5))
    {
        Console.WriteLine($"    Price: {level.Price} | Qty: {level.Quantity} | Orders: {level.Orders}");
    }
    Console.WriteLine("\n  Sell Depth:");
    foreach (var level in depth.Sell.Take(5))
    {
        Console.WriteLine($"    Price: {level.Price} | Qty: {level.Quantity} | Orders: {level.Orders}");
    }
}
else
{
    Console.WriteLine("  No Depth data received yet");
}
Console.WriteLine();

// Unsubscribe and disconnect
client.UnsubscribeDepth(instruments);
client.Disconnect();
Console.WriteLine("Disconnected from WebSocket.");
