using System.Reflection;
using SharedKernel;

namespace Web.Api.Infrastructure;

public static class AppDomainExtensions
{
    public static Assembly?[] GetApplicationAssemblies(this AppDomain appDomain)
    {
        ArgumentNullException.ThrowIfNull(appDomain);

        Func<Assembly, bool> predicateDiscoverableAssemblies = 
            a => a?.GetCustomAttribute<TechLeagueDiscoverableAssemblyAttribute>() is not null;
    
        Assembly[] currentAssemblies = [.. appDomain.GetAssemblies()
            .Where(predicateDiscoverableAssemblies)];

        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);

        return [.. currentAssemblies
            .Union(Directory // http://simpleinjector.readthedocs.io/en/latest/assembly-loading-resolution-conflicts.html
                    .EnumerateFiles(path!, "*.dll")
                    .Where(a => !currentAssemblies.Any(aa => aa.FullName == a))
                    .Select(a => Assembly.Load(AssemblyName.GetAssemblyName(a)))
                    .Where(predicateDiscoverableAssemblies))
            .Union([Assembly.GetEntryAssembly()])
            .GroupBy(x => x!.FullName)
            .Select(x => x.First())];
    }
}
