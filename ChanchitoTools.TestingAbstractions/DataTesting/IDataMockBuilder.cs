using Moq;

namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Interface for building mocks for data layer testing
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type</typeparam>
    public interface IDataMockBuilder<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the mocked DbContext
        /// </summary>
        Mock<TDbContext> MockDbContext { get; }

        /// <summary>
        /// Gets the mocked service provider
        /// </summary>
        Mock<IServiceProvider> MockServiceProvider { get; }

        /// <summary>
        /// Mocks a DbSet for the specified entity type
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="data">The data to include in the mock</param>
        /// <returns>The mock DbSet</returns>
        Mock<DbSet<TEntity>> MockDbSet<TEntity>(IEnumerable<TEntity>? data = null) where TEntity : class;

        /// <summary>
        /// Mocks a DbSet for the specified entity type with async support
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="data">The data to include in the mock</param>
        /// <returns>The mock DbSet</returns>
        Mock<DbSet<TEntity>> MockDbSetAsync<TEntity>(IEnumerable<TEntity>? data = null) where TEntity : class;

        /// <summary>
        /// Mocks a service
        /// </summary>
        /// <typeparam name="TService">The service type</typeparam>
        /// <returns>The mock service</returns>
        Mock<TService> MockService<TService>() where TService : class;

        /// <summary>
        /// Builds the complete mock setup
        /// </summary>
        /// <returns>The configured mock DbContext</returns>
        Mock<TDbContext> Build();
    }
}