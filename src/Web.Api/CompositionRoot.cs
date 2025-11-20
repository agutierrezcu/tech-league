using System.Reflection;
using System.Threading.Channels;
using Application;
using Application.Abstractions.Decorators;
using Application.Abstractions.DependencyInjection;
using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.DDD;
using FluentValidation;
using Infrastructure.Background;
using Infrastructure.Database;
using Infrastructure.DependencyInjection;
using Infrastructure.EventBus;
using Infrastructure.Time;
using Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using SharedKernel;
using Web.Api.Background;
using Web.Api.Infrastructure;

namespace Web.Api;

internal static class CompositionRoot
{
    private const string DdbbConnectionStringName = "TechLeagueDDBB";

    internal static IServiceCollection AddServices(this IServiceCollection services,
        IWebHostEnvironment environment, Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(assemblies);

        services.AddHttpContextAccessor();

        services.AddHybridCache(options =>
        {
            // Maximum size of cached items
            options.MaximumPayloadBytes = 1024 * 1024 * 10; // 10MB
            options.MaximumKeyLength = 512;

            // Default timeouts
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(1),
                LocalCacheExpiration = TimeSpan.FromHours(1)
            };
        });

        services.AddSingleton<IEventBus, EventBus>();

        services.AddSingleton(typeof(IEventBusPublisher), typeof(DomainChannelPublisher));

        services.AddHostedService<DomainEventChannelProcessorBackgroundService>();

        if (environment.IsDevelopment())
        {
            services.AddHostedService<ClubFinanceStatusProjectionInitializerBackgroundService>();
        }

        services.AddSingleton<IExecutionStrategy, ExecutionInParallerlStrategy>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<ICommandHandlerMediator, CommandHandlerMediator>();

        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true,
            lifetime: ServiceLifetime.Singleton);

        return services;
    }

    internal static IServiceCollection AddApplication(this IServiceCollection services,
        Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
                .AsSelf()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventBroadcaster<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

        RegisterDomainEventHandlerScopedWrappers(services, assemblies);

        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandHandler<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(ValidationDecorator.QueryHandler<,>));

        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandHandler<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        services.Decorate(typeof(IDomainEventHandlerScopedWrapper<>), typeof(LoggingDecorator.DomainEventHandlerScopedWrapper<>));

        return services;
    }

    private static void RegisterDomainEventHandlerScopedWrappers(IServiceCollection services, Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        var handlerInfos = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsGenericType)
            .Select(t => new
            {
                HandlerInterfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                    .Select(i => new
                    {
                        EventType = i.GetGenericArguments()[0],
                        HandlerType = t
                    })
            })
            .SelectMany(x => x.HandlerInterfaces)
            .ToList();

        foreach (var handlerInfo in handlerInfos)
        {
            Type scopedHandlerType = typeof(IDomainEventHandlerScopedWrapper<>)
                .MakeGenericType(handlerInfo.EventType);

            Type scopedHandlerWrapperType = typeof(DomainEventHandlerScopedWrapper<,>)
                .MakeGenericType(handlerInfo.HandlerType, handlerInfo.EventType);

            services.AddSingleton(scopedHandlerType, scopedHandlerWrapperType);
        }
    }

    internal static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(assemblies);

        string connectionString = configuration.GetConnectionString(DdbbConnectionStringName)
                                  ?? throw new InvalidOperationException("Connection string 'TechLeague' not found.");

        services.AddPooledDbContextFactory<TechLeagueDbContext>((sp, options) =>
        {
            ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            TechLeagueDbContext.ConfigOptions(options, connectionString)
                .UseLoggerFactory(loggerFactory)
                .UseAsyncSeeding(EfCoreDataSeeder.GetSeeder())
                .UseSeeding((dbContext, _)
                    => EfCoreDataSeeder.GetSeeder()(dbContext, false, CancellationToken.None)
                        .GetAwaiter().GetResult());
        });

        services.Decorate(typeof(IDbContextFactory<>), typeof(TechLeagueDbContextFactoryDecorator<>));

        services.AddScoped(sp => sp.GetRequiredService<IDbContextFactory<TechLeagueDbContext>>()
            .CreateDbContext());

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IRepository>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<IQueryableDbContext>(), publicOnly: false)
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());

        var channel = Channel.CreateUnbounded<IDomainEvent>(
            new UnboundedChannelOptions
            {
                SingleReader = false,
                SingleWriter = false
            });

        services.AddSingleton(channel.Writer);
        services.AddSingleton(channel.Reader);

        return services;
    }

    internal static IServiceCollection AddChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddHealthChecks()
            .AddSqlServer(
                configuration.GetConnectionString(DdbbConnectionStringName)!,
                name: "SqlServer")
            .AddDbContextCheck<TechLeagueDbContext>(
                nameof(TechLeagueDbContext));

        return services;
    }

    internal static IServiceCollection AddoConfiguration<TSettings>(this IServiceCollection services,
        string configurationSection)
            where TSettings : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(configurationSection);

        services.AddOptions<TSettings>()
            .BindConfiguration(configurationSection)
            .ValidateFluentValidation()
            .ValidateOnStart();

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TSettings>>().Value);

        return services;
    }
}
