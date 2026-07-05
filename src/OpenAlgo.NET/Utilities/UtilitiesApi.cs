using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.Utilities;

/// <summary>
/// Response for telegram operations.
/// </summary>
public class TelegramResponse : BaseResponse
{
}

/// <summary>
/// Utilities API methods for OpenAlgo including Telegram, Market Timings, and Holidays.
/// </summary>
public abstract class UtilitiesApi : Options.OptionsApi
{
    /// <summary>
    /// Initializes a new instance of the UtilitiesApi class.
    /// </summary>
    protected UtilitiesApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
        : base(apiKey, host, version, timeout)
    {
    }

    #region Telegram

    /// <summary>
    /// Send Custom Alert Messages to Telegram Users (async).
    /// </summary>
    /// <param name="username">OpenAlgo login username (NOT Telegram username). Required.</param>
    /// <param name="message">Alert message to send (max 4096 characters). Required.</param>
    /// <param name="priority">Message priority (1-10, higher = more urgent). Defaults to 5.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Telegram response.</returns>
    /// <remarks>
    /// Prerequisites:
    /// 1. Telegram Bot Must Be Running - Start the bot from OpenAlgo Telegram settings
    /// 2. User Must Be Linked - User must have linked their account using /link command in Telegram
    /// 3. Valid API Key - API key must be active and valid
    ///
    /// Priority Levels:
    /// - 1-3: Low Priority (General updates, market news)
    /// - 4-6: Normal Priority (Trade signals, daily summaries)
    /// - 7-8: High Priority (Price alerts, position updates)
    /// - 9-10: Urgent (Stop loss hits, risk alerts)
    /// </remarks>
    public async Task<TelegramResponse> TelegramAsync(
        string username,
        string message,
        int priority = 5,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["username"] = username;
        payload["message"] = message;
        payload["priority"] = priority;

        return await MakeRequestAsync<TelegramResponse>("telegram/notify", payload, cancellationToken);
    }

    /// <summary>
    /// Send Custom Alert Messages to Telegram Users (sync).
    /// </summary>
    public TelegramResponse Telegram(string username, string message, int priority = 5)
    {
        return TelegramAsync(username, message, priority).GetAwaiter().GetResult();
    }

    #endregion

    #region Holidays

    /// <summary>
    /// Get trading holidays for a year (async).
    /// </summary>
    /// <param name="year">Year to get holidays for (2020-2050). Defaults to the current year if not provided.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Holidays response.</returns>
    public async Task<HolidaysResponse> HolidaysAsync(
        int? year = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();

        if (year.HasValue)
        {
            payload["year"] = year.Value;
        }

        return await MakeRequestAsync<HolidaysResponse>("market/holidays", payload, cancellationToken);
    }

    /// <summary>
    /// Get trading holidays for a year (sync).
    /// </summary>
    public HolidaysResponse Holidays(int? year = null)
    {
        return HolidaysAsync(year).GetAwaiter().GetResult();
    }

    #endregion

    #region Timings

    /// <summary>
    /// Get exchange timings for a date (async).
    /// </summary>
    /// <param name="date">Date in YYYY-MM-DD format. Defaults to the current date if not provided.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Timings response.</returns>
    public async Task<TimingsResponse> TimingsAsync(
        string? date = null,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();
        payload["date"] = date ?? DateTime.Now.ToString("yyyy-MM-dd");

        return await MakeRequestAsync<TimingsResponse>("market/timings", payload, cancellationToken);
    }

    /// <summary>
    /// Get exchange timings for a date (sync).
    /// </summary>
    public TimingsResponse Timings(string? date = null)
    {
        return TimingsAsync(date).GetAwaiter().GetResult();
    }

    #endregion
}
