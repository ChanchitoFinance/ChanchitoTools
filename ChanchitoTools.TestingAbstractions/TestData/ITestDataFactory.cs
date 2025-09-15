namespace ChanchitoBackend.System.Abstractions.TestData
{
    /// <summary>
    /// Interface for test data factory that generates test data
    /// </summary>
    public interface ITestDataFactory
    {
        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        string GenerateRandomString(int length = 10);

        /// <summary>
        /// Generates a random email address
        /// </summary>
        /// <returns>A random email address</returns>
        string GenerateRandomEmail();

        /// <summary>
        /// Generates a random date
        /// </summary>
        /// <param name="startYear">The start year</param>
        /// <param name="endYear">The end year</param>
        /// <returns>A random date</returns>
        DateTime GenerateRandomDate(int startYear = 2020, int endYear = 2030);

        /// <summary>
        /// Generates a random GUID
        /// </summary>
        /// <returns>A random GUID</returns>
        Guid GenerateRandomGuid();

        /// <summary>
        /// Generates a random integer
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>A random integer</returns>
        int GenerateRandomInt(int min = 1, int max = 1000);

        /// <summary>
        /// Generates a random decimal
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="decimals">The number of decimal places</param>
        /// <returns>A random decimal</returns>
        decimal GenerateRandomDecimal(decimal min = 0.01m, decimal max = 1000.00m, int decimals = 2);

        /// <summary>
        /// Generates a random boolean
        /// </summary>
        /// <returns>A random boolean</returns>
        bool GenerateRandomBoolean();

        /// <summary>
        /// Generates a random item from a collection
        /// </summary>
        /// <typeparam name="T">The type of items in the collection</typeparam>
        /// <param name="items">The collection of items</param>
        /// <returns>A random item from the collection</returns>
        T GenerateRandomItem<T>(IEnumerable<T> items);
    }
}