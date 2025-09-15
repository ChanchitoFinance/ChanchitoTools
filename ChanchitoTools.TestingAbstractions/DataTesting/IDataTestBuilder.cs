using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Interface for building data test environments that use the real application service configuration
    /// This treats application initialization as a black box - the concrete implementation
    /// knows how to configure the data layer properly
    /// </summary>
    public interface IDataTestBuilder
    {
        /// <summary>
        /// Creates a service provider with the full data layer configuration
        /// This is the black box approach - we don't manually configure services,
        /// instead we let the application configure its data layer as it would in production
        /// </summary>
        /// <param name="testDatabaseConnectionString">The test database connection string</param>
        /// <returns>A configured service provider with the data layer</returns>
        IServiceProvider CreateDataServiceProvider(string testDatabaseConnectionString);
    }
}
