using SharedKernel.Domain.Extensions;

namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class TheComparisonMatchesSpecification<T> : Specification<T>
{
    private readonly string _value;
    private readonly string? _propertyName;
    private readonly Operator? _operator;

    /// <summary>  </summary>
    public TheComparisonMatchesSpecification(string value, string? propertyName, Operator? @operator)
    {
        _value = value;
        _propertyName = propertyName;
        _operator = @operator;
    }

    /// <summary> </summary>
    /// <returns></returns>
    public override Expression<Func<T, bool>> SatisfiedBy()
    {
        return ExpressionHelper.GenerateExpression<T>(_value, _propertyName, _operator);
    }
}