using System;

namespace SharedKernel.Domain.Entities
{
    [Flags]
    public enum State
    {
        /// <summary>
        /// The entity is not being tracked by the context.
        /// An entity is in this state immediately after it has been created with the new operator
        /// or with one of the <see cref="T:System.Data.Entity.DbSet" /> Create methods.
        /// </summary>
        Detached = 1,
        /// <summary>
        /// The entity is being tracked by the context and exists in the database, and its property
        /// values have not changed from the values in the database.
        /// </summary>
        Unchanged = 2,
        /// <summary>
        /// The entity is being tracked by the context but does not yet exist in the database.
        /// </summary>
        Added = 4,
        /// <summary>
        /// The entity is being tracked by the context and exists in the database, but has been marked
        /// for deletion from the database the next time SaveChanges is called.
        /// </summary>
        Deleted = 8,
        /// <summary>
        /// The entity is being tracked by the context and exists in the database, and some or all of its
        /// property values have been modified.
        /// </summary>
        Modified = 16 // 0x00000010
    }
}