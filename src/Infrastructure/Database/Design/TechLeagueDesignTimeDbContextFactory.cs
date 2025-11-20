using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Database.Design;

internal sealed class TechLeagueDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TechLeagueDbContext>
{
    private const string DdbbConnectionStringName = "DesignTimeTechLeagueDDBB";

    public TechLeagueDbContext CreateDbContext(string[] args)
    {
        string connectionString = GetConnectionString();

        DbContextOptionsBuilder<TechLeagueDbContext> optionsBuilder = new();

        TechLeagueDbContext.ConfigOptions(optionsBuilder, connectionString);

        return new TechLeagueDbContext(optionsBuilder.Options,
            NullLogger<TechLeagueDbContext>.Instance);
    }

    private static string GetConnectionString()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        return configuration.GetConnectionString(DdbbConnectionStringName);
    }
}
