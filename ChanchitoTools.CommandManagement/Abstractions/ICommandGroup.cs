namespace ChanchitoTools.CommandManagement.Abstractions
{
    /// <summary>
    /// Interface for command groups that can execute multiple commands in sequence
    /// </summary>
    public interface ICommandGroup
    {
        /// <summary>
        /// Gets the group name (used in command-line arguments)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the group description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the commands to execute in order
        /// </summary>
        IEnumerable<string> CommandNames { get; }

        /// <summary>
        /// Gets the execution order priority (lower numbers execute first)
        /// </summary>
        int Priority { get; }
    }
}
