using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Base class for data layer tests that provides comprehensive testing patterns
    /// This class is completely agnostic of specific implementations - concrete test classes
    /// must provide their own data test builder that knows how to configure the data layer
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type</typeparam>
    public abstract class DataTestBase<TDbContext> : TestBase.TestBase
        where TDbContext : DbContext
    {
        private IDataTestFixture<TDbContext>? _fixture;

        /// <summary>
        /// Gets the data test fixture
        /// </summary>
        protected IDataTestFixture<TDbContext> Fixture
        {
            get
            {
                if (_fixture == null)
                {
                    throw new InvalidOperationException("Test fixture not initialized. Call InitializeFixture() in OnSetUpAsync().");
                }
                return _fixture;
            }
        }

        /// <summary>
        /// Gets the database context
        /// </summary>
        protected TDbContext DbContext => Fixture.DbContext;

        /// <summary>
        /// Gets the service provider
        /// </summary>
        protected IServiceProvider ServiceProvider => Fixture.ServiceProvider;

        /// <summary>
        /// Initializes the test fixture using a data test builder
        /// This method must be implemented by concrete test classes to provide
        /// the appropriate data test builder for their specific application
        /// </summary>
        /// <param name="dataTestBuilder">The data test builder that knows how to configure the data layer</param>
        /// <param name="databaseName">The database name</param>
        /// <param name="useInMemoryDatabase">Whether to use in-memory database</param>
        protected void InitializeFixture(IDataTestBuilder dataTestBuilder, string databaseName = "TestDb", bool useInMemoryDatabase = true)
        {
            if (dataTestBuilder == null)
                throw new ArgumentNullException(nameof(dataTestBuilder));

            _fixture = useInMemoryDatabase
                ? DataTestFixtureFactory.CreateInMemory<TDbContext>(dataTestBuilder, databaseName)
                : DataTestFixtureFactory.CreateSqlite<TDbContext>(dataTestBuilder, $"Data Source={databaseName}.db");
        }

        /// <summary>
        /// Creates the data test builder that knows how to configure the data layer for this specific application
        /// This method must be implemented by concrete test classes
        /// </summary>
        /// <returns>A data test builder configured for the specific application</returns>
        protected abstract IDataTestBuilder CreateDataTestBuilder();

        /// <summary>
        /// Seeds the database with test data
        /// </summary>
        /// <param name="seedData">The seed data to use</param>
        protected void SeedDatabase(ISeedData seedData)
        {
            Fixture.SeedDatabase(seedData);
        }

        /// <summary>
        /// Clears all data from the database
        /// </summary>
        protected void ClearDatabase()
        {
            Fixture.ClearDatabase();
        }

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns>The number of affected entities</returns>
        protected int SaveChanges()
        {
            return Fixture.SaveChanges();
        }

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        /// <returns>The database transaction</returns>
        protected IDbContextTransaction BeginTransaction()
        {
            return Fixture.BeginTransaction();
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        protected void RollbackTransaction()
        {
            Fixture.RollbackTransaction();
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        protected void CommitTransaction()
        {
            Fixture.CommitTransaction();
        }

        /// <summary>
        /// Gets a service from the DI container
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        protected T GetService<T>() where T : class
        {
            return Fixture.GetService<T>();
        }

        /// <summary>
        /// Gets a required service from the DI container
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        protected T GetRequiredService<T>() where T : class
        {
            return Fixture.GetRequiredService<T>();
        }

        /// <summary>
        /// Creates a new service scope
        /// </summary>
        /// <returns>A service scope</returns>
        protected IServiceScope CreateScope()
        {
            return Fixture.CreateScope();
        }

        /// <summary>
        /// Asserts that an entity exists in the database
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="predicate">The predicate to match</param>
        /// <param name="message">Optional assertion message</param>
        protected void AssertEntityExists<T>(Func<T, bool> predicate, string? message = null) where T : class
        {
            var entity = DbContext.Set<T>().FirstOrDefault(predicate);
            if (entity == null)
            {
                var errorMessage = message ?? $"Entity of type {typeof(T).Name} matching the predicate was not found in the database.";
                throw new AssertionException(errorMessage);
            }
        }

        /// <summary>
        /// Asserts that an entity does not exist in the database
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="predicate">The predicate to match</param>
        /// <param name="message">Optional assertion message</param>
        protected void AssertEntityNotExists<T>(Func<T, bool> predicate, string? message = null) where T : class
        {
            var entity = DbContext.Set<T>().FirstOrDefault(predicate);
            if (entity != null)
            {
                var errorMessage = message ?? $"Entity of type {typeof(T).Name} matching the predicate was found in the database when it should not exist.";
                throw new AssertionException(errorMessage);
            }
        }

        /// <summary>
        /// Asserts that the count of entities matches the expected count
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="expectedCount">The expected count</param>
        /// <param name="message">Optional assertion message</param>
        protected void AssertEntityCount<T>(int expectedCount, string? message = null) where T : class
        {
            var actualCount = DbContext.Set<T>().Count();
            if (actualCount != expectedCount)
            {
                var errorMessage = message ?? $"Expected {expectedCount} entities of type {typeof(T).Name}, but found {actualCount}.";
                throw new AssertionException(errorMessage);
            }
        }

        /// <summary>
        /// Asserts that the count of entities matching a predicate matches the expected count
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="predicate">The predicate to match</param>
        /// <param name="expectedCount">The expected count</param>
        /// <param name="message">Optional assertion message</param>
        protected void AssertEntityCount<T>(Func<T, bool> predicate, int expectedCount, string? message = null) where T : class
        {
            var actualCount = DbContext.Set<T>().Count(predicate);
            if (actualCount != expectedCount)
            {
                var errorMessage = message ?? $"Expected {expectedCount} entities of type {typeof(T).Name} matching the predicate, but found {actualCount}.";
                throw new AssertionException(errorMessage);
            }
        }

        /// <summary>
        /// Override OnTearDownAsync to properly dispose the test fixture
        /// </summary>
        protected override async Task OnTearDownAsync()
        {
            Console.WriteLine($"[DataTestBase] OnTearDownAsync called for {GetType().Name}");

            // Dispose the test fixture
            if (_fixture != null)
            {
                Console.WriteLine($"[DataTestBase] Disposing test fixture for {GetType().Name}");
                _fixture.Dispose();
                _fixture = null;
            }

            // Call base teardown
            await base.OnTearDownAsync();
        }
    }

    /// <summary>
    /// Exception thrown when an assertion fails
    /// </summary>
    public class AssertionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class
        /// </summary>
        /// <param name="message">The exception message</param>
        public AssertionException(string message) : base(message)
        {
        }
    }
}