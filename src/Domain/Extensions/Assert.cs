namespace SharedKernel.Domain.Extensions;

/// <summary> Checks values of an object. </summary>
public static class Assert
{
    /// <summary> Check that all properties have the default value. </summary>
    /// <param name="object">An object</param>
    public static bool AllPropertiesHaveDefaultValue(object? @object)
    {
        return @object == default ||
               @object.GetType().GetProperties().All(pi => pi.GetValue(@object, default) == default);
    }
}
