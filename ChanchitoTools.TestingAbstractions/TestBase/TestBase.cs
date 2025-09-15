using ChanchitoBackend.System.Abstractions.Assertions;
using ChanchitoBackend.System.Abstractions.Builders;
using ChanchitoBackend.System.Abstractions.Observability;
using ChanchitoBackend.System.Abstractions.TestData;
using ChanchitoBackend.System.Abstractions.TestingContext;
using Xunit;

namespace ChanchitoBackend.System.Abstractions.TestBase
{
    /// <summary>
    /// Base class for tests that provides common testing functionality with comprehensive observability
    /// </summary>
    public abstract class TestBase : IAsyncLifetime
    {
        protected readonly ITestContext TestContext;
        protected readonly ITestDataFactory TestDataFactory;
        protected readonly ITestMetrics Metrics;
        protected readonly ILogger<TestBase> Logger;
        private bool _isInitialized = false;

        protected TestBase()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var metrics = new TestMetrics();

            TestContext = new TestContext(loggerFactory, metrics);
            TestDataFactory = TestContext.TestDataFactory;
            Metrics = metrics;
            Logger = loggerFactory.CreateLogger<TestBase>();

            Console.WriteLine($"[TEST BASE] TestBase initialized for {GetType().Name}");
        }

        protected TestBase(int seed)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var metrics = new TestMetrics();

            TestContext = new TestContext(seed, loggerFactory, metrics);
            TestDataFactory = TestContext.TestDataFactory;
            Metrics = metrics;
            Logger = loggerFactory.CreateLogger<TestBase>();

            Console.WriteLine($"[TEST BASE] TestBase initialized for {GetType().Name} with seed {seed}");
        }

        /// <summary>
        /// Initialize the test fixture asynchronously
        /// </summary>
        public virtual async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                Console.WriteLine($"[TEST BASE] InitializeAsync called for {GetType().Name}");
                await OnSetUpAsync();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Cleanup the test fixture asynchronously
        /// </summary>
        public virtual async Task DisposeAsync()
        {
            if (_isInitialized)
            {
                Console.WriteLine($"[TEST BASE] DisposeAsync called for {GetType().Name}");
                await OnTearDownAsync();
                _isInitialized = false;
            }
        }

        /// <summary>
        /// Gets the assertion helper for the specified type
        /// </summary>
        /// <typeparam name="T">The type to get assertions for</typeparam>
        /// <returns>An assertion helper</returns>
        protected ITestAssertion<T> GetAssertion<T>() where T : class
        {
            return TestContext.GetAssertion<T>();
        }

        /// <summary>
        /// Gets the entity builder for the specified type
        /// </summary>
        /// <typeparam name="T">The type to get a builder for</typeparam>
        /// <returns>An entity builder</returns>
        protected ITestEntityBuilder<T> GetBuilder<T>() where T : class
        {
            return TestContext.GetBuilder<T>();
        }

        /// <summary>
        /// Registers a builder for the specified type
        /// </summary>
        /// <typeparam name="T">The type to register a builder for</typeparam>
        /// <param name="builder">The builder to register</param>
        protected void RegisterBuilder<T>(ITestEntityBuilder<T> builder) where T : class
        {
            TestContext.RegisterBuilder(builder);
        }

        /// <summary>
        /// Gets multiple test entities of the specified type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="count">The number of entities to generate</param>
        /// <returns>A collection of test entities</returns>
        protected IEnumerable<T> GetTestEntities<T>(int count) where T : class
        {
            return TestContext.GetTestEntities<T>(count);
        }

        /// <summary>
        /// Gets a single test entity of the specified type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>A test entity</returns>
        protected T GetTestEntity<T>() where T : class
        {
            return TestContext.GetTestEntity<T>();
        }

        /// <summary>
        /// Asserts that a model is valid using the provided validator
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        protected void AssertModelValid<T>(T model, IValidator<T> validator) where T : class
        {
            Console.WriteLine($"[TEST BASE] AssertModelValid called for {typeof(T).Name}");
            Console.WriteLine($"[TEST BASE] Model: {GetEntitySummary(model)}");
            Console.WriteLine($"[TEST BASE] Validator: {validator.GetType().Name}");

            var assertion = GetAssertion<T>();
            assertion.AssertValid(model, validator);

            Console.WriteLine($"[TEST BASE] AssertModelValid PASSED");
        }

        /// <summary>
        /// Asserts that a model is invalid using the provided validator
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        protected void AssertModelInvalid<T>(T model, IValidator<T> validator) where T : class
        {
            Console.WriteLine($"[TEST BASE] AssertModelInvalid called for {typeof(T).Name}");
            Console.WriteLine($"[TEST BASE] Model: {GetEntitySummary(model)}");
            Console.WriteLine($"[TEST BASE] Validator: {validator.GetType().Name}");

            var assertion = GetAssertion<T>();
            assertion.AssertInvalid(model, validator);

            Console.WriteLine($"[TEST BASE] AssertModelInvalid PASSED");
        }

        /// <summary>
        /// Asserts that a model satisfies a specific rule
        /// </summary>
        /// <typeparam name="T">The type to test</typeparam>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        protected void AssertRuleSatisfied<T>(T model, IRule<T> rule) where T : class
        {
            Console.WriteLine($"[TEST BASE] AssertRuleSatisfied called for {typeof(T).Name}");
            Console.WriteLine($"[TEST BASE] Model: {GetEntitySummary(model)}");
            Console.WriteLine($"[TEST BASE] Rule: {rule.GetType().Name}");

            var assertion = GetAssertion<T>();
            assertion.AssertRuleSatisfied(model, rule);

            Console.WriteLine($"[TEST BASE] AssertRuleSatisfied PASSED");
        }

        /// <summary>
        /// Asserts that a model does not satisfy a specific rule
        /// </summary>
        /// <typeparam name="T">The type to test</typeparam>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        protected void AssertRuleNotSatisfied<T>(T model, IRule<T> rule) where T : class
        {
            Console.WriteLine($"[TEST BASE] AssertRuleNotSatisfied called for {typeof(T).Name}");
            Console.WriteLine($"[TEST BASE] Model: {GetEntitySummary(model)}");
            Console.WriteLine($"[TEST BASE] Rule: {rule.GetType().Name}");

            var assertion = GetAssertion<T>();
            assertion.AssertRuleNotSatisfied(model, rule);

            Console.WriteLine($"[TEST BASE] AssertRuleNotSatisfied PASSED");
        }

        /// <summary>
        /// Virtual method for test setup - called automatically by InitializeAsync
        /// </summary>
        protected virtual Task OnSetUpAsync()
        {
            Console.WriteLine($"[TEST BASE] OnSetUpAsync called for {GetType().Name}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Virtual method for test teardown - called automatically by DisposeAsync
        /// </summary>
        protected virtual Task OnTearDownAsync()
        {
            Console.WriteLine($"[TEST BASE] OnTearDownAsync called for {GetType().Name}");
            Console.WriteLine($"[TEST BASE] Final metrics summary:");
            Console.WriteLine(Metrics.GetMetricsSummary());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a summary of an entity for logging purposes
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity to summarize</param>
        /// <returns>A string summary of the entity</returns>
        private string GetEntitySummary<T>(T entity)
        {
            if (entity == null)
                return "null";

            try
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                    .Take(3) // Limit to first 3 properties to avoid too much output
                    .Select(p => $"{p.Name}={p.GetValue(entity)?.ToString() ?? "null"}")
                    .ToArray();

                return $"{{{string.Join(", ", properties)}}}";
            }
            catch
            {
                return entity.ToString() ?? "Unknown";
            }
        }
    }
}