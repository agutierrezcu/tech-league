using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SharedKernel;

namespace Infrastructure.Database.StronglyTyped;

internal sealed class SortedGuidValueOnAddConvention : IModelFinalizingConvention
{
    private static readonly Type SortedGuidValueGeneratorGenericType =
        typeof(SortedGuidValueGenerator<,>);

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        foreach (IConventionProperty? property in modelBuilder.Metadata.GetEntityTypes()
                     .SelectMany(entityType => entityType.GetDeclaredProperties()
                         .Where(property => property.IsPrimaryKey())))
        {
            Type keyPropertyType = property.ClrType.UnwrapNullableType();

            if (!keyPropertyType.ImplementsGeneric(
                    typeof(IStronglyTyped<>), out Type implementedType))
            {
                continue;
            }

            Type genericArgType = implementedType.GetGenericArguments()[0];

            if (genericArgType != typeof(Guid))
            {
                continue;
            }

            Type valueGeneratorType = SortedGuidValueGeneratorGenericType
                .MakeGenericType(keyPropertyType, genericArgType);

            IConventionPropertyBuilder builder = property.Builder;

            builder.ValueGenerated(ValueGenerated.OnAdd);
            builder.HasValueGenerator(valueGeneratorType);
        }
    }
}
