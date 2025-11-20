using System.Reflection;
using SharedKernel;

namespace Infrastructure.Application;

public static class AppDomainExtensions
{
    public static Assembly?[] GetApplicationAssemblies(this AppDomain appDomain)
    {
        ArgumentNullException.ThrowIfNull(appDomain);

        Assembly[] currentAssemblies =
        [
            .. appDomain.GetAssemblies()
                .Where(PredicateDiscoverableAssemblies)
        ];

        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);

        return
        [
            .. currentAssemblies
                .Union(
                    Directory // http://simpleinjector.readthedocs.io/en/latest/assembly-loading-resolution-conflicts.html
                        .EnumerateFiles(path!, "*.dll")
                        .Where(a => currentAssemblies.All(aa => aa.FullName != a))
                        .Select(a => Assembly.Load(AssemblyName.GetAssemblyName(a)))
                        .Where(PredicateDiscoverableAssemblies))
                .Union([Assembly.GetEntryAssembly()])
                .GroupBy(x => x!.FullName)
                .Select(x => x.First())
        ];

        static bool PredicateDiscoverableAssemblies(Assembly a) 
            => a.GetCustomAttribute<TechLeagueDiscoverableAssemblyAttribute>() is not null;
    }
}
