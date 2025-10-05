using ChanchitoTools.CommandManagement.Abstractions;
using ChanchitoTools.CommandManagement.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChanchitoTools.CommandManagement.Commands
{
    /// <summary>
    /// Base class for commands that provides common functionality and logging
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Gets the command name (used in command-line arguments)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the command description
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Executes the management command
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency resolution</param>
        /// <param name="args">Additional command arguments</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Exit code (0 for success, non-zero for failure)</returns>
        public async Task<int> ExecuteAsync(IServiceProvider serviceProvider, string[] args, CancellationToken cancellationToken = default)
        {
            var logger = GetLogger(serviceProvider);

            try
            {
                logger?.LogInformation(string.Format(CommandConstants.StartingCommand, Name));

                var result = await ExecuteInternalAsync(serviceProvider, args, cancellationToken);

                if (result == 0)
                {
                    logger?.LogInformation(string.Format(CommandConstants.CommandCompletedSuccessfully, Name));
                }
                else
                {
                    logger?.LogWarning(string.Format(CommandConstants.CommandCompletedWithExitCode, Name, result));
                }

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, string.Format(CommandConstants.CommandFailed, Name));
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1; // Failure
            }
        }

        /// <summary>
        /// Internal implementation of the command execution
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency resolution</param>
        /// <param name="args">Additional command arguments</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Exit code (0 for success, non-zero for failure)</returns>
        protected abstract Task<int> ExecuteInternalAsync(IServiceProvider serviceProvider, string[] args, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a logger for the command
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The logger instance</returns>
        protected virtual ILogger? GetLogger(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(GetType());
        }

        /// <summary>
        /// Checks if the current environment is production
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>True if in production environment</returns>
        protected virtual bool IsProductionEnvironment(IServiceProvider serviceProvider)
        {
            var environment = serviceProvider.GetService<IHostEnvironment>();
            return environment?.IsProduction() == true;
        }

        /// <summary>
        /// Checks if the current environment is development
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>True if in development environment</returns>
        protected virtual bool IsDevelopmentEnvironment(IServiceProvider serviceProvider)
        {
            var environment = serviceProvider.GetService<IHostEnvironment>();
            return environment?.IsDevelopment() == true;
        }
    }
}
