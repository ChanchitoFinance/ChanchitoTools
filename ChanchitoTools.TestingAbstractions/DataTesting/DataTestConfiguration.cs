namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Configuration for data testing scenarios
    /// </summary>
    public class DataTestConfiguration
    {
        /// <summary>
        /// Gets or sets the database provider to use for testing
        /// </summary>
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.InMemory;

        /// <summary>
        /// Gets or sets the connection string for the database
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database name for in-memory databases
        /// </summary>
        public string DatabaseName { get; set; } = "TestDb";

        /// <summary>
        /// Gets or sets whether to enable sensitive data logging
        /// </summary>
        public bool EnableSensitiveDataLogging { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to enable detailed errors
        /// </summary>
        public bool EnableDetailedErrors { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to use lazy loading proxies
        /// </summary>
        public bool UseLazyLoadingProxies { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to automatically seed the database
        /// </summary>
        public bool AutoSeedDatabase { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to use transactions for test isolation
        /// </summary>
        public bool UseTransactions { get; set; } = true;

        /// <summary>
        /// Gets or sets the command timeout in seconds
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// Gets or sets the maximum retry count for transient failures
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// Gets or sets the retry delay in seconds
        /// </summary>
        public int RetryDelay { get; set; } = 1;

        /// <summary>
        /// Creates a configuration for in-memory database testing
        /// </summary>
        /// <param name="databaseName">The database name</param>
        /// <returns>A configuration for in-memory testing</returns>
        public static DataTestConfiguration CreateInMemory(string databaseName = "TestDb")
        {
            return new DataTestConfiguration
            {
                Provider = DatabaseProvider.InMemory,
                DatabaseName = databaseName,
                EnableSensitiveDataLogging = true,
                EnableDetailedErrors = true,
                AutoSeedDatabase = false,
                UseTransactions = false
            };
        }

        /// <summary>
        /// Creates a configuration for SQLite database testing
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns>A configuration for SQLite testing</returns>
        public static DataTestConfiguration CreateSqlite(string connectionString = "Data Source=TestDb.db")
        {
            return new DataTestConfiguration
            {
                Provider = DatabaseProvider.Sqlite,
                ConnectionString = connectionString,
                EnableSensitiveDataLogging = true,
                EnableDetailedErrors = true,
                AutoSeedDatabase = false,
                UseTransactions = true
            };
        }

        /// <summary>
        /// Creates a configuration for SQL Server database testing
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns>A configuration for SQL Server testing</returns>
        public static DataTestConfiguration CreateSqlServer(string connectionString)
        {
            return new DataTestConfiguration
            {
                Provider = DatabaseProvider.SqlServer,
                ConnectionString = connectionString,
                EnableSensitiveDataLogging = true,
                EnableDetailedErrors = true,
                AutoSeedDatabase = false,
                UseTransactions = true,
                CommandTimeout = 30,
                MaxRetryCount = 3,
                RetryDelay = 1
            };
        }
    }
}