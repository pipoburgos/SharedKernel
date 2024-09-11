namespace SharedKernel.Domain.Specifications;

/// <summary> . </summary>
public class PropertiesContainsOrEqualSpecification<T> : ISpecification<T> where T : class
{
    private readonly IEnumerable<Property> _properties;

    /// <summary> . </summary>
    public PropertiesContainsOrEqualSpecification(IEnumerable<Property> properties)
    {
        _properties = properties;
    }

    /// <summary> . </summary>
    public Expression<Func<T, bool>> SatisfiedBy()
    {
        ISpecification<T> filter = new TrueSpecification<T>();

        if (_properties == default!)
            return filter.SatisfiedBy();

        foreach (var property in _properties.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            var propertyInfo = typeof(T).GetProperties().Where(p => p.CanWrite)
                .SingleOrDefault(t => t.Name.ToUpper() == property.Field.ToUpper());

            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            filter = filter.And(new PropertyContainsOrEqualSpecification<T>(propertyInfo, property.Value));
        }


        return filter.SatisfiedBy();
    }
}
