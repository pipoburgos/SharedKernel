using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

internal class OriginalEntry
{
    public OriginalEntry(EntityEntry entry, PropertyValues originalValues, EntityState entityState)
    {
        Entry = entry;
        OriginalValues = originalValues;
        State = entityState;
    }

    public EntityEntry Entry { get; }

    public PropertyValues OriginalValues { get; }

    public EntityState State { get; }
}
