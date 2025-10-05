namespace ChanchitoTools.CommandManagement.Constants
{
    /// <summary>
    /// Constants for command management system messages and formatting
    /// </summary>
    public static class CommandConstants
    {
        // Header and footer
        public const string HeaderSeparator = "==========================================================";
        public const string HeaderTitle = "Management Commands";
        public const string UsageTitle = "Usage: dotnet run -- --<command> [arguments]";
        public const string AvailableCommandsTitle = "Available Commands:";
        public const string CommandsSeparator = "-------------------";
        public const string ExamplesTitle = "Examples:";
        public const string ExamplesSeparator = "---------";
        public const string HelpCommandExample = "  dotnet run -- --help                    # Show this help message";

        // Error messages
        public const string UnknownCommandError = "Unknown command: {0}";
        public const string CommandRunnerNotRegistered = "Error: Command runner is not registered. Call AddCommandRunner() in service configuration.";
        public const string NoCommandArguments = "No command arguments provided";

        // Success/Info messages
        public const string ExecutingCommand = "Executing management command: {0}";
        public const string CommandCompletedSuccessfully = "=== {0} Command Completed Successfully ===";
        public const string CommandCompletedWithExitCode = "=== {0} Command Completed with Exit Code {1} ===";
        public const string CommandFailed = "=== {0} Command Failed ===";
        public const string StartingCommand = "=== Starting {0} Command ===";

        // Format strings
        public const string CommandFormat = "  --{0,-15} {1}";
        public const string ExampleFormat = "  dotnet run -- --{0,-15} # {1}";

        // Default values
        public const string DefaultApplicationName = "Application";
    }
}
