using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChanchitoBackend.System.Abstractions.E2ETesting
{
    /// <summary>
    /// Interface for building test applications that mimics the real application startup process
    /// This treats application initialization as a black box - the concrete implementation
    /// knows how to start up the application properly
    /// </summary>
    public interface IApplicationTestBuilder
    {
        /// <summary>
        /// Creates a test host that mimics the real application startup process
        /// This is the black box approach - we don't manually configure services,
        /// instead we let the application configure itself as it would in production
        /// </summary>
        /// <param name="testDatabaseConnectionString">Optional test database connection string to override</param>
        /// <returns>A configured host that represents the full application</returns>
        IHost CreateTestHost(string? testDatabaseConnectionString = null);

        /// <summary>
        /// Creates a service provider from the test host
        /// </summary>
        /// <param name="testDatabaseConnectionString">Optional test database connection string</param>
        /// <returns>A configured service provider</returns>
        IServiceProvider CreateServiceProvider(string? testDatabaseConnectionString = null);
    }
}
