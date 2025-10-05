namespace ChanchitoTools.CommandManagement.Core
{
    /// <summary>
    /// Represents a command to be executed with its arguments
    /// </summary>
    internal class CommandExecution
    {
        /// <summary>
        /// Gets the command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the command arguments
        /// </summary>
        public string[] Arguments { get; }

        /// <summary>
        /// Initializes a new instance of the CommandExecution class
        /// </summary>
        /// <param name="name">The command name</param>
        /// <param name="arguments">The command arguments</param>
        public CommandExecution(string name, string[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
