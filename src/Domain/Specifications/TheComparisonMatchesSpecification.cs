using SharedKernel.Domain.Extensions;
using System.Reflection;

namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class TheComparisonMatchesSpecification<T> : Specification<T>
{
    private readonly Operator? _operator;
    private PropertyInfo? PropertyInfo { get; }

    private string Value { get; }

    /// <summary>  </summary>
    public TheComparisonMatchesSpecification(PropertyInfo? propertyInfo, string value, Operator? @operator)
    {
        _operator = @operator;
        PropertyInfo = propertyInfo;
        Value = value;
    }

    /// <summary> </summary>
    /// <returns></returns>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return ExpressionHelper.GenerateExpression<T>(PropertyInfo, _operator, Value);
    }
}