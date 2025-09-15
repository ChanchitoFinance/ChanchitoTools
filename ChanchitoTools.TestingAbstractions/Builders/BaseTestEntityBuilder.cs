namespace ChanchitoBackend.System.Abstractions.Builders
{
    /// <summary>
    /// Base abstract class for test entity builders with common functionality and comprehensive observability
    /// </summary>
    /// <typeparam name="T">The type to build</typeparam>
    public abstract class BaseTestEntityBuilder<T> : ITestEntityBuilder<T> where T : class
    {
        private int _buildCount;
        private int _validBuildCount;
        private int _invalidBuildCount;

        /// <summary>
        /// Builds a valid entity for testing
        /// </summary>
        /// <returns>A valid instance</returns>
        public abstract T BuildValid();

        /// <summary>
        /// Builds an invalid entity for testing
        /// </summary>
        /// <returns>An invalid instance</returns>
        public abstract T BuildInvalid();

        /// <summary>
        /// Builds a collection of valid entities for testing
        /// </summary>
        /// <param name="count">The number of entities to build</param>
        /// <returns>A collection of valid instances</returns>
        public virtual IEnumerable<T> BuildValidCollection(int count)
        {
            if (count <= 0)
            {
                var errorMessage = "Count must be greater than zero";
                Console.WriteLine($"[BUILDER] {errorMessage}");
                throw new ArgumentException(errorMessage, nameof(count));
            }

            Console.WriteLine($"[BUILDER] Building {count} valid entities of type {typeof(T).Name}");

            var entities = Enumerable.Range(0, count).Select(_ => BuildValid()).ToList();

            Console.WriteLine($"[BUILDER] Built {entities.Count} valid entities");
            return entities;
        }

        /// <summary>
        /// Builds a collection of invalid entities for testing
        /// </summary>
        /// <param name="count">The number of entities to build</param>
        /// <returns>A collection of invalid instances</returns>
        public virtual IEnumerable<T> BuildInvalidCollection(int count)
        {
            if (count <= 0)
            {
                var errorMessage = "Count must be greater than zero";
                Console.WriteLine($"[BUILDER] {errorMessage}");
                throw new ArgumentException(errorMessage, nameof(count));
            }

            Console.WriteLine($"[BUILDER] Building {count} invalid entities of type {typeof(T).Name}");

            var entities = Enumerable.Range(0, count).Select(_ => BuildInvalid()).ToList();

            Console.WriteLine($"[BUILDER] Built {entities.Count} invalid entities");
            return entities;
        }

        /// <summary>
        /// Builds an entity with custom properties
        /// </summary>
        /// <param name="customizer">Action to customize the entity</param>
        /// <returns>A customized instance</returns>
        public virtual T BuildWithCustomization(Action<T> customizer)
        {
            if (customizer == null)
            {
                var errorMessage = "Customizer cannot be null";
                Console.WriteLine($"[BUILDER] {errorMessage}");
                throw new ArgumentNullException(nameof(customizer));
            }

            Console.WriteLine($"[BUILDER] Building customized entity of type {typeof(T).Name}");

            var entity = BuildValid();
            customizer(entity);

            Console.WriteLine($"[BUILDER] Built customized entity");
            return entity;
        }

        /// <summary>
        /// Creates a random string for testing purposes
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        protected static string CreateRandomString(int length = 10)
        {
            Console.WriteLine($"[BUILDER] Creating random string of length {length}");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            Console.WriteLine($"[BUILDER] Created random string: '{result}'");
            return result;
        }

        /// <summary>
        /// Creates a random email for testing purposes
        /// </summary>
        /// <returns>A random email address</returns>
        protected static string CreateRandomEmail()
        {
            Console.WriteLine($"[BUILDER] Creating random email");

            var random = new Random();
            var username = CreateRandomString(8);
            var domain = CreateRandomString(6);
            var extensions = new[] { "com", "org", "net", "edu", "gov", "io", "co", "uk", "de", "fr" };
            var extension = extensions[random.Next(extensions.Length)];
            var result = $"{username}@{domain}.{extension}";

            Console.WriteLine($"[BUILDER] Created random email: '{result}'");
            return result;
        }

        /// <summary>
        /// Creates a random date for testing purposes
        /// </summary>
        /// <param name="startYear">The start year for the random date</param>
        /// <param name="endYear">The end year for the random date</param>
        /// <returns>A random date</returns>
        protected static DateTime CreateRandomDate(int startYear = 2020, int endYear = 2030)
        {
            Console.WriteLine($"[BUILDER] Creating random date between {startYear} and {endYear}");

            var random = new Random();
            var start = new DateTime(startYear, 1, 1);
            var end = new DateTime(endYear, 12, 31);
            var range = (end - start).Days;
            var result = start.AddDays(random.Next(range));

            Console.WriteLine($"[BUILDER] Created random date: {result:yyyy-MM-dd}");
            return result;
        }

        /// <summary>
        /// Records a build operation for metrics
        /// </summary>
        /// <param name="isValid">Whether the built entity is valid</param>
        protected void RecordBuildOperation(bool isValid)
        {
            Interlocked.Increment(ref _buildCount);

            if (isValid)
            {
                Interlocked.Increment(ref _validBuildCount);
                Console.WriteLine($"[BUILDER] Built valid entity. Total builds: {_buildCount}, Valid: {_validBuildCount}, Invalid: {_invalidBuildCount}");
            }
            else
            {
                Interlocked.Increment(ref _invalidBuildCount);
                Console.WriteLine($"[BUILDER] Built invalid entity. Total builds: {_buildCount}, Valid: {_validBuildCount}, Invalid: {_invalidBuildCount}");
            }
        }

        /// <summary>
        /// Gets build statistics
        /// </summary>
        /// <returns>Build statistics summary</returns>
        public string GetBuildStatistics()
        {
            var stats = $@"
[BUILDER STATISTICS]
=====================
Total Builds: {_buildCount}
Valid Builds: {_validBuildCount}
Invalid Builds: {_invalidBuildCount}
Type: {typeof(T).Name}
=====================";

            Console.WriteLine(stats);
            return stats;
        }
    }
}