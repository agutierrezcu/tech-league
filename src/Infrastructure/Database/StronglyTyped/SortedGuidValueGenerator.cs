using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SharedKernel;

namespace Infrastructure.Database.StronglyTyped;

internal sealed class SortedGuidValueGenerator<TStronglyTyped, TValue>
    : ValueGenerator<TStronglyTyped>
        where TStronglyTyped : notnull, IStronglyTyped<TValue>, new()
{
    public override bool GeneratesTemporaryValues => false;

    public override TStronglyTyped Next(EntityEntry entry)
    {
        return new();
    }
}
