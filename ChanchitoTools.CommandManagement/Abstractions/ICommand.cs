namespace ChanchitoTools.CommandManagement.Abstractions
{
    /// <summary>
    /// Interface for management commands that can be executed via command-line arguments
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the command name (used in command-line arguments)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the command description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Executes the management command
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency resolution</param>
        /// <param name="args">Additional command arguments</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Exit code (0 for success, non-zero for failure)</returns>
        Task<int> ExecuteAsync(IServiceProvider serviceProvider, string[] args, CancellationToken cancellationToken = default);
    }
}
