using ChanchitoTools.CommandManagement.Abstractions;
using ChanchitoTools.CommandManagement.Constants;
using Microsoft.Extensions.Logging;

namespace ChanchitoTools.CommandManagement.Core
{
    /// <summary>
    /// Executes management commands based on command-line arguments
    /// </summary>
    public class CommandRunner
    {
        private readonly Dictionary<string, ICommand> _commands;
        private readonly Dictionary<string, ICommandGroup> _commandGroups;
        private readonly ILogger<CommandRunner>? _logger;
        private readonly string _applicationName;

        public CommandRunner(IEnumerable<ICommand> commands, IEnumerable<ICommandGroup>? commandGroups = null, ILogger<CommandRunner>? logger = null, string? applicationName = null)
        {
            _commands = commands.ToDictionary(c => c.Name, c => c, StringComparer.OrdinalIgnoreCase);
            _commandGroups = commandGroups?.ToDictionary(cg => cg.Name, cg => cg, StringComparer.OrdinalIgnoreCase) ?? new Dictionary<string, ICommandGroup>();
            _logger = logger;
            _applicationName = applicationName ?? CommandConstants.DefaultApplicationName;
        }

        /// <summary>
        /// Checks if command-line arguments contain a management command
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        /// <returns>True if a management command is present</returns>
        public static bool HasCommand(string[] args)
        {
            return args.Length > 0 && 
                   args[0].StartsWith("--") && 
                   args[0].Length > 2;
        }

        /// <summary>
        /// Parses and executes management commands from command-line arguments
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        /// <param name="serviceProvider">Service provider for dependency resolution</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Exit code (0 for success, non-zero for failure)</returns>
        public async Task<int> RunAsync(string[] args, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return 1;
            }

            // Parse commands from arguments
            var commandsToExecute = ParseCommands(args);

            // If only help command is requested, show help and return
            if (commandsToExecute.Count == 1 && commandsToExecute[0].Name.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                PrintUsage();
                return 0;
            }

            // Execute commands in sequence (including help if it's part of multiple commands)
            return await ExecuteCommandsAsync(commandsToExecute, serviceProvider, cancellationToken);
        }

        /// <summary>
        /// Parses command-line arguments into a list of commands to execute
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        /// <returns>List of commands with their arguments</returns>
        private List<CommandExecution> ParseCommands(string[] args)
        {
            var commands = new List<CommandExecution>();
            var currentCommandName = "";
            var currentArgs = new List<string>();

            foreach (var arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    // Save previous command if exists
                    if (!string.IsNullOrEmpty(currentCommandName))
                    {
                        commands.Add(new CommandExecution(currentCommandName, currentArgs.ToArray()));
                    }

                    // Start new command
                    currentCommandName = arg.TrimStart('-');
                    currentArgs.Clear();
                }
                else
                {
                    // Add argument to current command
                    currentArgs.Add(arg);
                }
            }

            // Add the last command
            if (!string.IsNullOrEmpty(currentCommandName))
            {
                commands.Add(new CommandExecution(currentCommandName, currentArgs.ToArray()));
            }

            return commands;
        }

        /// <summary>
        /// Executes a list of commands in sequence
        /// </summary>
        /// <param name="commands">Commands to execute</param>
        /// <param name="serviceProvider">Service provider for dependency resolution</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Exit code (0 for success, non-zero for failure)</returns>
        private async Task<int> ExecuteCommandsAsync(List<CommandExecution> commands, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var allCommandsToExecute = new List<CommandExecution>();

            // Expand command groups and resolve individual commands
            foreach (var command in commands)
            {
                if (_commandGroups.TryGetValue(command.Name, out var commandGroup))
                {
                    // Expand command group
                    _logger?.LogInformation("Expanding command group: {CommandGroupName}", command.Name);
                    foreach (var cmdName in commandGroup.CommandNames)
                    {
                        allCommandsToExecute.Add(new CommandExecution(cmdName, command.Arguments));
                    }
                }
                else if (command.Name.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle help command inline
                    PrintUsage();
                    Console.WriteLine(); // Add spacing after help
                }
                else if (_commands.ContainsKey(command.Name))
                {
                    // Individual command
                    allCommandsToExecute.Add(command);
                }
                else
                {
                    // Unknown command
                    Console.Error.WriteLine(string.Format(CommandConstants.UnknownCommandError, command.Name));
                    Console.Error.WriteLine();
                    PrintUsage();
                    return 1;
                }
            }

            // Execute commands in order
            foreach (var command in allCommandsToExecute)
            {
                if (_commands.TryGetValue(command.Name, out var cmd))
                {
                    _logger?.LogInformation("Executing command: {CommandName}", command.Name);
                    var result = await cmd.ExecuteAsync(serviceProvider, command.Arguments, cancellationToken);
                    
                    if (result != 0)
                    {
                        _logger?.LogError("Command {CommandName} failed with exit code {ExitCode}", command.Name, result);
                        return result;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Prints usage information for all available commands
        /// </summary>
        private void PrintUsage()
        {
            Console.WriteLine(CommandConstants.HeaderSeparator);
            Console.WriteLine($"           {_applicationName} - {CommandConstants.HeaderTitle}");
            Console.WriteLine(CommandConstants.HeaderSeparator);
            Console.WriteLine();
            Console.WriteLine(CommandConstants.UsageTitle);
            Console.WriteLine("           dotnet run -- --command1 [args] --command2 [args] ...");
            Console.WriteLine();
            Console.WriteLine(CommandConstants.AvailableCommandsTitle);
            Console.WriteLine(CommandConstants.CommandsSeparator);

            // Show individual commands
            foreach (var command in _commands.Values.OrderBy(c => c.Name))
            {
                Console.WriteLine(string.Format(CommandConstants.CommandFormat, command.Name, command.Description));
            }

            // Show command groups
            if (_commandGroups.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Command Groups:");
                Console.WriteLine("---------------");
                foreach (var group in _commandGroups.Values.OrderBy(g => g.Priority).ThenBy(g => g.Name))
                {
                    var commands = string.Join(", ", group.CommandNames);
                    Console.WriteLine(string.Format(CommandConstants.CommandFormat, group.Name, $"{group.Description} (executes: {commands})"));
                }
            }

            Console.WriteLine();
            Console.WriteLine(CommandConstants.ExamplesTitle);
            Console.WriteLine(CommandConstants.ExamplesSeparator);
            Console.WriteLine("  dotnet run -- --help                    # Show this help message");
            Console.WriteLine("  dotnet run -- --migrate                 # Run database migrations");
            Console.WriteLine("  dotnet run -- --migrate --seed          # Run migrations then seed");
            Console.WriteLine("  dotnet run -- --setup                   # Run setup group (if defined)");
            Console.WriteLine();
            Console.WriteLine(CommandConstants.HeaderSeparator);
        }

        /// <summary>
        /// Gets all registered commands
        /// </summary>
        public IEnumerable<ICommand> GetCommands() => _commands.Values;

        /// <summary>
        /// Gets all registered command groups
        /// </summary>
        public IEnumerable<ICommandGroup> GetCommandGroups() => _commandGroups.Values;
    }
}
