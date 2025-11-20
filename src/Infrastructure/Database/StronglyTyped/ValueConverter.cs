using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel;

namespace Infrastructure.Database.StronglyTyped;

internal sealed class ValueConverter<TStronglyTypedId, TValue>
    : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<TStronglyTypedId, TValue>
       where TStronglyTypedId : notnull, IStronglyTyped<TValue>, new()
{
    public ValueConverter(ConverterMappingHints mappingHints)
        : base(
            stronglyTyped => stronglyTyped.Value,
            value => (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), value),
            mappingHints)
    {
    }
}
