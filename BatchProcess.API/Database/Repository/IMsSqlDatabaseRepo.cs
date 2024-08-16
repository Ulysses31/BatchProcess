using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BatchProcess.Api.Database.Repository;

/// <summary>
/// IMsSqlDatabaseBaseRepo interface
/// </summary>
/// <typeparam name="TEntity">BaseEntity</typeparam>
public interface IMsSqlDatabaseBaseRepo<TEntity>
{
    #region Synchronous Methods

    /// <summary>
    /// Returns an <see cref="IQueryable{TEntity}"/> representing all entities in the database.
    /// </summary>
    IQueryable<TEntity> Filter();

    /// <summary>
    /// Returns an <see cref="IQueryable{TEntity}"/> representing all entities in the database without tracking changes.
    /// </summary>
    IQueryable<TEntity> FilterAsNoTracking();

    /// <summary>
    /// Returns an enumerable collection of entities filtered by the specified predicate.
    /// </summary>
    IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

    /// <summary>
    /// Returns an enumerable collection of entities filtered by the specified predicate without tracking changes.
    /// </summary>
    IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate);

    /// <summary>
    /// Returns an entity with the specified ID.
    /// </summary>
    TEntity? Filter(string id);

    /// <summary>
    /// Creates a new entity in the database.
    /// </summary>
    TEntity Create(TEntity entity);

    /// <summary>
    /// Updates an entity in the database based on the specified predicate.
    /// </summary>
    TEntity Update(Func<TEntity, bool> predicate, TEntity entity);

    /// <summary>
    /// Deletes an entity from the database.
    /// </summary>
    TEntity Delete(TEntity entity);

    #endregion

    #region Asynchronous Methods

    /// <summary>
    /// Returns an asynchronous <see cref="Task"/> representing all entities in the database.
    /// </summary>
    Task<IQueryable<TEntity>> FilterAsync();

    /// <summary>
    /// Returns an asynchronous <see cref="Task"/> representing all entities in the database without tracking changes.
    /// </summary>
    Task<IQueryable<TEntity>> FilterAsNoTrackingAsync();

    /// <summary>
    /// Returns an asynchronous enumerable collection of entities filtered by the specified predicate.
    /// </summary>
    Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate);

    /// <summary>
    /// Returns an asynchronous enumerable collection of entities filtered by the specified predicate without tracking changes.
    /// </summary>
    Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate);

    /// <summary>
    /// Returns an asynchronous entity with the specified ID.
    /// </summary>
    Task<TEntity?> FilterAsync(string id);

    /// <summary>
    /// Creates a new entity in the database asynchronously.
    /// </summary>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Updates an entity in the database asynchronously based on the specified predicate.
    /// </summary>
    Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity);

    /// <summary>
    /// Deletes an entity from the database asynchronously.
    /// </summary>
    Task<TEntity> DeleteAsync(TEntity entity);

    #endregion

    /// <summary>
    /// Saves changes to the database context.
    /// </summary>
    int SaveChanges(DbContext context);

    /// <summary>
    /// Saves changes to the database context asynchronously.
    /// </summary>
    Task<int> SaveChangesAsync(DbContext context);
}
