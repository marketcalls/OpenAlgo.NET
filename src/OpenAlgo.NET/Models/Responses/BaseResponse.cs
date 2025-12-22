using System.Text.Json.Serialization;

namespace OpenAlgo.NET.Models.Responses;

/// <summary>
/// Base response class for all API responses.
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// Response status (success or error).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Error message (only present when status is error).
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Error type (only present when status is error).
    /// </summary>
    [JsonPropertyName("error_type")]
    public string? ErrorType { get; set; }

    /// <summary>
    /// HTTP status code (only present on errors).
    /// </summary>
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    /// <summary>
    /// Check if the response indicates success.
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Status?.ToLower() == "success";
}
