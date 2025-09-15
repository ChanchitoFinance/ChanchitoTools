using ChanchitoBackend.System.Abstractions.Assertions;
using ChanchitoBackend.System.Abstractions.Observability;
using ChanchitoBackend.System.Abstractions.TestData;

namespace ChanchitoBackend.System.Abstractions.Helpers
{
    /// <summary>
    /// Static helper class for common testing operations with comprehensive observability
    /// </summary>
    public static class TestHelper
    {
        private static readonly ITestDataFactory _testDataFactory = new TestDataFactory();
        private static readonly ITestMetrics _metrics = new TestMetrics();
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        /// <summary>
        /// Asserts that a model is valid using the provided validator
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation result</returns>
        public static ValidationResult AssertValid<T>(T model, IValidator<T> validator) where T : class
        {
            Console.WriteLine($"[TEST HELPER] AssertValid called for {typeof(T).Name}");
            var assertion = new TestAssertion<T>(_loggerFactory.CreateLogger<TestAssertion<T>>(), _metrics);
            return assertion.AssertValid(model, validator);
        }

        /// <summary>
        /// Asserts that a model is invalid using the provided validator
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation result</returns>
        public static ValidationResult AssertInvalid<T>(T model, IValidator<T> validator) where T : class
        {
            Console.WriteLine($"[TEST HELPER] AssertInvalid called for {typeof(T).Name}");
            var assertion = new TestAssertion<T>(_loggerFactory.CreateLogger<TestAssertion<T>>(), _metrics);
            return assertion.AssertInvalid(model, validator);
        }

        /// <summary>
        /// Asserts that a model satisfies a specific rule
        /// </summary>
        /// <typeparam name="T">The type to test</typeparam>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        /// <returns>True if the rule is satisfied</returns>
        public static bool AssertRuleSatisfied<T>(T model, IRule<T> rule) where T : class
        {
            Console.WriteLine($"[TEST HELPER] AssertRuleSatisfied called for {typeof(T).Name}");
            var assertion = new TestAssertion<T>(_loggerFactory.CreateLogger<TestAssertion<T>>(), _metrics);
            return assertion.AssertRuleSatisfied(model, rule);
        }

        /// <summary>
        /// Asserts that a model does not satisfy a specific rule
        /// </summary>
        /// <typeparam name="T">The type to test</typeparam>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        /// <returns>True if the rule is not satisfied</returns>
        public static bool AssertRuleNotSatisfied<T>(T model, IRule<T> rule) where T : class
        {
            Console.WriteLine($"[TEST HELPER] AssertRuleNotSatisfied called for {typeof(T).Name}");
            var assertion = new TestAssertion<T>(_loggerFactory.CreateLogger<TestAssertion<T>>(), _metrics);
            return assertion.AssertRuleNotSatisfied(model, rule);
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        public static string GenerateRandomString(int length = 10)
        {
            Console.WriteLine($"[TEST HELPER] Generating random string of length {length}");
            var result = _testDataFactory.GenerateRandomString(length);
            Console.WriteLine($"[TEST HELPER] Generated: '{result}'");
            return result;
        }

        /// <summary>
        /// Generates a random email address
        /// </summary>
        /// <returns>A random email address</returns>
        public static string GenerateRandomEmail()
        {
            Console.WriteLine($"[TEST HELPER] Generating random email");
            var result = _testDataFactory.GenerateRandomEmail();
            Console.WriteLine($"[TEST HELPER] Generated: '{result}'");
            return result;
        }

        /// <summary>
        /// Generates a random date
        /// </summary>
        /// <param name="startYear">The start year</param>
        /// <param name="endYear">The end year</param>
        /// <returns>A random date</returns>
        public static DateTime GenerateRandomDate(int startYear = 2020, int endYear = 2030)
        {
            Console.WriteLine($"[TEST HELPER] Generating random date between {startYear} and {endYear}");
            var result = _testDataFactory.GenerateRandomDate(startYear, endYear);
            Console.WriteLine($"[TEST HELPER] Generated: {result:yyyy-MM-dd}");
            return result;
        }

        /// <summary>
        /// Generates a random GUID
        /// </summary>
        /// <returns>A random GUID</returns>
        public static Guid GenerateRandomGuid()
        {
            Console.WriteLine($"[TEST HELPER] Generating random GUID");
            var result = _testDataFactory.GenerateRandomGuid();
            Console.WriteLine($"[TEST HELPER] Generated: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random integer
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>A random integer</returns>
        public static int GenerateRandomInt(int min = 1, int max = 1000)
        {
            Console.WriteLine($"[TEST HELPER] Generating random int between {min} and {max}");
            var result = _testDataFactory.GenerateRandomInt(min, max);
            Console.WriteLine($"[TEST HELPER] Generated: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random decimal
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="decimals">The number of decimal places</param>
        /// <returns>A random decimal</returns>
        public static decimal GenerateRandomDecimal(decimal min = 0.01m, decimal max = 1000.00m, int decimals = 2)
        {
            Console.WriteLine($"[TEST HELPER] Generating random decimal between {min} and {max} with {decimals} decimals");
            var result = _testDataFactory.GenerateRandomDecimal(min, max, decimals);
            Console.WriteLine($"[TEST HELPER] Generated: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random boolean
        /// </summary>
        /// <returns>A random boolean</returns>
        public static bool GenerateRandomBoolean()
        {
            Console.WriteLine($"[TEST HELPER] Generating random boolean");
            var result = _testDataFactory.GenerateRandomBoolean();
            Console.WriteLine($"[TEST HELPER] Generated: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random item from a collection
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="items">The collection of items</param>
        /// <returns>A random item from the collection</returns>
        public static T GenerateRandomItem<T>(IEnumerable<T> items)
        {
            Console.WriteLine($"[TEST HELPER] Generating random item from collection of {items.Count()} items");
            var result = _testDataFactory.GenerateRandomItem(items);
            Console.WriteLine($"[TEST HELPER] Generated: {result}");
            return result;
        }

        /// <summary>
        /// Creates a test data factory with a specific seed
        /// </summary>
        /// <param name="seed">The seed for the random number generator</param>
        /// <returns>A test data factory</returns>
        public static ITestDataFactory CreateTestDataFactory(int seed)
        {
            Console.WriteLine($"[TEST HELPER] Creating TestDataFactory with seed {seed}");
            return new TestDataFactory(seed);
        }

        /// <summary>
        /// Gets the current metrics summary
        /// </summary>
        /// <returns>Metrics summary</returns>
        public static string GetMetricsSummary()
        {
            Console.WriteLine($"[TEST HELPER] Getting metrics summary");
            return _metrics.GetMetricsSummary();
        }
    }
}