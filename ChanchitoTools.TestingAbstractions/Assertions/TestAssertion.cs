using ChanchitoBackend.System.Abstractions.Observability;

namespace ChanchitoBackend.System.Abstractions.Assertions
{
    /// <summary>
    /// Concrete implementation of test assertions for any type with comprehensive observability
    /// </summary>
    /// <typeparam name="T">The type to assert</typeparam>
    public class TestAssertion<T> : ITestAssertion<T> where T : class
    {
        private readonly ILogger<TestAssertion<T>> _logger;
        private readonly ITestMetrics _metrics;

        public TestAssertion(ILogger<TestAssertion<T>> logger, ITestMetrics metrics)
        {
            _logger = logger;
            _metrics = metrics;
        }

        /// <summary>
        /// Asserts that the model is valid according to the validator
        /// </summary>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation result</returns>
        public ValidationResult AssertValid(T model, IValidator<T> validator)
        {
            Console.WriteLine($"[ASSERTION] Starting AssertValid for {typeof(T).Name}");
            Console.WriteLine($"[ASSERTION] Model: {GetModelSummary(model)}");
            Console.WriteLine($"[ASSERTION] Validator: {validator.GetType().Name}");

            _metrics.IncrementAssertionCount();
            _metrics.IncrementValidationCount();

            var result = validator.Validate(model);

            Console.WriteLine($"[ASSERTION] Result: IsValid={result.IsValid}, Errors={result.Errors.Count}");

            if (result.Errors.Any())
            {
                Console.WriteLine($"[ASSERTION] Validation errors:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"[ASSERTION]   - {error}");
                }
            }

            _metrics.RecordValidationResult(result.IsValid, result.Errors.Count);

            if (!result.IsValid)
            {
                var errorMessage = $"Expected model to be valid, but validation failed with errors: {string.Join(", ", result.Errors)}";
                Console.WriteLine($"[ASSERTION] ASSERTION FAILED: {errorMessage}");
                Console.WriteLine($"[ASSERTION] DETAILED DEBUG INFO:");
                Console.WriteLine($"[ASSERTION]   Model Type: {typeof(T).Name}");
                Console.WriteLine($"[ASSERTION]   Model Properties: {GetDetailedModelInfo(model)}");
                Console.WriteLine($"[ASSERTION]   Validator Type: {validator.GetType().Name}");
                Console.WriteLine($"[ASSERTION]   Validation Result: IsValid={result.IsValid}");
                Console.WriteLine($"[ASSERTION]   Error Count: {result.Errors.Count}");
                Console.WriteLine($"[ASSERTION]   All Errors:");
                for (int i = 0; i < result.Errors.Count; i++)
                {
                    Console.WriteLine($"[ASSERTION]     Error {i + 1}: {result.Errors[i]}");
                }
                _metrics.RecordAssertionResult("AssertValid", false);
                throw new AssertionException(errorMessage);
            }

            Console.WriteLine($"[ASSERTION] AssertValid PASSED");
            _metrics.RecordAssertionResult("AssertValid", true);
            return result;
        }

        /// <summary>
        /// Asserts that the model is invalid according to the validator
        /// </summary>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation result</returns>
        public ValidationResult AssertInvalid(T model, IValidator<T> validator)
        {
            Console.WriteLine($"[ASSERTION] Starting AssertInvalid for {typeof(T).Name}");
            Console.WriteLine($"[ASSERTION] Model: {GetModelSummary(model)}");
            Console.WriteLine($"[ASSERTION] Validator: {validator.GetType().Name}");

            _metrics.IncrementAssertionCount();
            _metrics.IncrementValidationCount();

            var result = validator.Validate(model);

            Console.WriteLine($"[ASSERTION] Result: IsValid={result.IsValid}, Errors={result.Errors.Count}");

            if (result.Errors.Any())
            {
                Console.WriteLine($"[ASSERTION] Validation errors:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"[ASSERTION]   - {error}");
                }
            }

            _metrics.RecordValidationResult(result.IsValid, result.Errors.Count);

            if (result.IsValid)
            {
                var errorMessage = "Expected model to be invalid, but validation passed";
                Console.WriteLine($"[ASSERTION] ASSERTION FAILED: {errorMessage}");
                Console.WriteLine($"[ASSERTION] DETAILED DEBUG INFO:");
                Console.WriteLine($"[ASSERTION]   Model Type: {typeof(T).Name}");
                Console.WriteLine($"[ASSERTION]   Model Properties: {GetDetailedModelInfo(model)}");
                Console.WriteLine($"[ASSERTION]   Validator Type: {validator.GetType().Name}");
                Console.WriteLine($"[ASSERTION]   Validation Result: IsValid={result.IsValid}");
                Console.WriteLine($"[ASSERTION]   Error Count: {result.Errors.Count}");
                _metrics.RecordAssertionResult("AssertInvalid", false);
                throw new AssertionException(errorMessage);
            }

            Console.WriteLine($"[ASSERTION] AssertInvalid PASSED");
            _metrics.RecordAssertionResult("AssertInvalid", true);
            return result;
        }

        /// <summary>
        /// Asserts that the model satisfies a specific rule
        /// </summary>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        /// <returns>True if the rule is satisfied</returns>
        public bool AssertRuleSatisfied(T model, IRule<T> rule)
        {
            Console.WriteLine($"[ASSERTION] Starting AssertRuleSatisfied for {typeof(T).Name}");
            Console.WriteLine($"[ASSERTION] Model: {GetModelSummary(model)}");
            Console.WriteLine($"[ASSERTION] Rule: {rule.GetType().Name}");

            _metrics.IncrementAssertionCount();

            var isSatisfied = rule.IsSatisfiedBy(model);

            Console.WriteLine($"[ASSERTION] Result: IsSatisfied={isSatisfied}");

            if (!isSatisfied)
            {
                var errorMessage = $"Expected rule to be satisfied, but it failed with error: {rule.ErrorMessage}";
                Console.WriteLine($"[ASSERTION] ASSERTION FAILED: {errorMessage}");
                Console.WriteLine($"[ASSERTION] DETAILED DEBUG INFO:");
                Console.WriteLine($"[ASSERTION]   Model Type: {typeof(T).Name}");
                Console.WriteLine($"[ASSERTION]   Model Properties: {GetDetailedModelInfo(model)}");
                Console.WriteLine($"[ASSERTION]   Rule Type: {rule.GetType().Name}");
                Console.WriteLine($"[ASSERTION]   Rule IsSatisfied: {isSatisfied}");
                Console.WriteLine($"[ASSERTION]   Rule Error Message: {rule.ErrorMessage}");
                _metrics.RecordAssertionResult("AssertRuleSatisfied", false);
                throw new AssertionException(errorMessage);
            }

            Console.WriteLine($"[ASSERTION] AssertRuleSatisfied PASSED");
            _metrics.RecordAssertionResult("AssertRuleSatisfied", true);
            return true;
        }

        /// <summary>
        /// Asserts that the model does not satisfy a specific rule
        /// </summary>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        /// <returns>True if the rule is not satisfied</returns>
        public bool AssertRuleNotSatisfied(T model, IRule<T> rule)
        {
            Console.WriteLine($"[ASSERTION] Starting AssertRuleNotSatisfied for {typeof(T).Name}");
            Console.WriteLine($"[ASSERTION] Model: {GetModelSummary(model)}");
            Console.WriteLine($"[ASSERTION] Rule: {rule.GetType().Name}");

            _metrics.IncrementAssertionCount();

            var isSatisfied = rule.IsSatisfiedBy(model);

            Console.WriteLine($"[ASSERTION] Result: IsSatisfied={isSatisfied}");

            if (isSatisfied)
            {
                var errorMessage = "Expected rule to not be satisfied, but it passed";
                Console.WriteLine($"[ASSERTION] ASSERTION FAILED: {errorMessage}");
                Console.WriteLine($"[ASSERTION] DETAILED DEBUG INFO:");
                Console.WriteLine($"[ASSERTION]   Model Type: {typeof(T).Name}");
                Console.WriteLine($"[ASSERTION]   Model Properties: {GetDetailedModelInfo(model)}");
                Console.WriteLine($"[ASSERTION]   Rule Type: {rule.GetType().Name}");
                Console.WriteLine($"[ASSERTION]   Rule IsSatisfied: {isSatisfied}");
                Console.WriteLine($"[ASSERTION]   Rule Error Message: {rule.ErrorMessage}");
                _metrics.RecordAssertionResult("AssertRuleNotSatisfied", false);
                throw new AssertionException(errorMessage);
            }

            Console.WriteLine($"[ASSERTION] AssertRuleNotSatisfied PASSED");
            _metrics.RecordAssertionResult("AssertRuleNotSatisfied", true);
            return true;
        }

        /// <summary>
        /// Gets a summary of the model for logging purposes
        /// </summary>
        /// <param name="model">The model to summarize</param>
        /// <returns>A string summary of the model</returns>
        private string GetModelSummary(T model)
        {
            if (model == null)
                return "null";

            try
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                    .Take(5) // Limit to first 5 properties to avoid too much output
                    .Select(p => $"{p.Name}={p.GetValue(model)?.ToString() ?? "null"}")
                    .ToArray();

                return $"{{{string.Join(", ", properties)}}}";
            }
            catch
            {
                return model.ToString() ?? "Unknown";
            }
        }

        /// <summary>
        /// Gets detailed information about the model for debugging
        /// </summary>
        /// <param name="model">The model to analyze</param>
        /// <returns>Detailed model information</returns>
        private string GetDetailedModelInfo(T model)
        {
            if (model == null)
                return "Model is null";

            try
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                    .Select(p =>
                    {
                        var value = p.GetValue(model);
                        var stringValue = value?.ToString() ?? "null";
                        var length = stringValue?.Length ?? 0;
                        return $"{p.Name}='{stringValue}' (length: {length})";
                    })
                    .ToArray();

                return string.Join(", ", properties);
            }
            catch (Exception ex)
            {
                return $"Error getting model info: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// Custom exception for test assertion failures
    /// </summary>
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message)
        {
        }

        public AssertionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}