namespace GroceryStore.Customers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Volo.Abp.Data;
    using Volo.Abp.DependencyInjection;
    using Volo.Abp.Domain.Repositories;

    /// <summary>
    /// Defines the <see cref="CustomerDataSeedContributor" />.
    /// </summary>
    public class CustomerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        /// <summary>
        /// Defines the _customerRepo.
        /// </summary>
        private readonly IRepository<Customer> _customerRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDataSeedContributor"/> class.
        /// </summary>
        /// <param name="customerRepo">The customerRepo<see cref="IRepository{Customer}"/>.</param>
        public CustomerDataSeedContributor(IRepository<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        /// <summary>
        /// The SeedAsync.
        /// </summary>
        /// <param name="context">The context<see cref="DataSeedContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
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
                var customers = JObject.Parse(text)
                    .SelectToken("customers")
                    .ToObject<dynamic[]>()
                    .Select(c => new Customer { LegacyId = c.id, Name = c.name });

                await _customerRepo.InsertManyAsync(customers);
            }
        }
    }
}
