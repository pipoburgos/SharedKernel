namespace SharedKernel.Infrastructure.Data;

/// <summary>
/// <example>
/// Sample usage:
/// <code>
/// public virtual async Task Populate(CancellationToken cancellationToken)
/// {
///     if (_dbContext.Database.IsSqlServer())
///         await _dbContext.Database.MigrateAsync();
///
///     _dbContext.Set&lt;Entity&gt;().AddOrNothing(new Entity());
///     _connection.Execute("INSERT INTO Entity VALUES (1)");
/// </code>
/// </example>
/// </summary>
public interface IPopulateDatabase
{
    /// <summary>
    /// <example>
    /// Sample usage:
    /// <code>
    /// public virtual async Task Populate(CancellationToken cancellationToken)
    /// {
    ///     if (_dbContext.Database.IsSqlServer())
    ///         await _dbContext.Database.MigrateAsync();
    ///
    ///     _dbContext.Set&lt;Entity&gt;().AddOrNothing(new Entity());
    ///     _connection.Execute("INSERT INTO Entity VALUES (1)");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task Populate(CancellationToken cancellationToken);
}