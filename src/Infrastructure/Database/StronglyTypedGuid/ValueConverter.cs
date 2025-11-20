using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel;

namespace Infrastructure.Database.StronglyTypedGuid;

internal sealed class ValueConverter<TStronglyTypedId, TValue>
    : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<TStronglyTypedId, TValue>
       where TStronglyTypedId : IStronglyTyped<TValue>
{
    public ValueConverter(ConverterMappingHints mappingHints)
      : base(
            stronglyTyped => stronglyTyped.Value,
            value => (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), value),
            mappingHints)
    {
    }
}
