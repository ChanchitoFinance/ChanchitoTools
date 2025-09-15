using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace ChanchitoBackend.System.Abstractions.E2ETesting
{
    /// <summary>
    /// Base class for E2E tests that provides WebApplicationFactory, HttpClient, and full application context
    /// Uses the real application service configuration instead of creating artificial test services
    /// </summary>
    /// <typeparam name="TProgram">The Program type from the API project</typeparam>
    /// <typeparam name="TDbContext">The DbContext type to use for testing</typeparam>
    public abstract class ApplicationE2ETestBase<TProgram, TDbContext> : IAsyncLifetime
        where TProgram : class
        where TDbContext : DbContext
    {
        protected WebApplicationFactory<TProgram> WebAppFactory { get; private set; } = null!;
        protected HttpClient HttpClient { get; private set; } = null!;
        protected IApplicationTestFixture<TDbContext> ApplicationFixture { get; private set; } = null!;
        protected IServiceScope ServiceScope { get; private set; } = null!;
        protected IServiceProvider Services => ServiceScope.ServiceProvider;
        
        private readonly string _databaseName;
        private readonly bool _useInMemoryDatabase;
        private readonly IApplicationTestBuilder _customTestBuilder;

        /// <summary>
        /// Initializes a new instance of the ApplicationE2ETestBase
        /// </summary>
        /// <param name="testBuilder">The test builder that knows how to configure the specific application</param>
        /// <param name="useInMemoryDatabase">Whether to use in-memory database</param>
        /// <param name="databaseName">Optional database name</param>
        protected ApplicationE2ETestBase(IApplicationTestBuilder testBuilder, bool useInMemoryDatabase = true, string? databaseName = null)
        {
            _useInMemoryDatabase = useInMemoryDatabase;
            _databaseName = databaseName ?? $"test_db_{Guid.NewGuid():N}";
            _customTestBuilder = testBuilder ?? throw new ArgumentNullException(nameof(testBuilder));
        }

        /// <summary>
        /// Initializes the test
        /// </summary>
        public virtual async Task InitializeAsync()
        {
            // Create the application test fixture using the real application configuration
            ApplicationFixture = _useInMemoryDatabase
                ? ApplicationTestFixtureFactory.CreateInMemory<TDbContext>(_customTestBuilder, _databaseName)
                : ApplicationTestFixtureFactory.CreateSqlite<TDbContext>(_customTestBuilder, _databaseName);

            // Initialize the fixture
            await ApplicationFixture.InitializeAsync();
            
            // Create WebApplicationFactory
            WebAppFactory = CreateWebApplicationFactory();
            HttpClient = WebAppFactory.CreateClient();
            ServiceScope = WebAppFactory.Services.CreateScope();
            
            // Setup test data
            await SetupTestDataAsync();
        }

        /// <summary>
        /// Disposes the test
        /// </summary>
        public virtual async Task DisposeAsync()
        {
            await CleanupTestDataAsync();
            
            ServiceScope?.Dispose();
            HttpClient?.Dispose();
            WebAppFactory?.Dispose();
            
            if (ApplicationFixture != null)
            {
                await ApplicationFixture.DisposeAsync();
            }
        }

        /// <summary>
        /// Creates a WebApplicationFactory with test configuration
        /// </summary>
        protected virtual WebApplicationFactory<TProgram> CreateWebApplicationFactory()
        {
            return new WebApplicationFactory<TProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Configure test logging
                        ConfigureTestLogging(services);
                        
                        // Configure test authentication
                        ConfigureTestAuthentication(services);
                        
                        // Replace the database context with our test database
                        ConfigureTestDatabase(services);
                        
                        // Allow additional test service configuration
                        ConfigureAdditionalTestServices(services);
                    });
                });
        }

        /// <summary>
        /// Configures test logging
        /// </summary>
        protected virtual void ConfigureTestLogging(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
        }

        /// <summary>
        /// Configures test authentication
        /// </summary>
        protected virtual void ConfigureTestAuthentication(IServiceCollection services)
        {
            // Override authentication services for testing if needed
            // This can be customized based on your authentication requirements
        }

        /// <summary>
        /// Configures the test database to use the same database as the application fixture
        /// </summary>
        protected virtual void ConfigureTestDatabase(IServiceCollection services)
        {
            // Remove existing DbContext registrations
            var descriptorsToRemove = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<TDbContext>) ||
                (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)) ||
                d.ServiceType == typeof(TDbContext)).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Register the test database context with the same connection as the application fixture
            var connectionString = _useInMemoryDatabase 
                ? $"Data Source={_databaseName};Mode=Memory;Cache=Shared"
                : $"Data Source={_databaseName}.db";

            if (_useInMemoryDatabase)
            {
                services.AddDbContext<TDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });
            }
            else
            {
                services.AddDbContext<TDbContext>(options =>
                {
                    options.UseSqlite(connectionString);
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });
            }
        }

        /// <summary>
        /// Configures additional test services
        /// Override this method to add custom test service configurations
        /// </summary>
        protected virtual void ConfigureAdditionalTestServices(IServiceCollection services)
        {
            // Override to add additional test services
        }

        /// <summary>
        /// Sets up test data
        /// </summary>
        protected virtual async Task SetupTestDataAsync()
        {
            // Override to setup test data
            await Task.CompletedTask;
        }

        /// <summary>
        /// Cleans up test data
        /// </summary>
        protected virtual async Task CleanupTestDataAsync()
        {
            // Override to cleanup test data
            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets a service from the application test fixture
        /// </summary>
        protected T GetRequiredService<T>() where T : class
        {
            return ApplicationFixture.GetRequiredService<T>();
        }

        /// <summary>
        /// Gets a service from the application test fixture
        /// </summary>
        protected T? GetService<T>() where T : class
        {
            return ApplicationFixture.GetService<T>();
        }

        /// <summary>
        /// Gets the database context from the application test fixture
        /// </summary>
        protected TDbContext GetDbContext()
        {
            return ApplicationFixture.DbContext;
        }

        /// <summary>
        /// Creates a new service scope from the application test fixture
        /// </summary>
        protected IServiceScope CreateApplicationScope()
        {
            return ApplicationFixture.CreateScope();
        }

        /// <summary>
        /// Clears all data from the test database
        /// </summary>
        protected async Task ClearDatabaseAsync()
        {
            await ApplicationFixture.ClearDatabaseAsync();
        }

        /// <summary>
        /// Creates an authenticated HTTP client with JWT token
        /// </summary>
        protected HttpClient CreateAuthenticatedClient(string token)
        {
            var client = WebAppFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        /// <summary>
        /// Creates an authenticated HTTP client with user credentials
        /// </summary>
        protected async Task<HttpClient> CreateAuthenticatedClientAsync(string email, string password)
        {
            var token = await GetAuthTokenAsync(email, password);
            return CreateAuthenticatedClient(token);
        }

        /// <summary>
        /// Gets authentication token for a user
        /// </summary>
        protected async Task<string> GetAuthTokenAsync(string email, string password)
        {
            var loginRequest = new { email, password };
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("/auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return authResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get access token");
        }

        /// <summary>
        /// Registers a new user and returns the user data
        /// </summary>
        protected async Task<UserData> RegisterUserAsync(string fullName, string email, string password)
        {
            var registerRequest = new { fullName, email, password };
            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("/auth/register", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new UserData
            {
                Id = authResponse?.User?.Id ?? throw new InvalidOperationException("Failed to get user ID"),
                FullName = fullName,
                Email = email,
                AccessToken = authResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get access token")
            };
        }

        /// <summary>
        /// Creates a test user and returns authenticated client
        /// </summary>
        protected async Task<(HttpClient Client, UserData User)> CreateTestUserAsync(
            string fullName = "Test User",
            string email = "test@example.com",
            string password = "TestPassword123!")
        {
            var user = await RegisterUserAsync(fullName, email, password);
            var client = CreateAuthenticatedClient(user.AccessToken);
            return (client, user);
        }

        /// <summary>
        /// Asserts that an HTTP response is successful
        /// </summary>
        protected static void AssertSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new InvalidOperationException(
                    $"Expected success status code, but got {response.StatusCode}. Content: {content}");
            }
        }
    }

    /// <summary>
    /// Convenience class for E2E tests using the default DbContext
    /// </summary>
    public abstract class ApplicationE2ETestBase<TProgram> : ApplicationE2ETestBase<TProgram, DbContext>
        where TProgram : class
    {
        protected ApplicationE2ETestBase(IApplicationTestBuilder testBuilder, bool useInMemoryDatabase = true, string? databaseName = null) 
            : base(testBuilder, useInMemoryDatabase, databaseName) { }
    }

    /// <summary>
    /// User data for testing
    /// </summary>
    public class UserData
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Authentication response model
    /// </summary>
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public User? User { get; set; }
    }

    /// <summary>
    /// User model
    /// </summary>
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
