using System;
using Xunit;
using GroceryStoreAPI.DTO;
using GroceryStoreAPI.Domain;

namespace GroceryStoreAPI.UnitTest.Repos.CustomerRepo
{
    // Test that customer data is updated.
    // Test one failed scenario. 
    // Additional fail scenarios are in the Customer domain object.

    public class UpdateTest:IClassFixture<TestEnvironmentWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly TestEnvironmentWebApplicationFactory<Startup> _factory;
        public UpdateTest(TestEnvironmentWebApplicationFactory<Startup> factory)
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

        private Customer GivenCustomer(string name)
        {
            return Customer.Create(name);
        }
       
        [Fact(DisplayName = "Customer Repository Update Should Succeed When Valid Data")]
        public void CustomerRepository_Update_Should_Succeed_When_Valid_Data()
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
                var actual = await customerRepository.Update(1, GivenCustomerDTO("Changed Name"));     
                
                var entity = db.Customers.Find(1);

                Assert.NotNull(entity);
                Assert.Equal("Changed Name", entity.Name);
                Assert.Equal(actual, entity);
            });                                    
        }

        [Fact(DisplayName = "Customer Repository Update Should Fail When Name Is Empty String")]
        public void CustomerRepository_Update_Should_Fail_When_Name_Is_Empty_String()
        {           
            _factory.CreateClient();            
            var logger = _factory.GetCustomerRepositoryLogger();

            _factory.SeedDbContext<GroceryStoreDbContext>(db =>
                {
                    db.Customers.Add(GivenCustomer("Original Test Name"));
                }
            );
                       
            _factory.WithDbContext<GroceryStoreDbContext>(db =>
            {
                var customerRepository = new CustomerRepository(db, logger);

                Assert.ThrowsAsync<ArgumentNullException>("name", async () => await customerRepository.Add(GivenCustomerDTO("")));

                var entity = db.Customers.Find(1);
                Assert.NotNull(entity);
                Assert.Equal("Original Test Name", entity.Name);
            });                                    
        }

        public void Dispose()
        {
            _factory.ClearInMemoryDatabase();
        }
    }
}
