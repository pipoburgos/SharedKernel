namespace SharedKernel.Domain.Entities
{
    /// <summary>  </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        #region Constructors

        /// <summary> Entity constructor for ORMs. </summary>
        protected Entity() { }

        /// <summary> Entity constructor. </summary>
        /// <param name="id">The identifier</param>
        protected Entity(TKey id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        /// <summary> The object identifier. </summary>
        public TKey Id { get; protected set; }

        #endregion

        #region Public Methods

        /// <summary> Check if this entity is transient, ie, without identity at this moment. </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(default, Id);
        }

        #endregion

        #region Overrides Methods

        /// <summary> <see cref="M:System.Object.Equals"/> </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TKey>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var item = (Entity<TKey>)obj;

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        /// <summary> Compare by identifier. </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }

        #endregion
    }
}