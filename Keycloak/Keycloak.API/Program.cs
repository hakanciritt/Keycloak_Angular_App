using Keycloak.API;
using Keycloak.API.Endpoints;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Users",
        builder => builder.RequireResourceRoles("UserCreate", "UserGetAll", "UserUpdate", "UserDelete"));
    options.AddPolicy("Roles",
        builder => builder.RequireResourceRoles("RoleCreate", "RoleGetAll", "RoleUpdate", "RoleDelete"));
}).AddKeycloakAuthorization(builder.Configuration);

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<KeyCloakConfiguration>(builder.Configuration.GetSection("KeyCloakConfiguration"));
builder.Services.AddCors();
builder.Services.AddHttpClient();
builder.Services.AddScoped<KeyCloakService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api/Users").AddUserEndpoints().RequireAuthorization("Users").WithTags("User");
app.MapGroup("api/Roles").AddRoleEndpoints().RequireAuthorization("Roles").WithTags("Role");
app.MapGroup("api/Auth").AddAuthEndpoints().WithTags("Auth");
app.MapGroup("api/UserRoles").AddUserRolesEndpoints().RequireAuthorization().WithTags("UserRole");

app.UseCors(d =>
{
    string? origins = builder.Configuration["AllowedOrigins"];
    if (string.IsNullOrEmpty(origins))
    {
        d.AllowAnyHeader().AllowAnyMethod().AllowAnyMethod();
    }
    else
    {
        var splitOrigins = origins.Split(",")
            .Where(c => !string.IsNullOrEmpty(c))
            .Select(d => d.TrimEnd('/'))
            .ToArray();

        d.WithOrigins(splitOrigins)
            .AllowAnyHeader().AllowAnyMethod().AllowAnyMethod();
    }
});
app.Run();