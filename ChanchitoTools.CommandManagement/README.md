# ChanchitoTools.CommandManagement

A simple, reusable command management system for .NET applications with dependency injection support.

## Features

- **Simple API**: Minimal setup with just `AddCommand()` and `AddCommandRunner()`
- **Multiple Commands**: Run multiple commands in a single line with execution order
- **Command Groups**: Define groups of commands that execute together
- **Dependency Injection**: Full integration with Microsoft.Extensions.DependencyInjection
- **Built-in Logging**: Automatic logging for all commands
- **Environment Safety**: Built-in production/development environment checks
- **Base Command Class**: Common functionality with `BaseCommand`
- **WebApplication Integration**: One-line integration with ASP.NET Core
- **Constants-based**: All strings stored in constants for easy customization

## Installation

```xml
<PackageReference Include="ChanchitoTools.CommandManagement" Version="1.0.0" />
```

## Quick Start

### 1. Register Commands and Runner

```csharp
// In Program.cs or Startup.cs
builder.Services
    .AddCommand<MyCustomCommand>()
    .AddCommandGroup<SetupCommandGroup>()
    .AddCommandRunner("My Application");
```

### 2. Execute Commands in Application Startup

```csharp
var app = builder.Build();

// Check for commands BEFORE configuring middleware
if (await app.RunCommandIfPresentAsync(args))
{
    return; // Command was executed, exit
}

// Continue with normal application startup
app.MapControllers();
app.Run();
```

### 3. Create Custom Commands

```csharp
public class MyCustomCommand : BaseCommand
{
    public override string Name => "mycommand";
    public override string Description => "Does something useful";

    protected override async Task<int> ExecuteInternalAsync(
        IServiceProvider serviceProvider, 
        string[] args, 
        CancellationToken cancellationToken)
    {
        // Your command logic here
        Console.WriteLine("Executing my custom command!");
        return 0; // Success
    }
}
```

## Advanced Usage

### Register Multiple Commands and Groups

```csharp
builder.Services
    .AddCommand<SeedCommand>()
    .AddCommand<MigrateCommand>()
    .AddCommand<ClearCommand>()
    .AddCommandGroup<SetupCommandGroup>()
    .AddCommandGroup<ResetCommandGroup>()
    .AddCommandRunner("My Application");
```

### Create Command Groups

```csharp
public class SetupCommandGroup : BaseCommandGroup
{
    public override string Name => "setup";
    public override string Description => "Complete application setup";
    public override int Priority => 10;

    public override IEnumerable<string> CommandNames => new[]
    {
        "migrate",
        "seed"
    };
}
```

### Environment-Safe Commands

```csharp
public class DestructiveCommand : BaseCommand
{
    public override string Name => "destructive";
    public override string Description => "Destructive operation (dev only)";

    protected override async Task<int> ExecuteInternalAsync(
        IServiceProvider serviceProvider, 
        string[] args, 
        CancellationToken cancellationToken)
    {
        // Safety check: only allow in development
        if (IsProductionEnvironment(serviceProvider))
        {
            Console.Error.WriteLine("Error: This command is not allowed in production");
            return 1;
        }

        // Perform destructive operation
        // ...
        return 0;
    }
}
```

### Using with Console Applications

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddCommand<MyCommand>();
        services.AddCommandRunner("My Console App");
    })
    .Build();

// Execute commands
var runner = host.Services.GetRequiredService<CommandRunner>();
var exitCode = await runner.RunAsync(args, host.Services);
Environment.Exit(exitCode);
```

## Command Line Usage

```bash
# Execute a single command
dotnet run -- --mycommand

# Execute multiple commands in sequence
dotnet run -- --migrate --seed

# Execute with arguments
dotnet run -- --mycommand arg1 arg2

# Execute command group
dotnet run -- --setup

# Mix commands and groups
dotnet run -- --clear --setup

# Show help and then run another command
dotnet run -- --help --info

# Mix individual commands, groups, and help
dotnet run -- --help --status --migrate

# Show help
dotnet run -- --help
```

### Example Output

Here's what the command output looks like when running multiple commands:

**Running `--help --info`:**

![Help and Info Command Output](Documentation/Images/image.png)

**Running `--migrate --help --info`:**

![Multiple Commands Output](Documentation/Images/image%20copy.png)

## Architecture

The library follows a simple, clean architecture:

- **Abstractions**: Core interfaces (`ICommand`, `ICommandGroup`)
- **Core**: Command execution engine (`CommandRunner`, `CommandExecution`)
- **Commands**: Base implementations (`BaseCommand`)
- **Extensions**: DI integration methods
- **Constants**: All printable strings centralized

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

MIT License - see LICENSE file for details.
