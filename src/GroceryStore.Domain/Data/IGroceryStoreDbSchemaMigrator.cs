using System.Threading.Tasks;

namespace GroceryStore.Data
{
    public interface IGroceryStoreDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
