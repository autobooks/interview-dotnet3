using System;
using Xunit;
using GroceryStoreAPI.DTO;

namespace GroceryStoreAPI.UnitTest.Repos.CustomerRepo
{
    // Test that customer data is added.
    // Test one failed scenario. 
    // Additional fail scenarios are in the Customer domain object.
    public class AddTest:IClassFixture<TestEnvironmentWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly TestEnvironmentWebApplicationFactory<Startup> _factory;
        public AddTest(TestEnvironmentWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private static CustomerDTO GivenCustomerDTO(string name)
        {
             return new CustomerDTO
                {
                    Name = name
                };
        }
       
        [Fact(DisplayName = "Customer Repository Add Should Succeed When Valid Data")]
        public void CustomerRepository_Add_Should_Succeed_When_Valid_Data()
        {           
            _factory.CreateClient();            
            var logger = _factory.GetCustomerRepositoryLogger();
                       
            _factory.WithDbContext<GroceryStoreDbContext>(async db =>
            {
                var customerRepository = new CustomerRepository(db, logger);
                var actual = await customerRepository.Add(GivenCustomerDTO("Test Name"));     
                
                // safe to assume the id is 1 for this demo 
                var entity = db.Customers.Find(1);

                Assert.NotNull(actual);
                Assert.NotNull(entity);
                Assert.Equal(actual, entity);
                Assert.Equal("Test Name", actual.Name);
                Assert.Equal("Test Name", entity.Name);
            });                                    
        }

        [Fact(DisplayName = "Customer Repository Add Should Fail When Name Is Empty String")]
        public void CustomerRepository__Add_Should_Fail_When_Name_Is_Empty_String()
        {           
            _factory.CreateClient();            
            var logger = _factory.GetCustomerRepositoryLogger();
                       
            _factory.WithDbContext<GroceryStoreDbContext>(db =>
            {
                var customerRepository = new CustomerRepository(db, logger);

                Assert.ThrowsAsync<ArgumentNullException>("name", async () => await customerRepository.Add(GivenCustomerDTO("")));
            });                                    
        }

        public void Dispose()
		{		
			_factory.ClearInMemoryDatabase();
		}        
    }     
}
