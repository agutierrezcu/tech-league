using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SharedKernel;

namespace Infrastructure.Database.StronglyTypedGuid;

internal sealed class SortedGuidValueGenerator<TStronglyTyped> 
    : ValueGenerator<TStronglyTyped>
        where TStronglyTyped : IStronglyTyped<Guid>
{
    public override bool GeneratesTemporaryValues => false;

    public override TStronglyTyped Next(EntityEntry entry)
    {
        return (TStronglyTyped) Activator.CreateInstance(
            typeof(TStronglyTyped), Guid.CreateVersion7());
    }
}
