namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Base class for seed data implementations
    /// </summary>
    public abstract class BaseSeedData : ISeedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeedData"/> class
        /// </summary>
        /// <param name="name">The name of the seed data</param>
        /// <param name="description">The description of the seed data</param>
        /// <param name="autoApply">Whether this seed data should be applied automatically</param>
        protected BaseSeedData(string name, string description, bool autoApply = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            AutoApply = autoApply;
        }

        /// <summary>
        /// Gets the name of the seed data
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the seed data
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets whether this seed data should be applied automatically
        /// </summary>
        public bool AutoApply { get; }

        /// <summary>
        /// Seeds the database with test data
        /// </summary>
        /// <param name="context">The database context</param>
        public abstract void Seed(DbContext context);

        /// <summary>
        /// Seeds entities of a specific type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="context">The database context</param>
        /// <param name="entities">The entities to seed</param>
        protected void SeedEntities<T>(DbContext context, IEnumerable<T> entities) where T : class
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            var dbSet = context.Set<T>();
            dbSet.AddRange(entities);
        }

        /// <summary>
        /// Seeds a single entity
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="context">The database context</param>
        /// <param name="entity">The entity to seed</param>
        protected void SeedEntity<T>(DbContext context, T entity) where T : class
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var dbSet = context.Set<T>();
            dbSet.Add(entity);
        }

        /// <summary>
        /// Clears all entities of a specific type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="context">The database context</param>
        protected void ClearEntities<T>(DbContext context) where T : class
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var dbSet = context.Set<T>();
            dbSet.RemoveRange(dbSet);
        }
    }
}