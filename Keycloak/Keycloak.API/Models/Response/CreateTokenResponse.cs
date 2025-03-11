using System.Text.Json.Serialization;

namespace Keycloak.API.Models.Response;

public sealed record  CreateTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}