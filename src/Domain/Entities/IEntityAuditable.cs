namespace SharedKernel.Domain.Entities;

/// <summary>
/// https://stackoverflow.com/questions/26355486/entity-framework-6-audit-track-changes
/// </summary>
public interface IEntityAuditable
{
    /// <summary> . </summary>
    Guid CreatedBy { get; }

    /// <summary> . </summary>
    DateTime CreatedAt { get; }

    /// <summary> . </summary>
    Guid? LastModifiedBy { get; }

    /// <summary> . </summary>
    DateTime? LastModifiedAt { get; }

    /// <summary> Sets the creation auditable properties. </summary>
    void Create(DateTime createdAt, Guid createdBy);

    /// <summary> Sets the modification auditable properties. </summary>
    void Change(DateTime lastModifiedAt, Guid lastModifiedBy);
}
