using BatchProcess.Api.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BatchProcess.Api.Database.Repository
{
    /// <summary>
    /// Repository class for managing CRUD operations with a Microsoft SQL Server database.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being managed, which must be a class.</typeparam>
    public class MsSqlDatabaseRepo<TEntity> : IMsSqlDatabaseBaseRepo<TEntity> where TEntity : class
    {
        private const string NullEntity = "You must provide an entity.";
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlDatabaseRepo{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
        public MsSqlDatabaseRepo(DbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Sync Methods

        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/> representing all entities.</returns>
        public IQueryable<TEntity> Filter()
        {
            try
            {
                return _context.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Retrieves all entities from the database without tracking changes.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/> representing all entities without tracking changes.</returns>
        public IQueryable<TEntity> FilterAsNoTracking()
        {
            try
            {
                return _context.Set<TEntity>().AsNoTracking();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Retrieves entities from the database that match the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> containing the filtered entities.</returns>
        public IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
        {
            try
            {
                return _context.Set<TEntity>().Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Retrieves entities from the database that match the specified predicate without tracking changes.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> containing the filtered entities without tracking changes.</returns>
        public IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate)
        {
            try
            {
                return _context.Set<TEntity>().AsNoTracking().Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the ID is null.</exception>
        public TEntity? Filter(string id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                }

                return _context.Set<TEntity>().Find(Convert.ToInt32(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>The created entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        public TEntity Create(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                }

                _context.Set<TEntity>().Add(entity);
                SaveChanges(_context);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted. Must not be null.</param>
        /// <returns>The deleted entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the deletion process.</exception>
        /// <remarks>
        /// This method removes the given entity from the database context and saves the changes.
        /// </remarks>
        public TEntity Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                }

                _context.Set<TEntity>().Remove(entity);
                SaveChanges(_context);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Updates an entity in the database that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="entity">The entity with updated values. Must not be null.</param>
        /// <returns>The updated entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the entity to update is not found or the provided entity is null.
        /// </exception>
        /// <exception cref="Exception">Thrown when an error occurs during the update process.</exception>
        public TEntity Update(Func<TEntity, bool> predicate, TEntity entity)
        {
            try
            {
                TEntity entityToUpdate = Filter(predicate).FirstOrDefault() ?? throw new ArgumentNullException(nameof(entityToUpdate));
                entityToUpdate = entity ?? throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                _context.Entry(entity).State = EntityState.Modified;
                SaveChanges(_context);
                return entityToUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        #endregion

        #region Async Methods

        /// <summary>
        /// Asynchronously retrieves all entities from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IQueryable{TEntity}"/> representing all entities.</returns>
        public async Task<IQueryable<TEntity>> FilterAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<TEntity>());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously retrieves all entities from the database without tracking changes.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IQueryable{TEntity}"/> representing all entities without tracking changes.</returns>
        public async Task<IQueryable<TEntity>> FilterAsNoTrackingAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<TEntity>().AsNoTracking());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously retrieves entities from the database that match the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{TEntity}"/> containing the filtered entities.</returns>
        public async Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate)
        {
            try
            {
                return await Task.FromResult(_context.Set<TEntity>().Where(predicate));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously retrieves entities from the database that match the specified predicate without tracking changes.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{TEntity}"/> containing the filtered entities without tracking changes.</returns>
        public async Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate)
        {
            try
            {
                return await Task.FromResult(_context.Set<TEntity>().AsNoTracking().Where(predicate));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the entity with the specified ID, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the ID is null.</exception>
        public async Task<TEntity?> FilterAsync(string id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                }

                return await _context.Set<TEntity>().FindAsync(Convert.ToInt32(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the created entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                }

                await _context.Set<TEntity>().AddAsync(entity);
                await SaveChangesAsync(_context);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously updates an entity in the database that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="entity">The entity with updated values. Must not be null.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the updated entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the entity to update is not found or the provided entity is null.
        /// </exception>
        /// <exception cref="Exception">Thrown when an error occurs during the update process.</exception>
        public async Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity)
        {
            try
            {
                var entityToUpdateTmp = await FilterAsNoTrackingAsync(predicate);
                TEntity entityToUpdate = (TEntity)(entityToUpdateTmp.FirstOrDefault() ?? throw new ArgumentNullException(nameof(entityToUpdate)));
                entityToUpdate = entity ?? throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                _context.Entry(entity).State = EntityState.Modified;
                await SaveChangesAsync(_context);
                return await Task.FromResult(entityToUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously deletes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted. Must not be null.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deleted entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the deletion process.</exception>
        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(NullEntity, nameof(NullEntity));
                }

                _context.Set<TEntity>().Remove(entity);
                await SaveChangesAsync(_context);
                return await Task.FromResult(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        #endregion

        /// <summary>
        /// Saves changes made to the database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the save process.</exception>
        public int SaveChanges(DbContext context)
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Asynchronously saves changes made to the database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the save process.</exception>
        public async Task<int> SaveChangesAsync(DbContext context)
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
