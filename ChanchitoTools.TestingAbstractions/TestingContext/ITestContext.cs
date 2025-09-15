using ChanchitoBackend.System.Abstractions.Assertions;
using ChanchitoBackend.System.Abstractions.Builders;
using ChanchitoBackend.System.Abstractions.TestData;

namespace ChanchitoBackend.System.Abstractions.TestingContext
{
    /// <summary>
    /// Interface for test context that manages test state and dependencies
    /// </summary>
    public interface ITestContext
    {
        /// <summary>
        /// Gets the test data factory
        /// </summary>
        ITestDataFactory TestDataFactory { get; }

        /// <summary>
        /// Gets a test assertion instance for the specified type
        /// </summary>
        /// <typeparam name="T">The type to assert</typeparam>
        /// <returns>A test assertion instance</returns>
        ITestAssertion<T> GetAssertion<T>() where T : class;

        /// <summary>
        /// Gets a test entity builder for the specified type
        /// </summary>
        /// <typeparam name="T">The type to build</typeparam>
        /// <returns>A test entity builder instance</returns>
        ITestEntityBuilder<T> GetBuilder<T>() where T : class;

        /// <summary>
        /// Registers a test entity builder for the specified type
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="builder">The builder to register</param>
        void RegisterBuilder<T>(ITestEntityBuilder<T> builder) where T : class;

        /// <summary>
        /// Clears all registered builders
        /// </summary>
        void ClearBuilders();

        /// <summary>
        /// Gets a collection of test entities
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="count">The number of entities to get</param>
        /// <returns>A collection of test entities</returns>
        IEnumerable<T> GetTestEntities<T>(int count) where T : class;

        /// <summary>
        /// Gets a single test entity
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>A test entity</returns>
        T GetTestEntity<T>() where T : class;
    }
}