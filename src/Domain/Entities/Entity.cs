// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
#pragma warning disable S3875
namespace SharedKernel.Domain.Entities;

/// <summary> . </summary>
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
    /// <param name="obj"> The object to compare with the current object. </param>
    /// <returns><see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is Entity<TId>))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        var item = (Entity<TId>)obj;

        return Id.Equals(item.Id);
    }

    /// <summary> <see cref="M:System.Object.GetHashCode"/> </summary>
    /// <returns> A hash code for the current object. </returns>
    public override int GetHashCode()
    {
        if (IsTransient())
            return base.GetHashCode();

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
