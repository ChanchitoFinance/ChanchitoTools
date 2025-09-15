using ChanchitoBackend.System.Abstractions.Assertions;
using ChanchitoBackend.System.Abstractions.Builders;
using ChanchitoBackend.System.Abstractions.Observability;
using ChanchitoBackend.System.Abstractions.TestData;


namespace ChanchitoBackend.System.Abstractions.TestingContext
{
    /// <summary>
    /// Concrete implementation of test context
    /// </summary>
    public class TestContext : ITestContext
    {
        private readonly Dictionary<Type, object> _builders;
        private readonly ITestDataFactory _testDataFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ITestMetrics _metrics;

        public TestContext(ILoggerFactory loggerFactory, ITestMetrics metrics)
        {
            _builders = new Dictionary<Type, object>();
            _testDataFactory = new TestDataFactory();
            _loggerFactory = loggerFactory;
            _metrics = metrics;
        }

        public TestContext(int seed, ILoggerFactory loggerFactory, ITestMetrics metrics)
        {
            _builders = new Dictionary<Type, object>();
            _testDataFactory = new TestDataFactory(seed);
            _loggerFactory = loggerFactory;
            _metrics = metrics;
        }

        /// <summary>
        /// Gets the test data factory
        /// </summary>
        public ITestDataFactory TestDataFactory => _testDataFactory;

        /// <summary>
        /// Gets a test assertion instance for the specified type
        /// </summary>
        /// <typeparam name="T">The type to assert</typeparam>
        /// <returns>A test assertion instance</returns>
        public ITestAssertion<T> GetAssertion<T>() where T : class
        {
            return new TestAssertion<T>(_loggerFactory.CreateLogger<TestAssertion<T>>(), _metrics);
        }

        /// <summary>
        /// Gets a test entity builder for the specified type
        /// </summary>
        /// <typeparam name="T">The type to build</typeparam>
        /// <returns>A test entity builder instance</returns>
        public ITestEntityBuilder<T> GetBuilder<T>() where T : class
        {
            var type = typeof(T);

            if (!_builders.ContainsKey(type))
            {
                throw new InvalidOperationException($"No builder registered for type {type.Name}. Use RegisterBuilder<T>() to register a builder first.");
            }

            return (ITestEntityBuilder<T>)_builders[type];
        }

        /// <summary>
        /// Registers a test entity builder for the specified type
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="builder">The builder to register</param>
        public void RegisterBuilder<T>(ITestEntityBuilder<T> builder) where T : class
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var type = typeof(T);
            _builders[type] = builder;
        }

        /// <summary>
        /// Clears all registered builders
        /// </summary>
        public void ClearBuilders()
        {
            _builders.Clear();
        }

        /// <summary>
        /// Gets a collection of test entities
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="count">The number of entities to get</param>
        /// <returns>A collection of test entities</returns>
        public IEnumerable<T> GetTestEntities<T>(int count) where T : class
        {
            var builder = GetBuilder<T>();
            return builder.BuildValidCollection(count);
        }

        /// <summary>
        /// Gets a single test entity
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>A test entity</returns>
        public T GetTestEntity<T>() where T : class
        {
            var builder = GetBuilder<T>();
            return builder.BuildValid();
        }
    }
}