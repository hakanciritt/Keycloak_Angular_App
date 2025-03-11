using System.Text;
using System.Text.Json;
using Keycloak.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Keycloak.API.Endpoints;

public static class UserRolesEndpointExt
{
    public static RouteGroupBuilder AddUserRolesEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/Create/{userId}",
            async ([FromRoute] Guid userId, [FromBody] List<RoleDto> roles, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                string stringData = JsonSerializer.Serialize(roles);
                var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users/{userId}/role-mappings/clients/" +
                    $"{optionsMonitor.CurrentValue.ClientUUID}";

                var response = await client.PostAsync(endpoint, content, cancellationToken);
                if (response.IsSuccessStatusCode)
                    return Results.Created();

                return Results.BadRequest(await response.Content.ReadAsStringAsync(cancellationToken));
            });
        
        routeGroupBuilder.MapGet("/GetAllUserRoles/{userId}",
            async ([FromRoute] Guid userId, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
  
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users/{userId}/role-mappings/clients/" +
                    $"{optionsMonitor.CurrentValue.ClientUUID}";

                var response = await client.GetAsync(endpoint, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<object>(content);
                    return Results.Ok(json);
                }

                return Results.BadRequest();
            });

        routeGroupBuilder.MapDelete("/Delete/{userId}",
            async ([FromRoute] Guid userId, [FromBody] List<RoleDto> roles, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users/{userId}/role-mappings/clients/" +
                    $"{optionsMonitor.CurrentValue.ClientUUID}";

                var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
                string stringData = JsonSerializer.Serialize(roles);
                var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                    return Results.Created();

                return Results.BadRequest(await response.Content.ReadAsStringAsync(cancellationToken));
            });

        return routeGroupBuilder;
    }
}