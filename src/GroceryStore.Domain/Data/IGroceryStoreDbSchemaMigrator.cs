namespace GroceryStore.Data
{
    using System.Threading.Tasks;

    /// <summary>
	/// Defines the <see cref="IGroceryStoreDbSchemaMigrator" />.
	/// </summary>
    public interface IGroceryStoreDbSchemaMigrator
    {
        /// <summary>
		/// The MigrateAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
        Task MigrateAsync();
    }
}
