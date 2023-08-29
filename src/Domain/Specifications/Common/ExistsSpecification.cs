using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Specifications.Common;

/// <summary>  </summary>
public interface IName
{
    /// <summary>  </summary>
    string Name { get; set; }
}

/// <summary>  </summary>
public class ExistsSpecification<TId, TEntity, TTranslation> : Specification<TEntity>
    where TEntity : class, IEntityIsTranslatable<TId, TEntity, TTranslation>
    where TTranslation : IEntityTranslated<TId, TEntity>, IName
    where TId : notnull
{
    private readonly TId _id;
    private readonly string _name;

    /// <summary>  </summary>
    public ExistsSpecification(TId id, string name)
    {
        _id = id;
        _name = name;
    }

    /// <summary>  </summary>
    public override Expression<Func<TEntity, bool>> SatisfiedBy()
    {
        return l => l.Translations.Any(t => !t.EntityId!.Equals(_id) && t.Name.ToUpper() == _name.ToUpper());
    }
}
