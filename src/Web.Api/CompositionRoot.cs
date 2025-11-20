using System.Reflection;
using System.Threading.Channels;
using Application;
using Application.Abstractions.EventBus;
using Application.Abstractions.Decorators;
using Application.Abstractions.DependencyInjection;
using Application.Abstractions.Messaging;
using Domain.DDD;
using Infrastructure.Database;
using Infrastructure.EventBus;
using Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedKernel;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Background;
using Web.Api.Infrastructure.Validation;

namespace Web.Api;

internal static class CompositionRoot
{
    private const string DDBBConnectionStringName = "TechLeagueDDBB";

    internal static IServiceCollection AddServices(this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(environment);

        services.AddSingleton<IEventBus, EventBus>();

        services.AddSingleton(typeof(IEventBusPublisher), typeof(ChannelWriterPublisher));

        services.AddHostedService<EventChannelProcessorBackgroundService>();

        if (environment.IsDevelopment())
        {
            services.AddHostedService<ClubFinanceStatusProjectionInitializerBackgroundService>();
        }

        services.AddSingleton<IBackgroundExecutionStrategy, BackgroundExecutionInParallerlStrategy>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<ICommandHandlerMediator, CommandHandlerMediator>();

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
            .Select(
                t => new
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

        string connectionString = configuration.GetConnectionString(DDBBConnectionStringName)
            ?? throw new InvalidOperationException("Connection string 'TechLeague' not found.");

        services.AddDbContext<TechLeagueDbContext>(
            options =>
                TechLeagueDbContext.ConfigOptions(options, connectionString)
                .UseAsyncSeeding(EFCoreDataSeeder.GetSeeder())
                .UseSeeding((dbContext, _)
                    => EFCoreDataSeeder.GetSeeder()(dbContext, false, CancellationToken.None)
                        .GetAwaiter().GetResult())
        );

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IRepository>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<IQueryableDbContext>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

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
                configuration.GetConnectionString(DDBBConnectionStringName)!,
                name: "SqlServer")
            .AddDbContextCheck<TechLeagueDbContext>(
                name: nameof(TechLeagueDbContext));

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

        services.AddSingleton(
            sp => sp.GetRequiredService<IOptions<TSettings>>().Value);

        return services;
    }
}
