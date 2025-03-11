using Keycloak.API.Models.Response;
using Microsoft.Extensions.Options;

namespace Keycloak.API;

public sealed class KeyCloakService(IHttpClientFactory httpClientFactory,
    IOptionsMonitor<KeyCloakConfiguration> optionsMonitor)
{
    public async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient();
        string endpoint = $"{optionsMonitor.CurrentValue.HostName}/realms/{optionsMonitor.CurrentValue.Realm}/protocol/openid-connect/token";

        var formBody = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", optionsMonitor.CurrentValue.ClientId),
            new KeyValuePair<string, string>("client_secret", optionsMonitor.CurrentValue.ClientSecret),
        };
        var response = await client.PostAsync(endpoint, new FormUrlEncodedContent(formBody), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        CreateTokenResponse? content = await response.Content.ReadFromJsonAsync<CreateTokenResponse>(cancellationToken);
        return content.AccessToken;
    }
}