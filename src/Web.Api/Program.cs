using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Clubs.Players.SignUp.SigningWindow;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Web.Api;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Assembly[] assemblies = AppDomain.CurrentDomain.GetApplicationAssemblies()!;

ConfigurationManager configuration = builder.Configuration;

IServiceCollection services = builder.Services;

services.AddHttpLogging(l => l.LoggingFields = HttpLoggingFields.All);

services.AddHsts(
    opts =>
    {
        opts.Preload = true;
        opts.IncludeSubDomains = true;

        // 31536000 seconds = One year (recomended value)
        opts.MaxAge = TimeSpan.FromSeconds(31536000);
    });

services.AddFastEndpoints();
services.SwaggerDocument();

services
    .AddoConfiguration<SigningWindowSetting>(
        SigningWindowSetting.DefaultConfigurationSection)
    .AddServices(builder.Environment)
    .AddApplication(assemblies)
    .AddInfrastructure(configuration, assemblies)
    .AddChecks(configuration);

services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

services.AddProblemDetails();

services.AddExceptionHandler<GlobalExceptionHandler>();

WebApplication app = builder.Build();

app.UseHttpLogging();

app.UseDefaultExceptionHandler();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerGen();
    app.ApplyMigrations();
}
else
{
    app.UseHsts();
}

app.MapHealthChecks("api/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSecurityHeaders(
    opts => opts
        .AddDefaultSecurityHeaders()
        .AddXssProtectionDisabled()
        .RemoveServerHeader()
);

app.UseFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
    c.Endpoints.RoutePrefix = "api";
    c.Endpoints.Configurator = ep => ep.AllowAnonymous();

    JsonSerializerOptions options = c.Serializer.Options;
    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.Converters.Add(new JsonStringEnumConverter());
});

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Web.Api
{
    public partial class Program;
}
