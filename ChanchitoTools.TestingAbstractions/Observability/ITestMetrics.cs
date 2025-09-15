namespace ChanchitoBackend.System.Abstractions.Observability
{
    /// <summary>
    /// Interface for tracking test metrics and operations
    /// </summary>
    public interface ITestMetrics
    {
        /// <summary>
        /// Increments the validation counter
        /// </summary>
        void IncrementValidationCount();

        /// <summary>
        /// Increments the assertion counter
        /// </summary>
        void IncrementAssertionCount();

        /// <summary>
        /// Increments the builder counter
        /// </summary>
        void IncrementBuilderCount();

        /// <summary>
        /// Records a validation result
        /// </summary>
        /// <param name="isValid">Whether the validation passed</param>
        /// <param name="errorCount">Number of errors</param>
        void RecordValidationResult(bool isValid, int errorCount);

        /// <summary>
        /// Records an assertion result
        /// </summary>
        /// <param name="assertionType">Type of assertion</param>
        /// <param name="passed">Whether the assertion passed</param>
        void RecordAssertionResult(string assertionType, bool passed);

        /// <summary>
        /// Records a builder operation
        /// </summary>
        /// <param name="builderType">Type of builder</param>
        /// <param name="entityType">Type of entity built</param>
        void RecordBuilderOperation(string builderType, string entityType);

        /// <summary>
        /// Gets the current metrics summary
        /// </summary>
        /// <returns>Metrics summary</returns>
        string GetMetricsSummary();
    }
}