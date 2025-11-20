using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database.Design;

internal sealed class TechLeagueDbContextFactory : IDesignTimeDbContextFactory<TechLeagueDbContext>
{
    private const string DDBBConnectionStringName = "DesignTimeTechLeagueDDBB";

    public TechLeagueDbContext CreateDbContext(string[] args)
    {
        string connectionString = GetConnectionString();

        DbContextOptionsBuilder<TechLeagueDbContext> optionsBuilder = new();

        TechLeagueDbContext.ConfigOptions(optionsBuilder, connectionString);

        return new TechLeagueDbContext(optionsBuilder.Options);
    }

    private static string GetConnectionString()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetConnectionString(DDBBConnectionStringName);
    }
}
