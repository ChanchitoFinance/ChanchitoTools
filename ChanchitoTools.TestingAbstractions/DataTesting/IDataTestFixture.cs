namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Interface for data test fixtures that provide comprehensive testing patterns for data layers
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type</typeparam>
    public interface IDataTestFixture<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the service provider for dependency injection
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the DbContext instance
        /// </summary>
        TDbContext DbContext { get; }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the database provider type
        /// </summary>
        DatabaseProvider Provider { get; }

        /// <summary>
        /// Initializes the test fixture with the specified configuration
        /// </summary>
        /// <param name="configuration">The test configuration</param>
        void Initialize(DataTestConfiguration configuration);

        /// <summary>
        /// Seeds the database with test data
        /// </summary>
        /// <param name="seedData">The seed data to use</param>
        void SeedDatabase(ISeedData seedData);

        /// <summary>
        /// Clears all data from the database
        /// </summary>
        void ClearDatabase();

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>The number of affected entities</returns>
        int SaveChanges();

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        /// <returns>The database transaction</returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Gets a service from the DI container
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// Gets a required service from the DI container
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        T GetRequiredService<T>() where T : class;

        /// <summary>
        /// Creates a new scope for testing
        /// </summary>
        /// <returns>A new service scope</returns>
        IServiceScope CreateScope();
    }
}