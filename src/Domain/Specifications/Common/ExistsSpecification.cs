using System;
using System.Linq;
using System.Linq.Expressions;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Specifications.Common
{
    public interface IName
    {
        string Name { get; set; }
    }

    public class ExistsSpecification<TKey, TEntity, TTranslation> : Specification<TEntity>
        where TEntity : class, IEntityIsTranslatable<TKey, TEntity, TTranslation>
        where TTranslation : IEntityTranslated<TKey, TEntity>, IName

    {
    private readonly TKey _id;
    private readonly string _name;

    public ExistsSpecification(TKey id, string name)
    {
        _id = id;
        _name = name;
    }

    public override Expression<Func<TEntity, bool>> SatisfiedBy()
    {
        return l => l.Translations.Any(t => !t.EntityId.Equals(_id) && t.Name.ToUpper() == _name.ToUpper());
    }
    }
}
