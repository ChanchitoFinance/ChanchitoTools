using ChanchitoTools.CommandManagement.Abstractions;
using ChanchitoTools.CommandManagement.Constants;
using ChanchitoTools.CommandManagement.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChanchitoTools.CommandManagement.Extensions
{
    /// <summary>
    /// Extension methods for registering and managing commands
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Registers a command with the service collection
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCommand<TCommand>(this IServiceCollection services)
            where TCommand : class, ICommand
        {
            services.AddSingleton<ICommand, TCommand>();
            return services;
        }

        /// <summary>
        /// Registers a command group with the service collection
        /// </summary>
        /// <typeparam name="TCommandGroup">The command group type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCommandGroup<TCommandGroup>(this IServiceCollection services)
            where TCommandGroup : class, ICommandGroup
        {
            services.AddSingleton<ICommandGroup, TCommandGroup>();
            return services;
        }

        /// <summary>
        /// Registers the command runner with the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="applicationName">Optional application name for usage display</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCommandRunner(this IServiceCollection services, string? applicationName = null)
        {
            services.AddSingleton<CommandRunner>(provider =>
            {
                var commands = provider.GetServices<ICommand>();
                var commandGroups = provider.GetServices<ICommandGroup>();
                var logger = provider.GetService<ILogger<CommandRunner>>();
                return new CommandRunner(commands, commandGroups, logger, applicationName);
            });
            return services;
        }

        /// <summary>
        /// Checks if the application should run a command instead of starting normally
        /// If a command is detected, it will be executed and the application will exit
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="args">Command-line arguments</param>
        /// <returns>True if a command was executed, false otherwise</returns>
        public static async Task<bool> RunCommandIfPresentAsync(this IServiceProvider serviceProvider, string[] args)
        {
            // Check if there's a command in the arguments
            if (!CommandRunner.HasCommand(args))
            {
                return false; // No command, continue with normal startup
            }

            // Get the command runner from DI
            var runner = serviceProvider.GetService<CommandRunner>();
            if (runner == null)
            {
                Console.Error.WriteLine(CommandConstants.CommandRunnerNotRegistered);
                Environment.Exit(1);
                return true;
            }

            // Execute the command
            var exitCode = await runner.RunAsync(args, serviceProvider, CancellationToken.None);

            // Exit the application with the command's exit code
            Environment.Exit(exitCode);
            return true; // This line won't be reached, but satisfies the return type
        }
    }
}
