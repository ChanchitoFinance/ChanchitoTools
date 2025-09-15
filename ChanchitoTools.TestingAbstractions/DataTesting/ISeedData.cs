namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Interface for seed data that can be used to populate test databases
    /// </summary>
    public interface ISeedData
    {
        /// <summary>
        /// Seeds the database with test data
        /// </summary>
        /// <param name="context">The database context</param>
        void Seed(DbContext context);

        /// <summary>
        /// Gets the name of the seed data
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the seed data
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets whether this seed data should be applied automatically
        /// </summary>
        bool AutoApply { get; }
    }
}