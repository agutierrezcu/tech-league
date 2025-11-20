using System.Diagnostics;
using System.Reflection;
using Application.Clubs.Projections;
using EntityFramework.Exceptions.SqlServer;
using Infrastructure.Database.StronglyTyped;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

[DebuggerDisplay("TechLeagueDbContext {ContextId}")]
public sealed class TechLeagueDbContext
    (DbContextOptions options, ILogger<TechLeagueDbContext> logger)
        : DbContext(options)
{
    private static readonly Assembly TechLeagueDbContextAssembly =
        typeof(TechLeagueDbContext).Assembly;

    public DbSet<Club> Clubs { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<ClubFinanceStatusProjection> FinanceStatusProjection { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(TechLeagueDbContextAssembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new SortedGuidValueOnAddConvention());
    }

    public static DbContextOptionsBuilder ConfigOptions(DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        options
            .ReplaceService<IValueConverterSelector, StronglyTyped.ValueConverterSelector>()
            .EnableDetailedErrors()
            .UseSqlServer(
                connectionString,
                builder =>
                {
                    builder
                        .EnableRetryOnFailure(2, TimeSpan.FromSeconds(2), null)
                        .MigrationsAssembly(TechLeagueDbContextAssembly.GetName().Name)
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName);
                })
            .UseExceptionProcessor();

#if DEBUG
        options.EnableSensitiveDataLogging();
#endif

        return options;
    }

    public override void Dispose()
    {
        logger.LogInformation("""
            "_____________________________________________________________________________________________________________
            {DbContext} {DbContextId} with hash {HashCode} disposed sync
            _____________________________________________________________________________________________________________
            """,
            nameof(TechLeagueDbContext), ContextId, GetHashCode());

        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        logger.LogInformation("""
            "_____________________________________________________________________________________________________________
            {DbContext} {DbContextId} with hash {HashCode} disposed async
            _____________________________________________________________________________________________________________
            """,
            nameof(TechLeagueDbContext), ContextId, GetHashCode());

        return base.DisposeAsync();
    }
}
