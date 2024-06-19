using Dapper;
using System.Data;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

/// <summary> </summary>
public static class DynamicParametersExtensions
{
    /// <summary> </summary>
    public static void Add(this DynamicParameters dynamicParameters, string paramName, object? value)
    {
        if (value == default) return;

        if (value.GetType().IsAssignableFrom(typeof(DateTime)) ||
            value.GetType().IsAssignableFrom(typeof(DateTime?)))
            dynamicParameters.Add(paramName, value, DbType.DateTime2);
        else
            dynamicParameters.Add(paramName, value);
    }

    /// <summary> </summary>
    public static DynamicParameters Create(object obj)
    {
        var parametrosLista = obj
            .GetType()
            .GetProperties()
            .Select(a => new KeyValuePair<string, object?>(a.Name, GetProperty(obj, a.Name)))
            .Where(x => x.Value != null)
            .ToList();

        return new DynamicParameters(parametrosLista);
    }

    /// <summary> </summary>
    private static object? GetProperty(object obj, string name)
    {
        var propertyInfo = obj.GetType().GetProperty(name);
        return propertyInfo == null || !propertyInfo.CanRead
            ? default
            : propertyInfo.GetValue(obj);
    }
}
