using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChanchitoBackend.System.Abstractions.E2ETesting
{
    /// <summary>
    /// Abstract base implementation of IApplicationTestBuilder that provides the black box approach
    /// Concrete implementations in the API layer will provide the actual application startup logic
    /// This ensures the System.Abstractions library doesn't need to know about specific application details
    /// </summary>
    public abstract class ApplicationTestBuilder : IApplicationTestBuilder
    {
        /// <summary>
        /// Creates a test host that mimics the real application startup process
        /// This must be implemented by the concrete class that knows about the specific application
        /// This is the black box approach - we don't manually configure services here
        /// </summary>
        /// <param name="testDatabaseConnectionString">Optional test database connection string to override</param>
        /// <returns>A configured host that represents the full application</returns>
        public abstract IHost CreateTestHost(string? testDatabaseConnectionString = null);

        /// <summary>
        /// Creates a service provider from the test host
        /// </summary>
        /// <param name="testDatabaseConnectionString">Optional test database connection string</param>
        /// <returns>A configured service provider</returns>
        public virtual IServiceProvider CreateServiceProvider(string? testDatabaseConnectionString = null)
        {
            var host = CreateTestHost(testDatabaseConnectionString);
            return host.Services;
        }

        /// <summary>
        /// Creates a test configuration with appropriate settings for testing
        /// This can be used by concrete implementations to build their test configuration
        /// </summary>
        /// <param name="testDatabaseConnectionString">The test database connection string</param>
        /// <returns>A configuration instance for testing</returns>
        protected virtual IConfiguration CreateTestConfiguration(string? testDatabaseConnectionString = null)
        {
            var configurationBuilder = new ConfigurationBuilder();

            // Add default configuration values
            var defaultSettings = new Dictionary<string, string?>
            {
                ["Logging:LogLevel:Default"] = "Debug",
                ["Logging:LogLevel:Microsoft"] = "Warning",
                ["Logging:LogLevel:Microsoft.Hosting.Lifetime"] = "Information",
                ["Environment"] = "Testing"
            };

            // Add test database connection string if provided
            if (!string.IsNullOrEmpty(testDatabaseConnectionString))
            {
                defaultSettings["ConnectionStrings:DefaultConnection"] = testDatabaseConnectionString;
            }

            configurationBuilder.AddInMemoryCollection(defaultSettings);

            // Add environment variables and other configuration sources if needed
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }
}