namespace SharedKernel.Domain.Extensions;

/// <summary> .  </summary>
public static class ConvertTypeExtensions
{

    /// <summary>
    /// Checks if the value is convertible to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsConvertible<T>(this object value)
    {
        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return value != null && targetType.IsAssignableFrom(value.GetType());
    }

    /// <summary>
    /// Determines if a type is nullable.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is nullable, otherwise false.</returns>
    public static bool IsNullable(this Type type)
    {
        return Nullable.GetUnderlyingType(type) != null;
    }

}
