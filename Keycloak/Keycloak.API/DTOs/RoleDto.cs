using System.Text.Json.Serialization;

namespace Keycloak.API.DTOs;

public sealed record RoleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}