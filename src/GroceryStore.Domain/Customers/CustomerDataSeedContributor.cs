using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace GroceryStore.Customers
{
    public class CustomerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Customer> _customerRepo;

        public CustomerDataSeedContributor(IRepository<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // if this has already been populated, ignore
            if (await _customerRepo.GetCountAsync() > 0)
                return;

            var x = AppDomain.CurrentDomain.BaseDirectory;
            using (
                StreamReader sRdr = new StreamReader(
                    AppDomain.CurrentDomain.BaseDirectory + @"\Properties\database.json"
                )
            ) {
                string text = sRdr.ReadToEnd();
                var jo = JObject.Parse(text);
                var customers = jo["customers"].Select(
                        c => new Customer { LegacyId = (int)c["id"], Name = (string)c["name"] }
                    )
                    .ToList();
                await _customerRepo.InsertManyAsync(customers);
            }
        }
    }
}
