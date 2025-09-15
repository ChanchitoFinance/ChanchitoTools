namespace ChanchitoBackend.System.Abstractions.Assertions
{
    /// <summary>
    /// Base interface for test assertions that work with any type
    /// </summary>
    /// <typeparam name="T">The type to assert</typeparam>
    public interface ITestAssertion<T> where T : class
    {
        /// <summary>
        /// Asserts that the model is valid according to the validator
        /// </summary>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation result</returns>
        ValidationResult AssertValid(T model, IValidator<T> validator);

        /// <summary>
        /// Asserts that the model is invalid according to the validator
        /// </summary>
        /// <param name="model">The model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <returns>The validation result</returns>
        ValidationResult AssertInvalid(T model, IValidator<T> validator);

        /// <summary>
        /// Asserts that the model satisfies a specific rule
        /// </summary>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        /// <returns>True if the rule is satisfied</returns>
        bool AssertRuleSatisfied(T model, IRule<T> rule);

        /// <summary>
        /// Asserts that the model does not satisfy a specific rule
        /// </summary>
        /// <param name="model">The model to test</param>
        /// <param name="rule">The rule to test against</param>
        /// <returns>True if the rule is not satisfied</returns>
        bool AssertRuleNotSatisfied(T model, IRule<T> rule);
    }

    /// <summary>
    /// Generic validator interface
    /// </summary>
    /// <typeparam name="T">The type to validate</typeparam>
    public interface IValidator<T> where T : class
    {
        ValidationResult Validate(T model);
    }

    /// <summary>
    /// Generic rule interface
    /// </summary>
    /// <typeparam name="T">The type to test</typeparam>
    public interface IRule<T> where T : class
    {
        bool IsSatisfiedBy(T model);
        string ErrorMessage { get; }
    }

    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; } = new();

        public void AddError(string error) => Errors.Add(error);
    }
}