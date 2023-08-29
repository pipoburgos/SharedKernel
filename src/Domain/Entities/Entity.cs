namespace SharedKernel.Domain.Entities;

/// <summary>  </summary>
/// <typeparam name="TId"></typeparam>
public abstract class Entity<TId> : IEntity<TId> where TId : notnull
{
    #region Constructors

    /// <summary> Entity constructor for ORMs. </summary>
    protected Entity() { }

    /// <summary> Entity constructor. </summary>
    /// <param name="id">The identifier</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    #endregion

    #region Properties

    /// <summary> The object identifier. </summary>
    public TId Id { get; protected set; } = default!;

    #endregion

    #region Public Methods

    /// <summary> Check if this entity is transient, ie, without identity at this moment. </summary>
    /// <returns>True if entity is transient, else false</returns>
    public bool IsTransient()
    {
        return EqualityComparer<TId>.Default.Equals(default!, Id);
    }

    #endregion

    #region Overrides Methods

    /// <summary> <see cref="M:System.Object.Equals"/> </summary>
    /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
    /// <returns><see cref="M:System.Object.Equals"/></returns>
    public override bool Equals(object? obj)
    {
        if (obj == default)
            return false;

        if (!(obj is Entity<TId>))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        var item = (Entity<TId>)obj;

        var equals = Id.Equals(item.Id);

        return equals;
    }

    /// <summary> <see cref="M:System.Object.GetHashCode"/> </summary>
    /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        if (IsTransient())
            return base.GetHashCode();

        // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Id.GetHashCode() ^ 31;
    }

    /// <summary> Compare by identifier. </summary>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary> Compare by identifier. </summary>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }

    #endregion
}
