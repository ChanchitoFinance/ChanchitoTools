namespace ChanchitoBackend.System.Abstractions.Builders
{
    /// <summary>
    /// Base interface for test entity builders that create objects for testing
    /// </summary>
    /// <typeparam name="T">The type to build</typeparam>
    public interface ITestEntityBuilder<T> where T : class
    {
        /// <summary>
        /// Builds a valid entity for testing
        /// </summary>
        /// <returns>A valid instance</returns>
        T BuildValid();

        /// <summary>
        /// Builds an invalid entity for testing
        /// </summary>
        /// <returns>An invalid instance</returns>
        T BuildInvalid();

        /// <summary>
        /// Builds a collection of valid entities for testing
        /// </summary>
        /// <param name="count">The number of entities to build</param>
        /// <returns>A collection of valid instances</returns>
        IEnumerable<T> BuildValidCollection(int count);

        /// <summary>
        /// Builds a collection of invalid entities for testing
        /// </summary>
        /// <param name="count">The number of entities to build</param>
        /// <returns>A collection of invalid instances</returns>
        IEnumerable<T> BuildInvalidCollection(int count);

        /// <summary>
        /// Builds an entity with custom properties
        /// </summary>
        /// <param name="customizer">Action to customize the entity</param>
        /// <returns>A customized instance</returns>
        T BuildWithCustomization(Action<T> customizer);
    }
}