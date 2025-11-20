using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Database.StronglyTyped;

internal static class TypeUtils
{
    internal static Type UnwrapNullableType(this Type? type)
    {
        return type switch
        {
            null => null,
            _ => Nullable.GetUnderlyingType(type) ?? type
        };
    }

    internal static bool Implements<T>(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.GetInterface(typeof(T).Name) is not null;
    }

    internal static bool ImplementsGeneric(this Type? type, Type genericType)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(genericType);

        return type.ImplementsGeneric(genericType, out _);
    }

    internal static bool ImplementsGeneric(this Type? type, Type genericType,
        [NotNullWhen(true)] out Type? implementedType)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(genericType);

        if (!genericType.IsGenericType)
        {
            throw new InvalidOperationException("Type parameter must be a generic.");
        }

        implementedType = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);

        return implementedType is not null;
    }
}
