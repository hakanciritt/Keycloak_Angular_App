using System.Text;
using System.Text.Json;
using Keycloak.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Keycloak.API.Endpoints;

public static class RoleEndpointExt
{
    public static RouteGroupBuilder AddRoleEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/Create",
            async (CreateRoleDto roleDto, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var stringContent =
                    new StringContent(JsonSerializer.Serialize(roleDto), Encoding.UTF8, "application/json");

                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/clients/{optionsMonitor.CurrentValue.ClientUUID}/roles";
                var response = await client.PostAsync(endpoint, stringContent, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return Results.Created();
                }

                return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
            });

        routeGroupBuilder.MapDelete("/Delete/{name}",
            async (string name, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/clients/{optionsMonitor.CurrentValue.ClientUUID}/roles/{name}";
                var response = await client.DeleteAsync(endpoint, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return Results.NoContent();
                }

                return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
            });

        routeGroupBuilder.MapGet("/GetAll",
            async (CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/clients/{optionsMonitor.CurrentValue.ClientUUID}/roles";
                var response = await client.GetAsync(endpoint, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<List<RoleDto>>(content);
                    return Results.Ok(json);
                }

                return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
            });

        routeGroupBuilder.MapGet("/GetByName/{name}",
            async (string name, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/clients/{optionsMonitor.CurrentValue.ClientUUID}/roles/{name}";
                var response = await client.GetAsync(endpoint, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonSerializer.Deserialize<RoleDto>(content);
                    return Results.Ok(json);
                }

                return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
            });


        return routeGroupBuilder;
    }
}