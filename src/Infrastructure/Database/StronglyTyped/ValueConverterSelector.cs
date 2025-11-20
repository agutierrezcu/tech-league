using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel;

namespace Infrastructure.Database.StronglyTyped;

internal sealed class ValueConverterSelector : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverterSelector
{
    private static readonly Type StronglyTypedGenericInterface = typeof(IStronglyTyped<>);

    private static readonly Type StronglyTypedGuidValueConverterGenericType =
        typeof(ValueConverter<,>);

    // The dictionary in the base type is private, so we need our own one here.
    private readonly ConcurrentDictionary<(Type ModelClrType, Type GenericType), ValueConverterInfo> _converters
        = new();

    public ValueConverterSelector(ValueConverterSelectorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        IEnumerable<ValueConverterInfo> baseConverters = base.Select(modelClrType, providerClrType);
        foreach (ValueConverterInfo converter in baseConverters)
        {
            yield return converter;
        }

        // Extract the "real" type T from Nullable<T> if required
        Type underlyingModelType = modelClrType.UnwrapNullableType();
        Type? underlyingProviderType = providerClrType.UnwrapNullableType();

        // 'null' means 'get any value converters for the modelClrType'
        if (underlyingProviderType is not null)
        {
            yield break;
        }

        if (!underlyingModelType.ImplementsGeneric(StronglyTypedGenericInterface,
                out Type implementedType))
        {
            yield break;
        }

        Type genericArgType = implementedType.GetGenericArguments()[0];

        yield return _converters.GetOrAdd(
            (underlyingModelType, genericArgType),
            kvp =>
            {
                Type converterType = StronglyTypedGuidValueConverterGenericType
                    .MakeGenericType(kvp.ModelClrType, kvp.GenericType);

                // Build the info for our strongly-typed ID => int converter
                return new ValueConverterInfo(kvp.ModelClrType, kvp.GenericType, Factory);

                // Create an instance of the converter whenever it's requested.
                ValueConverter Factory(ValueConverterInfo info) 
                    => (ValueConverter)Activator.CreateInstance(converterType, info.MappingHints);
            }
        );
    }
}
