using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace GroceryStore.Data
{
    /* This is used if database provider does't define
     * IGroceryStoreDbSchemaMigrator implementation.
     */
    public class NullGroceryStoreDbSchemaMigrator
        : IGroceryStoreDbSchemaMigrator,
          ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
