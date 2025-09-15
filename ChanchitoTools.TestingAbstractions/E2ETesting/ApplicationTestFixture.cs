using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChanchitoBackend.System.Abstractions.E2ETesting
{
    /// <summary>
    /// Application test fixture that provides access to the full application context
    /// </summary>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    public class ApplicationTestFixture<TDbContext> : IApplicationTestFixture<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IApplicationTestBuilder _testBuilder;
        private readonly bool _useInMemoryDatabase;
        private IServiceProvider? _serviceProvider;
        private IServiceScope? _scope;
        private bool _disposed;

        /// <summary>
        /// Gets the service provider with the full application configuration
        /// </summary>
        public IServiceProvider ServiceProvider => _serviceProvider ?? throw new InvalidOperationException("Fixture not initialized. Call InitializeAsync first.");

        /// <summary>
        /// Gets the database context
        /// </summary>
        public TDbContext DbContext => _scope?.ServiceProvider.GetRequiredService<TDbContext>() ?? throw new InvalidOperationException("Fixture not initialized. Call InitializeAsync first.");

        /// <summary>
        /// Gets the database name used for testing
        /// </summary>
        public string DatabaseName { get; }

        /// <summary>
        /// Initializes a new instance of the ApplicationTestFixture
        /// </summary>
        /// <param name="testBuilder">The test builder to use</param>
        /// <param name="databaseName">The database name</param>
        /// <param name="useInMemoryDatabase">Whether to use in-memory database</param>
        public ApplicationTestFixture(IApplicationTestBuilder testBuilder, string databaseName, bool useInMemoryDatabase = true)
        {
            _testBuilder = testBuilder ?? throw new ArgumentNullException(nameof(testBuilder));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _useInMemoryDatabase = useInMemoryDatabase;
        }

        /// <summary>
        /// Initializes the test fixture
        /// </summary>
        /// <returns>A task representing the initialization</returns>
        public async Task InitializeAsync()
        {
            if (_serviceProvider != null)
                return; // Already initialized

            try
            {
                // Create the connection string based on database type
                var connectionString = _useInMemoryDatabase 
                    ? $"Data Source={DatabaseName};Mode=Memory;Cache=Shared"
                    : $"Data Source={DatabaseName}.db";

                // Create service provider using the test builder
                _serviceProvider = _testBuilder.CreateServiceProvider(connectionString);

                // Create a scope for the database context
                _scope = _serviceProvider.CreateScope();

                // Ensure database is created
                await EnsureDatabaseCreatedAsync();

                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogInformation("Application test fixture initialized successfully with database: {DatabaseName}", DatabaseName);
            }
            catch (Exception ex)
            {
                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogError(ex, "Failed to initialize application test fixture");
                throw;
            }
        }

        /// <summary>
        /// Cleans up the test fixture
        /// </summary>
        /// <returns>A task representing the cleanup</returns>
        public async Task DisposeAsync()
        {
            if (_disposed)
                return;

            try
            {
                // Clear database if it's a file-based database
                if (!_useInMemoryDatabase && _scope != null)
                {
                    await ClearDatabaseAsync();
                }

                _scope?.Dispose();
                
                if (_serviceProvider is IDisposable disposableProvider)
                {
                    disposableProvider.Dispose();
                }

                // Delete SQLite database file if it exists
                if (!_useInMemoryDatabase)
                {
                    var dbFile = $"{DatabaseName}.db";
                    if (File.Exists(dbFile))
                    {
                        try
                        {
                            File.Delete(dbFile);
                        }
                        catch (Exception ex)
                        {
                            var fileLogger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                            fileLogger?.LogWarning(ex, "Failed to delete test database file: {DatabaseFile}", dbFile);
                        }
                    }
                }

                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogInformation("Application test fixture disposed successfully");
            }
            catch (Exception ex)
            {
                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogError(ex, "Error during application test fixture disposal");
            }
            finally
            {
                _disposed = true;
            }
        }

        /// <summary>
        /// Creates a new service scope
        /// </summary>
        /// <returns>A service scope</returns>
        public IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }

        /// <summary>
        /// Gets a service from the service provider
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance or null if not found</returns>
        public T? GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Gets a required service from the service provider
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        public T GetRequiredService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Clears all data from the database
        /// </summary>
        /// <returns>A task representing the operation</returns>
        public async Task ClearDatabaseAsync()
        {
            if (_scope == null)
                return;

            try
            {
                var dbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
                
                // Get all entity types
                var entityTypes = dbContext.Model.GetEntityTypes();

                // Disable foreign key constraints for SQLite
                if (!_useInMemoryDatabase)
                {
                    await dbContext.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF");
                }

                // Delete all data from all tables
                foreach (var entityType in entityTypes)
                {
                    var tableName = entityType.GetTableName();
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        await dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM \"{tableName}\"");
                    }
                }

                // Re-enable foreign key constraints for SQLite
                if (!_useInMemoryDatabase)
                {
                    await dbContext.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON");
                }

                await dbContext.SaveChangesAsync();

                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogInformation("Database cleared successfully");
            }
            catch (Exception ex)
            {
                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogError(ex, "Failed to clear database");
                throw;
            }
        }

        /// <summary>
        /// Ensures the database is created and ready for testing
        /// </summary>
        /// <returns>A task representing the operation</returns>
        public async Task EnsureDatabaseCreatedAsync()
        {
            if (_scope == null)
                return;

            try
            {
                var dbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
                await dbContext.Database.EnsureCreatedAsync();

                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogInformation("Database ensured created successfully");
            }
            catch (Exception ex)
            {
                var logger = GetService<ILogger<ApplicationTestFixture<TDbContext>>>();
                logger?.LogError(ex, "Failed to ensure database created");
                throw;
            }
        }

        /// <summary>
        /// Disposes the fixture
        /// </summary>
        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
