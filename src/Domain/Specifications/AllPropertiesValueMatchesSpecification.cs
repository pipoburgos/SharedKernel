namespace SharedKernel.Domain.Specifications;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AllPropertiesValueMatchesSpecification<T> : ISpecification<T>
{
    private readonly IEnumerable<Property> _properties;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="properties"></param>
    public AllPropertiesValueMatchesSpecification(IEnumerable<Property> properties)
    {
        _properties = properties;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Expression<Func<T, bool>> SatisfiedBy()
    {
        ISpecification<T> filter = new TrueSpecification<T>();

        if (_properties == default!)
            return filter.SatisfiedBy();

        if (!new IsClassTypeSpecification<T>().SatisfiedBy().Compile()(default!))
        {
            var property = _properties.SingleOrDefault();
            if (property != default)
                filter = filter.And(
                    new TheComparisonMatchesSpecification<T>(property.Value, default, property.Operator));

            return filter.SatisfiedBy();
        }

        return _properties
            .Aggregate(filter, (current, property) =>
                current.And(
                    new TheComparisonMatchesSpecification<T>(property.Value, property.Field, property.Operator)))
            .SatisfiedBy();
    }
}
