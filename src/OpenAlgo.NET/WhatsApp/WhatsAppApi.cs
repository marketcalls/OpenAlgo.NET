using System.Text.Json.Serialization;
using OpenAlgo.NET.Models.Responses;

namespace OpenAlgo.NET.WhatsApp;

/// <summary>
/// Response for WhatsApp notify operations.
/// </summary>
public class WhatsAppResponse : BaseResponse
{
    /// <summary>
    /// Number of recipients dispatched to the alert pool (fire-and-forget mode, i.e. waitForDelivery = false).
    /// </summary>
    [JsonPropertyName("queued")]
    public int? Queued { get; set; }

    /// <summary>
    /// Per-recipient delivery report (only populated when waitForDelivery = true).
    /// </summary>
    [JsonPropertyName("data")]
    public WhatsAppDeliveryData? Data { get; set; }
}

/// <summary>
/// Per-recipient delivery report for a WhatsApp notify call.
/// </summary>
public class WhatsAppDeliveryData
{
    /// <summary>
    /// Recipient identifiers (JIDs) that WhatsApp confirmed accepted.
    /// </summary>
    [JsonPropertyName("sent")]
    public List<string>? Sent { get; set; }

    /// <summary>
    /// Recipients that failed delivery.
    /// </summary>
    [JsonPropertyName("failed")]
    public List<WhatsAppFailedRecipient>? Failed { get; set; }

    /// <summary>
    /// Number of recipients trimmed by the 5-recipient broadcast cap.
    /// </summary>
    [JsonPropertyName("skipped")]
    public int Skipped { get; set; }
}

/// <summary>
/// A single failed WhatsApp recipient.
/// </summary>
public class WhatsAppFailedRecipient
{
    /// <summary>
    /// Recipient identifier (JID) the message could not be delivered to.
    /// </summary>
    [JsonPropertyName("to")]
    public string? To { get; set; }

    /// <summary>
    /// Failure reason reported by the WhatsApp bridge.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

/// <summary>
/// WhatsApp notification API methods for OpenAlgo.
/// </summary>
/// <remarks>
/// Surface mirror of the Telegram notifier with the richer fields the WhatsApp
/// endpoint supports (multi-recipient broadcast capped at 5, image and document
/// attachments). The OpenAlgo server must already be paired to a WhatsApp
/// account from the <c>/whatsapp</c> admin page; pairing itself is not exposed
/// via the API (a leaked API key must not be able to re-pair the device).
/// </remarks>
public abstract class WhatsAppApi : Utilities.UtilitiesApi
{
    /// <summary>
    /// Initializes a new instance of the WhatsAppApi class.
    /// </summary>
    protected WhatsAppApi(string apiKey, string host = "http://127.0.0.1:5000", string version = "v1", double timeout = 120.0)
        : base(apiKey, host, version, timeout)
    {
    }

    #region WhatsApp

    /// <summary>
    /// Send a WhatsApp message via the OpenAlgo paired device (async).
    /// </summary>
    /// <param name="message">Plain text body. Max 4096 characters. Optional if image or document is set.</param>
    /// <param name="to">Single recipient as an E.164 digit string, e.g. "919876543210". Ignored if <paramref name="toMany"/> is supplied.</param>
    /// <param name="toMany">Up to 5 E.164 digit strings for a small broadcast. Anything beyond 5 is dropped server-side (WhatsApp ToS-safety guardrail).</param>
    /// <param name="username">OpenAlgo username — resolves via the linked-users table on the server.</param>
    /// <param name="image">Server-local path to an image file. Caption falls back to <paramref name="message"/> if no explicit caption.</param>
    /// <param name="document">Server-local path to a document (PDF, CSV, ...).</param>
    /// <param name="caption">Caption for the image / follow-up text for the document.</param>
    /// <param name="filename">Override the document's display name on the recipient's device.</param>
    /// <param name="waitForDelivery">
    /// When true (default), the call blocks until WhatsApp confirms delivery and returns a
    /// per-recipient report. Set to false for true fire-and-forget; the response is a
    /// generic "queued" acknowledgement.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>WhatsApp response.</returns>
    /// <remarks>
    /// Recipient precedence (specify at most one form — defaults to self if none):
    /// <list type="bullet">
    /// <item><description><paramref name="username"/> — OpenAlgo username.</description></item>
    /// <item><description><paramref name="toMany"/> — small broadcast (max 5 recipients).</description></item>
    /// <item><description><paramref name="to"/> — single recipient.</description></item>
    /// <item><description>(none of the above) — defaults to <c>self: true</c>, the paired device's own number.</description></item>
    /// </list>
    ///
    /// Prerequisites:
    /// 1. WhatsApp device must be paired from the OpenAlgo web UI (<c>/whatsapp</c>, scan the QR code).
    /// 2. The bot auto-reconnects on every server boot from the encrypted session blob in <c>openalgo.db</c>.
    /// 3. The OpenAlgo API key must be active.
    ///
    /// Notes:
    /// - Image and document attachments are read from the OpenAlgo server's filesystem, not
    ///   uploaded by the API call. Place files under <c>WHATSAPP_ATTACHMENT_ROOTS</c>
    ///   (defaults to <c>&lt;openalgo&gt;/db/attachments/</c>).
    /// - This call uses the WhatsApp <c>notify</c> endpoint — pairing, start/stop, users,
    ///   config, broadcast, stats, and preferences are admin-only via the session-authed
    ///   <c>/whatsapp</c> web UI.
    /// </remarks>
    public async Task<WhatsAppResponse> WhatsAppAsync(
        string? message = null,
        string? to = null,
        List<string>? toMany = null,
        string? username = null,
        string? image = null,
        string? document = null,
        string? caption = null,
        string? filename = null,
        bool waitForDelivery = true,
        CancellationToken cancellationToken = default)
    {
        var payload = CreatePayload();

        if (!string.IsNullOrEmpty(username))
        {
            payload["username"] = username;
        }
        else if (toMany != null && toMany.Count > 0)
        {
            payload["phones"] = toMany;
        }
        else if (!string.IsNullOrWhiteSpace(to))
        {
            payload["phone"] = to;
        }
        else
        {
            payload["self"] = true;
        }

        if (message != null) payload["message"] = message;
        if (image != null) payload["image_path"] = image;
        if (document != null) payload["document_path"] = document;
        if (caption != null) payload["caption"] = caption;
        if (filename != null) payload["filename"] = filename;
        payload["wait_for_delivery"] = waitForDelivery;

        return await MakeRequestAsync<WhatsAppResponse>("whatsapp/notify", payload, cancellationToken);
    }

    /// <summary>
    /// Send a WhatsApp message via the OpenAlgo paired device (sync).
    /// </summary>
    public WhatsAppResponse WhatsApp(
        string? message = null,
        string? to = null,
        List<string>? toMany = null,
        string? username = null,
        string? image = null,
        string? document = null,
        string? caption = null,
        string? filename = null,
        bool waitForDelivery = true)
    {
        return WhatsAppAsync(message, to, toMany, username, image, document, caption, filename, waitForDelivery)
            .GetAwaiter().GetResult();
    }

    #endregion
}
