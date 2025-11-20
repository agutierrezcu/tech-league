using System.Reflection;
using Application.Clubs.Projections;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database;

public sealed class TechLeagueDbContext : DbContext
{
    private static readonly Assembly _techLeagueDbContextAssembly =
        typeof(TechLeagueDbContext).Assembly;

    public TechLeagueDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Club> Clubs { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<ClubFinanceStatusProjection> FinanceStatusProjection { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(_techLeagueDbContextAssembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new StronglyTypedGuid.SortedKeyGuidValueOnAddConvention());
    }

    public static DbContextOptionsBuilder ConfigOptions(DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        options
            .ReplaceService<IValueConverterSelector, StronglyTypedGuid.ValueConverterSelector>()
            .EnableDetailedErrors()
            .UseSqlServer(
                connectionString,
                builder =>
                {
                    builder
                        .EnableRetryOnFailure(2, TimeSpan.FromSeconds(2), null)
                        .MigrationsAssembly(_techLeagueDbContextAssembly.GetName().Name)
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName);
                })
            .UseExceptionProcessor();

#if DEBUG
        options.EnableSensitiveDataLogging();
#endif

        return options;
    }
}
