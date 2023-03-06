using SharedKernel.Domain.Entities.Paged;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace SharedKernel.Domain.Extensions;

/// <summary>
/// 
/// </summary>
public static class ExpressionHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyInfo"></param>
    /// <param name="operator"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Expression<Func<T, bool>> GenerateExpression<T>(PropertyInfo propertyInfo, Operator? @operator, string value)
    {
        var isString = typeof(T) == typeof(string) || propertyInfo?.PropertyType == typeof(string);

        var isDate = typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?) ||
                     propertyInfo?.PropertyType == typeof(DateTime) || propertyInfo?.PropertyType == typeof(DateTime?);

        var parameterExpression = Expression.Parameter(typeof(T), "x");

        Expression propertyExpression = propertyInfo == default
            ? parameterExpression
            : Expression.Property(parameterExpression, propertyInfo.Name);

        ConstantExpression GetExpression()
        {
            return Expression.Constant(isString
                ? value
                : Convert.ChangeType(value, propertyInfo != default ? propertyInfo.PropertyType : typeof(T)));
        }

        Expression binaryExpression;
        switch (@operator)
        {
            case Operator.EqualTo:
                binaryExpression = Expression.Equal(propertyExpression, GetExpression());
                break;

            case Operator.NotEqualTo:
                binaryExpression = Expression.NotEqual(propertyExpression, GetExpression());
                break;

            case Operator.IsEqualToNull:
                binaryExpression = Expression.Equal(propertyExpression, Expression.Constant(null));
                break;

            case Operator.IsNotEqualToNull:
                binaryExpression = Expression.NotEqual(propertyExpression, Expression.Constant(null));
                break;

            case Operator.LessThan:
                binaryExpression = Expression.LessThan(propertyExpression, Expression.Constant(null));
                break;

            case Operator.LessThanOrEqualTo:
                binaryExpression = Expression.LessThanOrEqual(propertyExpression, GetExpression());
                break;

            case Operator.GreaterThan:
                binaryExpression = Expression.GreaterThan(propertyExpression, GetExpression());
                break;

            case Operator.GreaterThanOrEqualTo:
                binaryExpression = Expression.GreaterThanOrEqual(propertyExpression, GetExpression());
                break;

            case Operator.StartsWith:
                if (!isString)
                    throw new Exception("Method not found string.StartsWith");

                binaryExpression = StartsWith(propertyExpression, GetExpression());
                break;

            case Operator.NotStartsWith:
                if (!isString)
                    throw new Exception("Method not found string.StartsWith");

                binaryExpression = NotStartsWith(propertyExpression, GetExpression());
                break;

            case Operator.EndsWith:
                if (!isString)
                    throw new Exception("Method not found string.EndsWith");

                binaryExpression = EndsWith(propertyExpression, GetExpression());
                break;

            case Operator.NotEndsWith:
                if (!isString)
                    throw new Exception("Method not found string.EndsWith");

                binaryExpression = NotEndsWith(propertyExpression, GetExpression());
                break;

            case Operator.Contains:
                if (!isString)
                    throw new Exception("Method not found string.Contains");

                binaryExpression = Contains(propertyExpression, GetExpression());
                break;

            case Operator.NotContains:
                if (!isString)
                    throw new Exception("Method not found string.Contains");

                binaryExpression = NotContains(propertyExpression, GetExpression());
                break;

            case Operator.IsEmpty:
                if (!isString)
                    throw new Exception("Only string property");

                binaryExpression = Expression.Equal(propertyExpression, Expression.Constant(string.Empty));
                break;

            case Operator.IsNotEmpty:
                if (!isString)
                    throw new Exception("Only string property");

                binaryExpression =
                    Expression.Not(Expression.Equal(propertyExpression, Expression.Constant(string.Empty)));
                break;

            case Operator.DateEqual:
                if (!isDate)
                    throw new Exception("Only date property");

                return DateEqual<T>(propertyExpression, value, parameterExpression);

            case Operator.NotDateEqual:
                if (!isDate)
                    throw new Exception("Only date property");

                return NotDateEqual<T>(propertyExpression, value, parameterExpression);

            //case null:
            default:
                if (isDate)
                    return DateEqual<T>(propertyExpression, value, parameterExpression);

                binaryExpression = isString
                    ? Contains(propertyExpression, GetExpression())
                    : Expression.Equal(propertyExpression, GetExpression());
                break;
        }

        return Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
    }

    private static Expression<Func<T, bool>> DateEqual<T>(Expression expression, string value, ParameterExpression parameterExpression)
    {
        var dayStart = DateTime.Parse(value, CultureInfo.InvariantCulture).ToUniversalTime();
        var dayEnd = dayStart.AddDays(1);

        var leftExpression = Expression.GreaterThanOrEqual(expression, Expression.Constant(dayStart));
        var left = Expression.Lambda<Func<T, bool>>(leftExpression, parameterExpression);

        var rightExpression = Expression.LessThan(expression, Expression.Constant(dayEnd));
        var right = Expression.Lambda<Func<T, bool>>(rightExpression, parameterExpression);

        return left.Compose(right, Expression.And);
    }

    private static Expression<Func<T, bool>> NotDateEqual<T>(Expression expression, string value, ParameterExpression parameterExpression)
    {
        var dayStart = DateTime.Parse(value, CultureInfo.InvariantCulture).ToUniversalTime();
        var dayEnd = dayStart.AddDays(1);

        var leftExpression = Expression.LessThan(expression, Expression.Constant(dayStart));
        var left = Expression.Lambda<Func<T, bool>>(leftExpression, parameterExpression);

        var rightExpression = Expression.GreaterThanOrEqual(expression, Expression.Constant(dayEnd));
        var right = Expression.Lambda<Func<T, bool>>(rightExpression, parameterExpression);

        return left.Compose(right, Expression.Or);
    }

    /// <summary>  </summary>
    public static Expression Contains(Expression expression, Expression valueExpression)
    {
        var methodInfo = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

        if (methodInfo == null)
            throw new Exception("Method not found string.Contains");

        return Expression.Call(expression, methodInfo, valueExpression);
    }

    /// <summary>  </summary>
    public static Expression NotContains(Expression expression, Expression valueExpression)
    {
        return Expression.Not(Contains(expression, valueExpression));
    }

    /// <summary>  </summary>
    public static Expression StartsWith(Expression expression, Expression valueExpression)
    {
        var methodInfo = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });

        if (methodInfo == null)
            throw new Exception("Method not found string.StartsWith");

        return Expression.Call(expression, methodInfo, valueExpression);
    }

    /// <summary>  </summary>
    public static Expression NotStartsWith(Expression expression, Expression valueExpression)
    {
        return Expression.Not(StartsWith(expression, valueExpression));
    }

    /// <summary>  </summary>
    public static Expression EndsWith(Expression expression, Expression valueExpression)
    {
        var methodInfo = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) });

        if (methodInfo == null)
            throw new Exception("Method not found string.EndsWith");

        return Expression.Call(expression, methodInfo, valueExpression);
    }

    /// <summary>  </summary>
    public static Expression NotEndsWith(Expression expression, Expression valueExpression)
    {
        return Expression.Not(EndsWith(expression, valueExpression));
    }

    /// <summary>
    ///     Convert a string to lambda expression
    ///     Example => "Person.Child.Name" : x => x.Person.Child.Name
    /// </summary>
    public static LambdaExpression GetLambdaExpressions<T>(string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
            return Generate<T>().TryGetValue(propertyName, out var result) ? result : null;

        var t = typeof(T);
        var parameter = Expression.Parameter(t, "x");
        // Create the lambda expression: x => x
        return Expression.Lambda(Expression.Convert(parameter, parameter.Type), parameter);
    }

    private static IDictionary<string, LambdaExpression> Generate<T>()
    {
        var storage = new Dictionary<string, LambdaExpression>();

        var t = typeof(T);
        var parameter = Expression.Parameter(t, "x");

        if (!new IsClassTypeSpecification<T>().SatisfiedBy().Compile()(default))
            return storage;

        foreach (var property in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var lambdaExpression = Expression.Lambda(propertyAccess, parameter);
            storage[property.Name] = lambdaExpression;
        }

        return storage;
    }
}
