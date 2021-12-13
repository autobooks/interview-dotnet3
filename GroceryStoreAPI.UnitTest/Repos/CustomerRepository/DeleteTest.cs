using System;
using System.Linq;
using Xunit;
using GroceryStoreAPI.Domain;

namespace GroceryStoreAPI.UnitTest.Repos.CustomerRepo
{
    public class DeleteTest:IClassFixture<TestEnvironmentWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly TestEnvironmentWebApplicationFactory<Startup> _factory;
        public DeleteTest(TestEnvironmentWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private Customer GivenCustomer(string name)
        {
            return Customer.Create(name);
        }

      
        [Fact(DisplayName = "Customer Repository Delete Should Succeed When Customer Exists")]
        public void CustomerRepository_Delete_Should_Succeed_When_Customer_Exists()
        {           
            _factory.CreateClient();
            var logger = _factory.GetCustomerRepositoryLogger();
           
            _factory.SeedDbContext<GroceryStoreDbContext>(db =>
                {
                    db.Customers.Add(GivenCustomer("TestName"));
                }
            );
            
            _factory.WithDbContext<GroceryStoreDbContext>(async db =>
            {
                var customerRepository = new CustomerRepository(db, logger);

                // for demo purposes it is safe to assume id is 1
                var result = await customerRepository.Delete(1);
                
                var entities = db.Customers;

                Assert.True(result);
                Assert.Empty(entities);                               
            });                 
        }

        [Fact(DisplayName = "Customer Repository Delete Should Fail When Customer Doesnt Exists")]
        public void CustomerRepository_Delete_Should_Fail_When_Customer_Doesnt_Exists()
        {           
            _factory.CreateClient();
            var logger = _factory.GetCustomerRepositoryLogger();
           
            _factory.SeedDbContext<GroceryStoreDbContext>(db =>
                {
                    db.Customers.Add(GivenCustomer("TestName"));
                }
            );
            
            _factory.WithDbContext<GroceryStoreDbContext>(async db =>
            {
                var customerRepository = new CustomerRepository(db, logger);

                // for demo purposes it is safe to assume id 2 doesn't exist
                var result = await customerRepository.Delete(2);
                
                var entities = db.Customers;

                Assert.False(result);
                Assert.Equal(1, entities.Count());                               
            });                 
        }
        
        public void Dispose()
		{		
			try { _factory.ClearInMemoryDatabase(); } catch { }
		}        
    }     
}
