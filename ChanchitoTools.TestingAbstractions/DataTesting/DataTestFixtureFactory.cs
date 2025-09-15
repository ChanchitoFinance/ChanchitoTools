using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Factory for creating data test fixtures with different configurations
    /// </summary>
    public static class DataTestFixtureFactory
    {
        /// <summary>
        /// Creates an in-memory database test fixture using the real application data layer configuration
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type</typeparam>
        /// <param name="dataTestBuilder">The data test builder that knows how to configure the data layer</param>
        /// <param name="databaseName">The database name</param>
        /// <returns>A data test fixture</returns>
        public static IDataTestFixture<TDbContext> CreateInMemory<TDbContext>(
            IDataTestBuilder dataTestBuilder,
            string databaseName = "TestDb")
            where TDbContext : DbContext
        {
            var connectionString = $"Data Source={databaseName};Mode=Memory;Cache=Shared";
            var serviceProvider = dataTestBuilder.CreateDataServiceProvider(connectionString);
            var dbContext = serviceProvider.GetRequiredService<TDbContext>();
            
            var configuration = DataTestConfiguration.CreateInMemory(databaseName);
            var fixture = new DataTestFixture<TDbContext>(serviceProvider, dbContext, configuration);
            fixture.Initialize(configuration);

            return fixture;
        }

        /// <summary>
        /// Creates a SQLite database test fixture using the real application data layer configuration
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type</typeparam>
        /// <param name="dataTestBuilder">The data test builder that knows how to configure the data layer</param>
        /// <param name="connectionString">The connection string</param>
        /// <returns>A data test fixture</returns>
        public static IDataTestFixture<TDbContext> CreateSqlite<TDbContext>(
            IDataTestBuilder dataTestBuilder,
            string connectionString = "Data Source=TestDb.db")
            where TDbContext : DbContext
        {
            var serviceProvider = dataTestBuilder.CreateDataServiceProvider(connectionString);
            var dbContext = serviceProvider.GetRequiredService<TDbContext>();
            
            var configuration = DataTestConfiguration.CreateSqlite(connectionString);
            var fixture = new DataTestFixture<TDbContext>(serviceProvider, dbContext, configuration);
            fixture.Initialize(configuration);

            return fixture;
        }

        /// <summary>
        /// Creates a SQL Server database test fixture using the real application data layer configuration
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type</typeparam>
        /// <param name="dataTestBuilder">The data test builder that knows how to configure the data layer</param>
        /// <param name="connectionString">The connection string</param>
        /// <returns>A data test fixture</returns>
        public static IDataTestFixture<TDbContext> CreateSqlServer<TDbContext>(
            IDataTestBuilder dataTestBuilder,
            string connectionString)
            where TDbContext : DbContext
        {
            var serviceProvider = dataTestBuilder.CreateDataServiceProvider(connectionString);
            var dbContext = serviceProvider.GetRequiredService<TDbContext>();
            
            var configuration = DataTestConfiguration.CreateSqlServer(connectionString);
            var fixture = new DataTestFixture<TDbContext>(serviceProvider, dbContext, configuration);
            fixture.Initialize(configuration);

            return fixture;
        }
    }
}