namespace GroceryStore.Customers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Shouldly;
    using Volo.Abp.Application.Dtos;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="CustomerAppService_Tests" />.
    /// </summary>
    public class CustomerAppService_Tests : GroceryStoreApplicationTestBase
    {
        /// <summary>
        /// Defines the _customerAppService.
        /// </summary>
        private readonly ICustomerAppService _customerAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerAppService_Tests"/> class.
        /// </summary>
        public CustomerAppService_Tests()
        {
            _customerAppService = GetRequiredService<ICustomerAppService>();
        }

        /// <summary>
        /// The Should_Return_Seeded_Customers.
        /// 3 customers where seeded. See GroceryStore.Customers.CustomerDataSeedContributor.
        /// One of those customers is named Bob.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Return_Seeded_Customers()
        {
            var customers = await _customerAppService.GetListAsync(
                new PagedAndSortedResultRequestDto()
            );

            customers.TotalCount.ShouldBeGreaterThan(0);
            customers.Items.Where(c => c.Name.ToLower() == "Bob");
        }

        /// <summary>
        /// The Should_Create_Valid_Customer.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Create_Valid_Customer()
        {
            var input = new Customers.EditCustomerDto { LegacyId = 4, Name = "Pythagorus" };
            var customer = await _customerAppService.CreateAsync(input);
            customer.Id.ShouldNotBe(Guid.Empty);
        }

        /// <summary>
        /// Should_Not_Create_Invalid_Customer_DB_Constraints.
        /// Legacy Id is ak and must unique.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Not_Create_Invalid_Customer_DB_Constraints()
        {
            var expectedExceptions = 1;
            try
            {
                var duplicateLegacy = new Customers.EditCustomerDto
                {
                    LegacyId = 1, // Bob is loaded with this key
                    Name = "Homer"
                };
                var duplicateLegacyCustomer = await _customerAppService.CreateAsync(
                    duplicateLegacy
                );
            }
            catch (Exception)
            {
                expectedExceptions -= 1;
            }

            expectedExceptions.ShouldBe(0);
        }

        /// <summary>
        /// The Should_Not_Create_Invalid_Customer_Model_Validation.
        /// Name must be under 128 char.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Not_Create_Invalid_Customer_Model_Validation()
        {
            var expectedExceptions = 1;
            try
            {
                var longName = new Customers.EditCustomerDto
                {
                    LegacyId = 5,
                    Name =
                        "0bnvp39wtc08noecmwsr8ng15sqv4fw0qe6dttmut4rgicfbqf5uokqjd33emhnvtkd5sw2qe9olp7th4i0px2zrvptcu8zohskniwoiygxgifijwr7do0jmdmsp68woo07pv5mbxxsue1b2cmgmy444n1tnk0umatd9wvf0xkm843i4id1a1sic5btaar4mmw88k2saoxvyccjzxhiqfylvuah5awn1gooir5uyx6qj6hf1pr6gz7mhlxb0k6o7dxlkfh1h11p0m5nkpckowstiw04m17ls6mmupw5mrya8s"
                };
                var longNameCustomer = await _customerAppService.CreateAsync(longName);
            }
            catch (Exception)
            {
                expectedExceptions -= 1;
            }

            expectedExceptions.ShouldBe(0);
        }
    }
}
