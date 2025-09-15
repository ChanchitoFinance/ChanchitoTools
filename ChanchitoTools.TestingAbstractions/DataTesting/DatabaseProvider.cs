namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Enumeration of supported database providers for testing
    /// </summary>
    public enum DatabaseProvider
    {
        /// <summary>
        /// SQLite database provider
        /// </summary>
        Sqlite,

        /// <summary>
        /// In-memory database provider
        /// </summary>
        InMemory,

        /// <summary>
        /// SQL Server database provider
        /// </summary>
        SqlServer,

        /// <summary>
        /// PostgreSQL database provider
        /// </summary>
        PostgreSql
    }
}