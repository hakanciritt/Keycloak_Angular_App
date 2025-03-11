using System.Text;
using System.Text.Json;
using Keycloak.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Keycloak.API.Endpoints;

public static class UserEndpointExt
{
    public static RouteGroupBuilder AddUserEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/Create", async (CancellationToken cancellationToken,
            [FromServices] KeyCloakService keycloakService) =>
        {
            var result = await keycloakService.GetAccessToken(cancellationToken);
            return Results.Ok(result);
        }).AllowAnonymous();

        routeGroupBuilder.MapDelete("/Delete/{id}", async (Guid id ,CancellationToken cancellationToken,
            [FromServices] KeyCloakService keyCloakService,
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
        {
            using var client = httpClientFactory.CreateClient();
            var token = await keyCloakService.GetAccessToken(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string endpoint =
                $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users/{id}";
            var response = await client.DeleteAsync(endpoint, cancellationToken);
            
            if (response.IsSuccessStatusCode)
                return Results.NoContent();

            return Results.BadRequest(await response.Content.ReadAsStringAsync(cancellationToken));
        });

        routeGroupBuilder.MapPut("/Update",
            async ([FromBody] UpdateUserDto updateDto, CancellationToken cancellationToken,
                [FromServices] KeyCloakService keyCloakService,
                [FromServices] IHttpClientFactory httpClientFactory,
                [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
            {
                string stringData = JsonSerializer.Serialize(updateDto);
                var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                using var client = httpClientFactory.CreateClient();
                var token = await keyCloakService.GetAccessToken(cancellationToken);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string endpoint =
                    $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users";
                var response = await client.PutAsync(endpoint, content, cancellationToken);
                if (response.IsSuccessStatusCode)
                    return Results.NoContent();

                return Results.BadRequest(await response.Content.ReadAsStringAsync(cancellationToken));
            }).AllowAnonymous();
        
        routeGroupBuilder.MapGet("/GetAll", async (CancellationToken cancellationToken,
            [FromServices] KeyCloakService keyCloakService,
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
        {
            using var client = httpClientFactory.CreateClient();
            var token = await keyCloakService.GetAccessToken(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string endpoint =
                $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users";
            var response = await client.GetAsync(endpoint, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var json = JsonSerializer.Deserialize<List<UserDto>>(content);
                return Results.Ok(json);
            }

            return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
        });

        routeGroupBuilder.MapGet("/GetByEmail/{email}", async (string email, CancellationToken cancellationToken,
            [FromServices] KeyCloakService keyCloakService,
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
        {
            using var client = httpClientFactory.CreateClient();
            var token = await keyCloakService.GetAccessToken(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string endpoint =
                $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users?email={email}";
            var response = await client.GetAsync(endpoint, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var json = JsonSerializer.Deserialize<List<UserDto>>(content);
                return Results.Ok(json);
            }

            return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
        });

        routeGroupBuilder.MapGet("/GetByUserName/{username}", async (string username,
            CancellationToken cancellationToken,
            [FromServices] KeyCloakService keyCloakService,
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
        {
            using var client = httpClientFactory.CreateClient();
            var token = await keyCloakService.GetAccessToken(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string endpoint =
                $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users?username={username}";
            var response = await client.GetAsync(endpoint, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var json = JsonSerializer.Deserialize<List<UserDto>>(content);
                return Results.Ok(json);
            }

            return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
        });

        routeGroupBuilder.MapGet("/GetById/{id}", async (Guid id, CancellationToken cancellationToken,
            [FromServices] KeyCloakService keyCloakService,
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromServices] IOptionsMonitor<KeyCloakConfiguration> optionsMonitor) =>
        {
            using var client = httpClientFactory.CreateClient();
            var token = await keyCloakService.GetAccessToken(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string endpoint =
                $"{optionsMonitor.CurrentValue.HostName}/admin/realms/{optionsMonitor.CurrentValue.Realm}/users/{id}";
            var response = await client.GetAsync(endpoint, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var json = JsonSerializer.Deserialize<UserDto>(content);
                return Results.Ok(json);
            }

            return Results.BadRequest(JsonSerializer.Deserialize<object>(content));
        });
        return routeGroupBuilder;
    }
}