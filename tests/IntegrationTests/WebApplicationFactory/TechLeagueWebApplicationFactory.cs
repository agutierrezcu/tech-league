using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Infrastructure.Background;

namespace IntegrationTests.WebApplicationFactory;

public sealed class TechLeagueWebApplicationFactory : WebApplicationFactory<Web.Api.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Decorate<IBackgroundExecutionStrategy, NoHandlingBackgroundExecutionStrategy>();
        });

    }
}
