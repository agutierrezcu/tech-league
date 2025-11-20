using System.Reflection;
using Application.Abstractions.Messaging;
using Domain.DDD;
using Infrastructure.Database;
using Web.Api;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Aggregate).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(TechLeagueDbContext).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
