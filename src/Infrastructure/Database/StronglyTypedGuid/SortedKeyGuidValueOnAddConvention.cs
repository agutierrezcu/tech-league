using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SharedKernel;

namespace Infrastructure.Database.StronglyTypedGuid;

internal sealed class SortedKeyGuidValueOnAddConvention : IModelFinalizingConvention
{
    private static readonly Type _sortedGuidValueGeneratorGenericType =
        typeof(SortedGuidValueGenerator<>);

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        foreach (IConventionProperty? property in modelBuilder.Metadata.GetEntityTypes()
                     .SelectMany(
                         entityType => entityType.GetDeclaredProperties()
                             .Where(property => property.IsPrimaryKey())))
        {
            Type keyPropertyType = property.ClrType.UnwrapNullableType();

            if (!keyPropertyType
                    .ImplementsGeneric(typeof(IStronglyTyped<>), out Type implementedType))
            {
                continue;
            }

            Type genericArgType = implementedType.GetGenericArguments()[0];

            if (genericArgType != typeof(Guid))
            {
                continue;
            }

            Type valueGeneratorType = _sortedGuidValueGeneratorGenericType
                .MakeGenericType(keyPropertyType);

            IConventionPropertyBuilder builder = property.Builder;

            builder.ValueGenerated(ValueGenerated.OnAdd);
            builder.HasValueGenerator(valueGeneratorType);
        }
    }
}
