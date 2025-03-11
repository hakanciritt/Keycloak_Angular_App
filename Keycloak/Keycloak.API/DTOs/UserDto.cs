using System.Text.Json.Serialization;

namespace Keycloak.API.DTOs;

public sealed record UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("username")]
    public string UserName { get; set; }
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    
    [JsonPropertyName("createdTimestamp")]
    public long CreatedTimestamp { get; set; }
    
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}