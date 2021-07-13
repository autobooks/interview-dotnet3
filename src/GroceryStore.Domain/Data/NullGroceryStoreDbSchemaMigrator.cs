namespace GroceryStore.Data
{
    using System.Threading.Tasks;
    using Volo.Abp.DependencyInjection;

    /* This is used if database provider does't define
     * IGroceryStoreDbSchemaMigrator implementation.
     */

    /// <summary>
	/// Defines the <see cref="NullGroceryStoreDbSchemaMigrator" />.
	/// </summary>
    public class NullGroceryStoreDbSchemaMigrator
        : IGroceryStoreDbSchemaMigrator,
          ITransientDependency
    {
        /// <summary>
		/// The MigrateAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
