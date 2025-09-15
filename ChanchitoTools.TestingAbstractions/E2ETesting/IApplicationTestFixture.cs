using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChanchitoBackend.System.Abstractions.E2ETesting
{
    /// <summary>
    /// Interface for application test fixtures that provide access to the full application context
    /// </summary>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    public interface IApplicationTestFixture<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the service provider with the full application configuration
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the database context
        /// </summary>
        TDbContext DbContext { get; }

        /// <summary>
        /// Gets the database name used for testing
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Initializes the test fixture
        /// </summary>
        /// <returns>A task representing the initialization</returns>
        Task InitializeAsync();

        /// <summary>
        /// Cleans up the test fixture
        /// </summary>
        /// <returns>A task representing the cleanup</returns>
        Task DisposeAsync();

        /// <summary>
        /// Creates a new service scope
        /// </summary>
        /// <returns>A service scope</returns>
        IServiceScope CreateScope();

        /// <summary>
        /// Gets a service from the service provider
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance or null if not found</returns>
        T? GetService<T>() where T : class;

        /// <summary>
        /// Gets a required service from the service provider
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        T GetRequiredService<T>() where T : class;

        /// <summary>
        /// Clears all data from the database
        /// </summary>
        /// <returns>A task representing the operation</returns>
        Task ClearDatabaseAsync();

        /// <summary>
        /// Ensures the database is created and ready for testing
        /// </summary>
        /// <returns>A task representing the operation</returns>
        Task EnsureDatabaseCreatedAsync();
    }
}
