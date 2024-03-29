﻿namespace SharedKernel.Domain.Aggregates;

/// <summary> Root aggregate with creation and modification audit. </summary>
public abstract class AggregateRootAuditable<TId> : AggregateRoot<TId>, IEntityAuditable where TId : notnull
{
    #region Constructor

    /// <summary> Aggregate Root Auditable constructor for ORMs. </summary>
    protected AggregateRootAuditable() { }

    /// <summary> Aggregate Root Auditable constructor for ORMs. </summary>
    protected AggregateRootAuditable(TId id) : base(id) { }

    /// <summary> Aggregate Root Auditable Constructor. </summary>
    /// <param name="id">Identifier</param>
    /// <param name="createdAt">Creation Date</param>
    /// <param name="createdBy">Creation user identifier</param>
    protected AggregateRootAuditable(TId id, DateTime createdAt, Guid createdBy) : base(id)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    #endregion

    #region Properties

    /// <summary> Creator user identifier. </summary>
    public Guid CreatedBy { get; private set; }

    /// <summary> Creation date. </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary> Modifier user identifier. </summary>
    public Guid? LastModifiedBy { get; private set; }

    /// <summary> Modification date. </summary>
    public DateTime? LastModifiedAt { get; private set; }

    #endregion

    /// <summary> Sets the creation auditable properties. </summary>
    public void Create(DateTime createdAt, Guid createdBy)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    /// <summary> Sets the modification auditable properties. </summary>
    public void Change(DateTime lastModifiedAt, Guid lastModifiedBy)
    {
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
    }
}
