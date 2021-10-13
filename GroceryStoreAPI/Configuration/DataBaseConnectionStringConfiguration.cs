using Microsoft.Extensions.Configuration;

namespace GroceryStoreAPI.Configuration
{
    public class DataBaseConnectionStringConfiguration
    {
        public string Db { get; set; }

        public DataBaseConnectionStringConfiguration()
        {
        }

        public DataBaseConnectionStringConfiguration(IConfiguration configuration)
        {
            Db = configuration.GetConnectionString(nameof(Db));
        }
    }
}