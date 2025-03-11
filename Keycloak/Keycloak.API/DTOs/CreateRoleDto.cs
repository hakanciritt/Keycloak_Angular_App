using System.Text.Json.Serialization;

namespace Keycloak.API.DTOs;

public sealed record  CreateRoleDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}