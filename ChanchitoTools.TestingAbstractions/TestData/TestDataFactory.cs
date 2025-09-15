namespace ChanchitoBackend.System.Abstractions.TestData
{
    /// <summary>
    /// Concrete implementation of test data factory with comprehensive observability
    /// </summary>
    public class TestDataFactory : ITestDataFactory
    {
        private readonly Random _random;
        private int _generationCount;

        public TestDataFactory()
        {
            _random = new Random();
            Console.WriteLine($"[TEST DATA FACTORY] TestDataFactory initialized with random seed");
        }

        public TestDataFactory(int seed)
        {
            _random = new Random(seed);
            Console.WriteLine($"[TEST DATA FACTORY] TestDataFactory initialized with seed {seed}");
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        public string GenerateRandomString(int length = 10)
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random string (length: {length}, total generations: {_generationCount})");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            Console.WriteLine($"[TEST DATA FACTORY] Generated string: '{result}'");
            return result;
        }

        /// <summary>
        /// Generates a random email address
        /// </summary>
        /// <returns>A random email address</returns>
        public string GenerateRandomEmail()
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random email (total generations: {_generationCount})");

            var username = GenerateRandomString(8);
            var domain = GenerateRandomString(6);
            var extensions = new[] { "com", "org", "net", "edu", "gov", "io", "co", "uk", "de", "fr" };
            var extension = extensions[_random.Next(extensions.Length)];
            var result = $"{username}@{domain}.{extension}";

            Console.WriteLine($"[TEST DATA FACTORY] Generated email: '{result}'");
            return result;
        }

        /// <summary>
        /// Generates a random date
        /// </summary>
        /// <param name="startYear">The start year</param>
        /// <param name="endYear">The end year</param>
        /// <returns>A random date</returns>
        public DateTime GenerateRandomDate(int startYear = 2020, int endYear = 2030)
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random date ({startYear}-{endYear}, total generations: {_generationCount})");

            var start = new DateTime(startYear, 1, 1);
            var end = new DateTime(endYear, 12, 31);
            var range = (end - start).Days;
            var result = start.AddDays(_random.Next(range));

            Console.WriteLine($"[TEST DATA FACTORY] Generated date: {result:yyyy-MM-dd}");
            return result;
        }

        /// <summary>
        /// Generates a random GUID
        /// </summary>
        /// <returns>A random GUID</returns>
        public Guid GenerateRandomGuid()
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random GUID (total generations: {_generationCount})");

            var result = Guid.NewGuid();
            Console.WriteLine($"[TEST DATA FACTORY] Generated GUID: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random integer
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>A random integer</returns>
        public int GenerateRandomInt(int min = 1, int max = 1000)
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random int ({min}-{max}, total generations: {_generationCount})");

            var result = _random.Next(min, max + 1);
            Console.WriteLine($"[TEST DATA FACTORY] Generated int: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random decimal
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="decimals">The number of decimal places</param>
        /// <returns>A random decimal</returns>
        public decimal GenerateRandomDecimal(decimal min = 0.01m, decimal max = 1000.00m, int decimals = 2)
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random decimal ({min}-{max}, {decimals} decimals, total generations: {_generationCount})");

            var range = (double)(max - min);
            var randomValue = (double)min + (_random.NextDouble() * range);
            var result = Math.Round((decimal)randomValue, decimals);

            Console.WriteLine($"[TEST DATA FACTORY] Generated decimal: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random boolean
        /// </summary>
        /// <returns>A random boolean</returns>
        public bool GenerateRandomBoolean()
        {
            Interlocked.Increment(ref _generationCount);
            Console.WriteLine($"[TEST DATA FACTORY] Generating random boolean (total generations: {_generationCount})");

            var result = _random.Next(2) == 1;
            Console.WriteLine($"[TEST DATA FACTORY] Generated boolean: {result}");
            return result;
        }

        /// <summary>
        /// Generates a random item from a collection
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="items">The collection of items</param>
        /// <returns>A random item from the collection</returns>
        public T GenerateRandomItem<T>(IEnumerable<T> items)
        {
            Interlocked.Increment(ref _generationCount);
            var itemCount = items.Count();
            Console.WriteLine($"[TEST DATA FACTORY] Generating random item from collection ({itemCount} items, total generations: {_generationCount})");

            var result = items.ElementAt(_random.Next(itemCount));
            Console.WriteLine($"[TEST DATA FACTORY] Generated item: {result}");
            return result;
        }

        /// <summary>
        /// Gets the total number of data generations
        /// </summary>
        /// <returns>The generation count</returns>
        public int GetGenerationCount()
        {
            Console.WriteLine($"[TEST DATA FACTORY] Total generations: {_generationCount}");
            return _generationCount;
        }
    }
}