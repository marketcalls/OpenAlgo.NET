using OpenAlgo.NET;

var client = new Api(
    apiKey: "a4565e952022cbde7142a1f8c005e60d6d18ec868b1aba76911e95ec18cc737a",
    host: "http://127.0.0.1:5000"
);

Console.WriteLine("=== Market Holidays 2025 ===\n");

var response = client.Holidays(year: 2025);

Console.WriteLine($"Status: {response.Status}");

if (response.IsSuccess && response.Data != null)
{
    Console.WriteLine($"Total Holidays: {response.Data.Count}\n");
    Console.WriteLine($"{"Date",-12} {"Description",-35} {"Type",-18} {"Closed"}");
    Console.WriteLine(new string('-', 90));

    foreach (var holiday in response.Data)
    {
        var closed = holiday.ClosedExchanges != null ? string.Join(",", holiday.ClosedExchanges) : "";
        if (closed.Length > 25) closed = closed.Substring(0, 22) + "...";
        Console.WriteLine($"{holiday.Date,-12} {holiday.Description,-35} {holiday.HolidayType,-18} {closed}");
    }
}
else
{
    Console.WriteLine($"Message: {response.Message}");
}
