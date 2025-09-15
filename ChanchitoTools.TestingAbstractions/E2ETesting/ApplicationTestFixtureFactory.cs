using Microsoft.EntityFrameworkCore;

namespace ChanchitoBackend.System.Abstractions.E2ETesting
{
    /// <summary>
    /// Factory for creating application test fixtures that use the real application service configuration
    /// </summary>
    public static class ApplicationTestFixtureFactory
    {
        /// <summary>
        /// Creates an in-memory database application test fixture
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type</typeparam>
        /// <param name="testBuilder">The test builder that knows how to configure the specific application</param>
        /// <param name="databaseName">The database name</param>
        /// <returns>An application test fixture</returns>
        public static IApplicationTestFixture<TDbContext> CreateInMemory<TDbContext>(
            IApplicationTestBuilder testBuilder,
            string? databaseName = null)
            where TDbContext : DbContext
        {
            var dbName = databaseName ?? $"test_db_{Guid.NewGuid():N}";
            
            return new ApplicationTestFixture<TDbContext>(testBuilder, dbName, useInMemoryDatabase: true);
        }

        /// <summary>
        /// Creates a SQLite database application test fixture
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type</typeparam>
        /// <param name="testBuilder">The test builder that knows how to configure the specific application</param>
        /// <param name="databaseName">The database name</param>
        /// <returns>An application test fixture</returns>
        public static IApplicationTestFixture<TDbContext> CreateSqlite<TDbContext>(
            IApplicationTestBuilder testBuilder,
            string? databaseName = null)
            where TDbContext : DbContext
        {
            var dbName = databaseName ?? $"test_db_{Guid.NewGuid():N}";
            
            return new ApplicationTestFixture<TDbContext>(testBuilder, dbName, useInMemoryDatabase: false);
        }

        /// <summary>
        /// Creates an application test fixture with custom configuration
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type</typeparam>
        /// <param name="testBuilder">The test builder to use</param>
        /// <param name="databaseName">The database name</param>
        /// <param name="useInMemoryDatabase">Whether to use in-memory database</param>
        /// <returns>An application test fixture</returns>
        public static IApplicationTestFixture<TDbContext> Create<TDbContext>(
            IApplicationTestBuilder testBuilder,
            string? databaseName = null,
            bool useInMemoryDatabase = true)
            where TDbContext : DbContext
        {
            var dbName = databaseName ?? $"test_db_{Guid.NewGuid():N}";
            
            return new ApplicationTestFixture<TDbContext>(testBuilder, dbName, useInMemoryDatabase);
        }
    }
}
