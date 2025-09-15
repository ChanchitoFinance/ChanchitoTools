using System.Collections.Concurrent;

namespace ChanchitoBackend.System.Abstractions.Observability
{
    /// <summary>
    /// Concrete implementation of test metrics with comprehensive logging
    /// </summary>
    public class TestMetrics : ITestMetrics
    {
        private int _validationCount;
        private int _assertionCount;
        private int _builderCount;
        private int _validValidationCount;
        private int _invalidValidationCount;
        private int _passedAssertionCount;
        private int _failedAssertionCount;
        private readonly ConcurrentDictionary<string, int> _assertionTypeCounts;
        private readonly ConcurrentDictionary<string, int> _builderTypeCounts;
        private readonly ConcurrentDictionary<string, int> _entityTypeCounts;

        public TestMetrics()
        {
            _assertionTypeCounts = new ConcurrentDictionary<string, int>();
            _builderTypeCounts = new ConcurrentDictionary<string, int>();
            _entityTypeCounts = new ConcurrentDictionary<string, int>();
        }

        public void IncrementValidationCount()
        {
            Interlocked.Increment(ref _validationCount);
            Console.WriteLine($"[METRICS] Validation count incremented. Total: {_validationCount}");
        }

        public void IncrementAssertionCount()
        {
            Interlocked.Increment(ref _assertionCount);
            Console.WriteLine($"[METRICS] Assertion count incremented. Total: {_assertionCount}");
        }

        public void IncrementBuilderCount()
        {
            Interlocked.Increment(ref _builderCount);
            Console.WriteLine($"[METRICS] Builder count incremented. Total: {_builderCount}");
        }

        public void RecordValidationResult(bool isValid, int errorCount)
        {
            if (isValid)
            {
                Interlocked.Increment(ref _validValidationCount);
                Console.WriteLine($"[METRICS] Validation PASSED (0 errors) - Valid: {_validValidationCount}, Invalid: {_invalidValidationCount}");
            }
            else
            {
                Interlocked.Increment(ref _invalidValidationCount);
                Console.WriteLine($"[METRICS] Validation FAILED ({errorCount} errors) - Valid: {_validValidationCount}, Invalid: {_invalidValidationCount}");
            }
        }

        public void RecordAssertionResult(string assertionType, bool passed)
        {
            _assertionTypeCounts.AddOrUpdate(assertionType, 1, (key, value) => value + 1);

            if (passed)
            {
                Interlocked.Increment(ref _passedAssertionCount);
                Console.WriteLine($"[METRICS] Assertion '{assertionType}' PASSED - Passed: {_passedAssertionCount}, Failed: {_failedAssertionCount}");
            }
            else
            {
                Interlocked.Increment(ref _failedAssertionCount);
                Console.WriteLine($"[METRICS] Assertion '{assertionType}' FAILED - Passed: {_passedAssertionCount}, Failed: {_failedAssertionCount}");
            }
        }

        public void RecordBuilderOperation(string builderType, string entityType)
        {
            _builderTypeCounts.AddOrUpdate(builderType, 1, (key, value) => value + 1);
            _entityTypeCounts.AddOrUpdate(entityType, 1, (key, value) => value + 1);

            Console.WriteLine($"[METRICS] Builder '{builderType}' created entity '{entityType}' - Builder calls: {_builderTypeCounts[builderType]}, Entity instances: {_entityTypeCounts[entityType]}");
        }

        public string GetMetricsSummary()
        {
            var summary = $@"
[METRICS SUMMARY]
===================
Validations: {_validationCount} (Valid: {_validValidationCount}, Invalid: {_invalidValidationCount})
Assertions: {_assertionCount} (Passed: {_passedAssertionCount}, Failed: {_failedAssertionCount})
Builders: {_builderCount}

Assertion Types:
{string.Join(Environment.NewLine, _assertionTypeCounts.Select(kvp => $"  {kvp.Key}: {kvp.Value}"))}

Builder Types:
{string.Join(Environment.NewLine, _builderTypeCounts.Select(kvp => $"  {kvp.Key}: {kvp.Value}"))}

Entity Types:
{string.Join(Environment.NewLine, _entityTypeCounts.Select(kvp => $"  {kvp.Key}: {kvp.Value}"))}
===================";

            Console.WriteLine(summary);
            return summary;
        }
    }
}