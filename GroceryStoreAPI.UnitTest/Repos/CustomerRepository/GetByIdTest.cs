using System;
using System.Linq;
using Xunit;
using GroceryStoreAPI.Domain;

namespace GroceryStoreAPI.UnitTest.Repos.CustomerRepo
{
    public class GetByIdTest:IClassFixture<TestEnvironmentWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly TestEnvironmentWebApplicationFactory<Startup> _factory;
        public GetByIdTest(TestEnvironmentWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private Customer GivenCustomer(string name)
        {
            return Customer.Create(name);
        }

        [Fact(DisplayName = "Customer Repository Get By Id Should Succeed And Return Correct Record")]
        public void CustomerRepository_GetById_Should_Succeed_And_Return_Correct_Record()
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
                var actual = await customerRepository.GetById(3);

                Assert.NotNull(actual);                
                Assert.Equal("Test Name 3", actual.Name);
            });                 
        }

        [Fact(DisplayName = "Customer Repository Get By Id Should Fail When Invalid Id")]
        public void CustomerRepository_GetById_Should_Fail_When_Invalid_Id()
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
                var actual = await customerRepository.GetById(4);

                Assert.Null(actual);                                
            });                 
        }

        [Fact(DisplayName = "Customer Repository Get By Id Should Fail When No Records")]
        public void CustomerRepository_GetById_Should_Fail_When_No_Records()
        {           
            _factory.CreateClient();
            var logger = _factory.GetCustomerRepositoryLogger();                       
            
            _factory.WithDbContext<GroceryStoreDbContext>(async db =>
            {
                var customerRepository = new CustomerRepository(db, logger);
                var actual = await customerRepository.GetById(1);

                Assert.Null(actual);                                
            });                 
        }
               
        public void Dispose()
		{		
			try { _factory.ClearInMemoryDatabase(); } catch { }
		}        
    }     
}
