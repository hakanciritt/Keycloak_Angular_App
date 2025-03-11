using System.Text;
using System.Text.Json;
using Keycloak.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Keycloak.API.Endpoints;

public static class AuthEndpointExt
{
    public static RouteGroupBuilder AddAuthEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/Register",
            async ([FromBody] RegisterDto register, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                #region request model

                object obj = new
                {
                    username = register.UserName,
                    firstName = register.FirstName,
                    lastName = register.LastName,
                    email = register.Email,
                    enabled = true,
                    emailVerified = true,
                    credentials = new List<object>
                    {
                        new
                        {
                            type = "password",
                            temporary = false,
                            value = register.Password,
                        }
                    }
                };

                #endregion

                string stringData = JsonSerializer.Serialize(obj);
                var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users";
                var response = await client.PostAsync(endpoint, content, cancellationToken);
                if (response.IsSuccessStatusCode)
                    return Results.Created();

                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                return Results.BadRequest(JsonSerializer.Deserialize<object>(responseContent));
            });

        routeGroupBuilder.MapPost("/Login",
            async ([FromBody] LoginDto loginDto, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keycloakService,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor,
                [FromServices] IHttpClientFactory httpClientFactory) =>
            {
                List<KeyValuePair<string, string>> body = new()
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", optionsMonitor.CurrentValue.ClientId),
                    new KeyValuePair<string, string>("client_secret", optionsMonitor.CurrentValue.ClientSecret),
                    new KeyValuePair<string, string>("username", loginDto.UserName),
                    new KeyValuePair<string, string>("password", loginDto.Password),
                };

                var formBody = new FormUrlEncodedContent(body);

                using var client = httpClientFactory.CreateClient();

                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/realms/{optionsMonitor.CurrentValue.Realm}/protocol/openid-connect/token";
                var response = await client.PostAsync(endpoint, formBody, cancellationToken);
                var content = await response.Content.ReadFromJsonAsync<object>(cancellationToken);
                if (response.IsSuccessStatusCode)
                    return Results.Ok(content);

                return Results.BadRequest(content);
            });

        return routeGroupBuilder;
    }
}