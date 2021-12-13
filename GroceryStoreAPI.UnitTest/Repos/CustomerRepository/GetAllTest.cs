using System;
using System.Linq;
using Xunit;
using GroceryStoreAPI.Domain;

namespace GroceryStoreAPI.UnitTest.Repos.CustomerRepo
{
    public class GetAllTest:IClassFixture<TestEnvironmentWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly TestEnvironmentWebApplicationFactory<Startup> _factory;
        public GetAllTest(TestEnvironmentWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private Customer GivenCustomer(string name)
        {
            return Customer.Create(name);
        }

        [Fact(DisplayName = "Customer Repository GetAll Should Return Multiple Records When Multiple Exist")]
        public void CustomerRepository_Should_Return_Multiple_Records_When_Multiple_Exist()
        {           
            _factory.CreateClient();
            var logger = _factory.GetCustomerRepositoryLogger();
           
            _factory.SeedDbContext<GroceryStoreDbContext>(db =>
                {
                    db.Customers.AddRange(
                        GivenCustomer("Test Name 1"),
                        GivenCustomer("Test Name 2"),
                        GivenCustomer("Test Name 3")
                    );
                }
            );
            
            _factory.WithDbContext<GroceryStoreDbContext>(async db =>
            {
                var customerRepository = new CustomerRepository(db, logger);
                var actual = await customerRepository.GetList();

                Assert.NotNull(actual);
                Assert.Equal(3, actual.Count());
                Assert.Equal("Test Name 1", actual.FirstOrDefault().Name);
            });                 
        }

        [Fact(DisplayName = "Customer Repository GetAll Should Succeed And Return One Customer When Single Seeded")]
        public void CustomerRepository_Should_Return_One_Customer_When_Single_Seeded()
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
                var actual = await customerRepository.GetList();

                Assert.NotNull(actual);
                Assert.Single(actual);
                Assert.Equal("TestName", actual.FirstOrDefault().Name);
            });                 
        }

        [Fact(DisplayName = "Customer Repository GetAll Should Succeed And Return Empty When No Records Exist")]
        public void CustomerRepository_Should_Return_Empty_When_No_Records_Exist()
        {           
            _factory.CreateClient();
            var logger = _factory.GetCustomerRepositoryLogger();                      
            
            _factory.WithDbContext<GroceryStoreDbContext>(async db =>
            {
                var customerRepository = new CustomerRepository(db, logger);
                var actual = await customerRepository.GetList();

                Assert.NotNull(actual);
                Assert.Empty(actual);                
            });                 
        }
        
        public void Dispose()
		{		
			try { _factory.ClearInMemoryDatabase(); } catch { }
		}        
    }     
}
