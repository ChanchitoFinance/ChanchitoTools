namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Implementation of data test fixture that provides comprehensive testing patterns for data layers
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type</typeparam>
    public class DataTestFixture<TDbContext> : IDataTestFixture<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TDbContext _dbContext;
        private readonly DataTestConfiguration _configuration;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTestFixture{TDbContext}"/> class
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="dbContext">The database context</param>
        /// <param name="configuration">The test configuration</param>
        public DataTestFixture(IServiceProvider serviceProvider, TDbContext dbContext, DataTestConfiguration configuration)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            ServiceProvider = serviceProvider;
            DbContext = dbContext;
            ConnectionString = configuration.ConnectionString;
            Provider = configuration.Provider;
        }

        /// <summary>
        /// Gets the service provider for dependency injection
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the DbContext instance
        /// </summary>
        public TDbContext DbContext { get; }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Gets the database provider type
        /// </summary>
        public DatabaseProvider Provider { get; }

        /// <summary>
        /// Initializes the test fixture with the specified configuration
        /// </summary>
        /// <param name="configuration">The test configuration</param>
        public void Initialize(DataTestConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Ensure database is created with proper schema
            try
            {
                // Drop any existing database to start fresh
                DbContext.Database.EnsureDeleted();

                // Create the database with schema
                DbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DataTestFixture] Database initialization failed: {ex.Message}");
                Console.WriteLine($"[DataTestFixture] Exception details: {ex}");

                // Try a more aggressive approach - recreate the database
                try
                {
                    DbContext.Database.EnsureDeleted();
                    DbContext.Database.EnsureCreated();
                }
                catch (Exception recreateEx)
                {
                    Console.WriteLine($"[DataTestFixture] Database recreation also failed: {recreateEx.Message}");
                    throw new InvalidOperationException("Failed to create database schema for testing", recreateEx);
                }
            }

            // Clear existing data
            ClearDatabase();

            // Auto-seed if configured
            if (configuration.AutoSeedDatabase)
            {
                // This would typically use a default seed data implementation
                // For now, we'll leave it empty and let tests provide their own seed data
            }
        }

        /// <summary>
        /// Seeds the database with test data
        /// </summary>
        /// <param name="seedData">The seed data to use</param>
        public void SeedDatabase(ISeedData seedData)
        {
            if (seedData == null)
                throw new ArgumentNullException(nameof(seedData));

            seedData.Seed(DbContext);
            SaveChanges();
        }

        /// <summary>
        /// Clears all data from the database
        /// </summary>
        public void ClearDatabase()
        {
            // Get all entity types
            var entityTypes = DbContext.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var entityName = entityType.Name;
                var dbSetType = typeof(DbSet<>).MakeGenericType(entityType.ClrType);
                var dbSet = DbContext.GetType().GetProperty(entityName)?.GetValue(DbContext);

                if (dbSet != null)
                {
                    var removeRangeMethod = dbSetType.GetMethod("RemoveRange");
                    var toListMethod = dbSetType.GetMethod("ToList");

                    if (removeRangeMethod != null && toListMethod != null)
                    {
                        var entities = toListMethod.Invoke(dbSet, null);
                        if (entities != null)
                        {
                            removeRangeMethod.Invoke(dbSet, new[] { entities });
                        }
                    }
                }
            }

            SaveChanges();
        }

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>The number of affected entities</returns>
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        /// <returns>The database transaction</returns>
        public IDbContextTransaction BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already active");
            }

            _currentTransaction = DbContext.Database.BeginTransaction();
            return _currentTransaction;
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction to rollback");
            }

            _currentTransaction.Rollback();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction to commit");
            }

            _currentTransaction.Commit();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        /// <summary>
        /// Gets a service from the DI container
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        public T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Gets a required service from the DI container
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        public T GetRequiredService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Creates a new scope for testing
        /// </summary>
        /// <returns>A new service scope</returns>
        public IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }

        /// <summary>
        /// Disposes the test fixture
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the test fixture
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _currentTransaction?.Dispose();
                _dbContext?.Dispose();
                _disposed = true;
            }
        }
    }
}