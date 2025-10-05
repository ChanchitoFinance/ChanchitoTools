using ChanchitoTools.CommandManagement.Abstractions;

namespace ChanchitoTools.CommandManagement.Commands
{
    /// <summary>
    /// Base class for command groups that provides common functionality
    /// </summary>
    public abstract class BaseCommandGroup : ICommandGroup
    {
        /// <summary>
        /// Gets the group name (used in command-line arguments)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the group description
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the commands to execute in order
        /// </summary>
        public abstract IEnumerable<string> CommandNames { get; }

        /// <summary>
        /// Gets the execution order priority (lower numbers execute first)
        /// </summary>
        public virtual int Priority => 100;
    }
}
