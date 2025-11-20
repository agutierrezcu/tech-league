using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel;

namespace Infrastructure.Database.StronglyTypedGuid;

internal sealed class ValueConverterSelector : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverterSelector
{
    private static readonly Type _stronglyTypedGenericInterface = typeof(IStronglyTyped<>);

    private static readonly Type _stronglyTypedGuidValueConverterGenericType =
        typeof(ValueConverter<,>);

    // The dictionary in the base type is private, so we need our own one here.
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
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
        if (underlyingProviderType is null)
        {
            if (!underlyingModelType.ImplementsGeneric(_stronglyTypedGenericInterface, 
                    out Type implementedType))
            {
                yield break;
            }

            Type genericArgType = implementedType.GetGenericArguments()[0];

            yield return _converters.GetOrAdd(
                (underlyingModelType, genericArgType),
                k =>
                {
                    Type converterType = _stronglyTypedGuidValueConverterGenericType
                        .MakeGenericType(underlyingModelType, genericArgType);

                    // Create an instance of the converter whenever it's requested.
                    Func<ValueConverterInfo, ValueConverter> factory =
                        info => (ValueConverter)Activator.CreateInstance(converterType, info.MappingHints);

                    // Build the info for our strongly-typed ID => int converter
                    return new ValueConverterInfo(modelClrType, genericArgType, factory);
                }
            );
        }
    }
}
