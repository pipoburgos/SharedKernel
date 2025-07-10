﻿using SharedKernel.Domain.Exceptions;
using System.Reflection;

namespace SharedKernel.Domain.Specifications;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class PropertyContainsOrEqualSpecification<T> : Specification<T> where T : class
{
    private PropertyInfo PropertyInfo { get; }

    private string Value { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <param name="value"></param>
    public PropertyContainsOrEqualSpecification(PropertyInfo propertyInfo, string value)
    {
        PropertyInfo = propertyInfo;
        Value = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        var parameterExp = Expression.Parameter(typeof(T), "type");
        var propertyExp = Expression.Property(parameterExp, PropertyInfo.Name);

        var methodInfo = PropertyInfo.PropertyType == typeof(string)
            ? typeof(string).GetMethod("Contains", [typeof(string)])
            : PropertyInfo.PropertyType.GetMethod("Equals", [PropertyInfo.PropertyType]);

        if (methodInfo == null)
            throw new TextException("Method not found PropertyContainsOrEqualSpecification.SatisfiedBy");

        // Property value
        object value;
        if (PropertyInfo.PropertyType != typeof(string))
            value = Convert.ChangeType(Value, PropertyInfo.PropertyType);
        else
            value = Value;

        var someValue = Expression.Constant(value, PropertyInfo.PropertyType);
        var containsMethodExp = Expression.Call(propertyExp, methodInfo, someValue);
        return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
    }
}