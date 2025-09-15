# ChanchitoTools.TestingAbstractions

A comprehensive testing library for .NET applications that provides robust testing utilities, abstractions, and helpers to streamline your testing workflow.

## Quick Start

### Installation

```bash
# Package Manager
Install-Package ChanchitoTools.TestingAbstractions

# .NET CLI
dotnet add package ChanchitoTools.TestingAbstractions

# PackageReference
<PackageReference Include="ChanchitoTools.TestingAbstractions" Version="1.0.0" />
```

### Basic Usage

```csharp
using ChanchitoTools.TestingAbstractions;

[TestClass]
public class MyServiceTests : TestBase
{
    [TestMethod]
    public async Task CreateUser_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var userData = new CreateUserDto
        {
            Username = TestHelper.GenerateRandomString(8),
            Email = TestHelper.GenerateRandomEmail(),
            Password = "Password123!"
        };
        
        var userService = GetRequiredService<IUserService>();

        // Act
        var result = await userService.CreateUserAsync(userData);

        // Assert
        result.ShouldBeSuccess();
        result.Data!.ShouldHaveId();
        result.Data.ShouldHaveCreatedAt();
    }
}
```

## What's Included

### Core Testing Infrastructure
- **TestBase**: Abstract base class for all test classes
- **IntegrationTestBase**: Base class for integration tests with database support
- **TestContainer**: Dependency injection container for testing
- **TestDatabase**: Test database management with seeding and cleanup

### Test Data Builders
- **BaseTestEntityBuilder<T>**: Generic base class for creating test entities
- **TestHelper**: Static utility class for generating random test data
- **Builder Pattern**: Fluent API for creating complex test objects

### Custom Assertions
- **CustomAssertions**: Extension methods for domain-specific assertions
- **Fluent API**: Readable and maintainable test assertions
- **Validation Helpers**: Built-in support for validation result testing

### Database Testing
- **Multi-Provider Support**: SQL Server, SQLite, In-Memory databases
- **Test Data Seeding**: Easy setup and cleanup of test data
- **Transaction Management**: Proper test isolation and cleanup

## Features

### Comprehensive Testing Support
- Unit testing with base classes and utilities
- Integration testing with database support
- End-to-end testing capabilities
- Mock management and dependency injection

### Test Data Generation
- Random string, email, and GUID generation
- Date and numeric value generation
- Enum value selection
- Customizable data ranges and constraints

### Database Testing
- Multiple database provider support
- Automatic test database creation and cleanup
- Test data seeding and management
- Transaction isolation for test safety

### Fluent Assertions
- Domain-specific assertion methods
- Readable test failure messages
- Validation result testing
- Entity property verification

## Usage Examples

### Unit Testing

```csharp
[TestClass]
public class UserServiceTests : TestBase
{
    private readonly UserBuilder _userBuilder = new();

    [TestMethod]
    public async Task GetUser_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = _userBuilder.BuildValid();
        var mockRepository = GetMock<IUserRepository>();
        mockRepository.Setup(r => r.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        var userService = new UserService(mockRepository.Object);

        // Act
        var result = await userService.GetUserAsync(user.Id);

        // Assert
        result.ShouldBeSuccess();
        result.Data!.ShouldHaveId(user.Id);
    }
}
```

### Integration Testing

```csharp
[TestClass]
public class UserControllerIntegrationTests : IntegrationTestBase
{
    private List<User> _testUsers = null!;

    protected override void SetupTestData()
    {
        _testUsers = new List<User>
        {
            new UserBuilder().BuildWithUsername("user1"),
            new UserBuilder().BuildWithUsername("user2"),
            new UserBuilder().BuildWithUsername("user3")
        };

        TestDatabase.SeedDataAsync(_testUsers).Wait();
    }

    [TestMethod]
    public async Task GetUsers_ReturnsAllUsers()
    {
        // Arrange
        var controller = GetRequiredService<UserController>();

        // Act
        var result = await controller.GetUsers();

        // Assert
        result.ShouldBeSuccess();
        Assert.AreEqual(3, result.Data!.Count);
    }
}
```

### Test Data Builders

```csharp
public class UserBuilder : BaseTestEntityBuilder<User>
{
    public override User BuildValid()
    {
        return new User
        {
            Id = TestHelper.GenerateRandomGuid(),
            Username = TestHelper.GenerateRandomString(8),
            Email = TestHelper.GenerateRandomEmail(),
            PasswordHash = TestHelper.GenerateRandomString(64),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public User BuildWithUsername(string username)
    {
        var user = BuildValid();
        user.Username = username;
        return user;
    }

    public User BuildWithEmail(string email)
    {
        var user = BuildValid();
        user.Email = email;
        return user;
    }
}
```

### Custom Assertions

```csharp
[TestMethod]
public async Task ValidateUser_WithInvalidData_ReturnsValidationErrors()
{
    // Arrange
    var invalidUser = new UserBuilder().BuildInvalid();
    var validator = GetRequiredService<IUserValidator>();

    // Act
    var result = await validator.ValidateAsync(invalidUser);

    // Assert
    result.ShouldBeInvalid();
    result.ShouldContainError("Username is required");
    result.ShouldContainError("Email format is invalid");
}
```

## Configuration

### Database Configuration

```csharp
// In your test setup
public class MyIntegrationTests : IntegrationTestBase
{
    protected override void SetupTestEnvironment()
    {
        // Use in-memory database for fast tests
        TestDatabase = new TestDatabase("InMemory");
        
        // Or use SQLite for more realistic testing
        TestDatabase = new TestDatabase("Data Source=test.db");
        
        // Or use SQL Server for full database testing
        TestDatabase = new TestDatabase("Server=(localdb)\\mssqllocaldb;Database=TestDB;Trusted_Connection=true;");
    }
}
```

### Dependency Injection Setup

```csharp
public class MyTestContainer : TestContainer
{
    protected override void RegisterTestServices(IServiceCollection services)
    {
        // Register your test services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        // Register mocks
        services.AddScoped<INotificationService>(_ => GetMock<INotificationService>().Object);
    }
}
```

## Best Practices

### Test Organization
- Use descriptive test method names: `MethodName_Scenario_ExpectedResult`
- Group related tests in the same test class
- Follow the AAA pattern (Arrange, Act, Assert)
- Keep tests independent and isolated

### Test Data Management
- Use builders for creating test data
- Avoid hardcoded test data
- Clean up test data after each test
- Use realistic but minimal test data

### Mocking Strategy
- Mock external dependencies
- Don't mock the system under test
- Use strict mocks when appropriate
- Verify mock interactions when important

### Assertion Patterns
- Use custom assertions for domain-specific checks
- Assert one concept per test
- Provide meaningful failure messages
- Use fluent assertion syntax when possible

## Requirements

- .NET 8.0 or later
- xUnit 2.6.6 or later
- Entity Framework Core 8.0 or later (for database testing)
- Moq 4.20.70 or later (for mocking)

## Dependencies

This library includes the following dependencies:
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.EntityFrameworkCore (with SQL Server, SQLite, In-Memory support)
- Microsoft.Extensions.Logging
- Moq
- xUnit

## Contributing

We welcome contributions! Please see our contributing guidelines for more information.