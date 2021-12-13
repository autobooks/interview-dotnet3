using System.Linq;
using System.Threading.Tasks;
using GroceryStoreAPI.Domain;
using Newtonsoft.Json;

namespace GroceryStoreAPI.Infrastructure
{
    public static class DataLoader
    {
        public static async Task Load(GroceryStoreDbContext dbContext, AppSettings appSettings)
        {   
           
            // Get filename from app settings
            var dataFile = appSettings?.JsonSampleDataFile;
            if (!string.IsNullOrEmpty(dataFile) && dbContext.Customers.Count() == 0)
            {
                // Deserialize in a class that has a list of Customers because of the json format
                var jsonData = JsonConvert.DeserializeObject<CustomerList>(System.IO.File.ReadAllText(dataFile));
                // Add to database
                dbContext.Customers.AddRange(jsonData.Customers);
                await dbContext.SaveChangesAsync(); 
            }                          
        }
    }
}
