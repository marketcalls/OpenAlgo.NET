using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.Telegram;

/// <summary>
/// Response for telegram operations.
/// </summary>
public class TelegramResponse : BaseResponse
{
}

/// <summary>
/// Telegram notification API methods for OpenAlgo.
/// </summary>
public abstract class TelegramApi : Options.OptionsApi
{
    /// <summary>
    /// Initializes a new instance of the TelegramApi class.
    /// </summary>
    protected TelegramApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
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
}
