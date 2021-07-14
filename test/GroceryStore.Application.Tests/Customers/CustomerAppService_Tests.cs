namespace GroceryStore.Customers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Shouldly;
    using Volo.Abp;
    using Volo.Abp.Application.Dtos;
    using Volo.Abp.Domain.Repositories;
    using Volo.Abp.Guids;
    using Volo.Abp.Validation;
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
        /// CustomerRepo the _repo..
        /// </summary>
        private readonly IRepository<Customer, Guid> _repo;

        /// <summary>
        /// Defines the _guidGenerator.
        /// </summary>
        private readonly IGuidGenerator _guidGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerAppService_Tests"/> class.
        /// </summary>
        public CustomerAppService_Tests()
        {
            _customerAppService = GetRequiredService<ICustomerAppService>();
            _repo = GetRequiredService<IRepository<Customer, Guid>>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        /// <summary>
        /// The Should_Return_Seeded_
        /// 3 customers where seeded. See GroceryStore.CustomerDataSeedContributor.
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
        }

        /// <summary>
        /// The Should_Create_Valid_Customer.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Create_Valid_Customer()
        {
            var input = new EditCustomerDto { LegacyId = 4, Name = "Pythagorus" };
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
            var dupException = await Assert.ThrowsAsync<DbUpdateException>(
                async () =>
                {
                    var duplicateLegacy = new EditCustomerDto
                    {
                        LegacyId = 1, // Bob is loaded with this key
                        Name = "Homer"
                    };
                    await _customerAppService.CreateAsync(duplicateLegacy);
                }
            );
            dupException.InnerException.Message.ShouldContain("Unique", Case.Insensitive);
        }

        /// <summary>
        /// The Should_Not_Create_Invalid_Customer_Model_Validation.
        /// Name must be under 128 char.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Not_Create_Invalid_Customer_Model_Validation()
        {
            var nameLengthException = await Assert.ThrowsAsync<AbpValidationException>(
                async () =>
                {
                    var longName = new EditCustomerDto
                    {
                        LegacyId = 5,
                        Name =
                            "0bnvp39wtc08noecmwsr8ng15sqv4fw0qe6dttmut4rgicfbqf5uokqjd33emhnvtkd5sw2qe9olp7th4i0px2zrvptcu8zohskniwoiygxgifijwr7do0jmdmsp68woo07pv5mbxxsue1b2cmgmy444n1tnk0umatd9wvf0xkm843i4id1a1sic5btaar4mmw88k2saoxvyccjzxhiqfylvuah5awn1gooir5uyx6qj6hf1pr6gz7mhlxb0k6o7dxlkfh1h11p0m5nkpckowstiw04m17ls6mmupw5mrya8s"
                    };
                    await _customerAppService.CreateAsync(longName);
                }
            );

            nameLengthException.ValidationErrors.ShouldContain(
                err => err.MemberNames.Any(mem => mem == "Name")
            );
        }

        /// <summary>
        /// The Should_Update_Valid_Customer.
        /// Changing bobs name is valid.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Update_Valid_Customer()
        {
            var customers = await _repo.GetListAsync();
            var bob = customers.Where(c => c.Name == "Bob").FirstOrDefault();
            var properBob = new EditCustomerDto { LegacyId = bob.LegacyId, Name = "Bob-thaniel" };
            var updatedBob = await _customerAppService.UpdateAsync(bob.Id, properBob);
            updatedBob.Name.ShouldContain("thaniel");
        }

        /// <summary>
        /// The Should_Not_Update_Invalid_Customer_DB_Constraints.
        /// Bob cannot re-use Mary's LegacyId.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Not_Update_Invalid_Customer_DB_Constraints()
        {
            var dupException = await Assert.ThrowsAsync<DbUpdateException>(
                async () =>
                {
                    var customers = await _repo.GetListAsync();
                    var bob = customers.Where(c => c.Name == "Bob").FirstOrDefault();
                    var mary = customers.Where(c => c.Name == "Mary").FirstOrDefault();

                    var improperBob = new EditCustomerDto
                    {
                        LegacyId = mary.LegacyId,
                        Name = "Bob-thaniel"
                    };
                    await _customerAppService.CreateAsync(improperBob);
                }
            );
            dupException.InnerException.Message.ShouldContain("Unique", Case.Insensitive);
        }

        /// <summary>
        /// The Should_Not_Update_Invalid_Customer_Model_Validation.
        /// Bob cannot change his name to be a cryptographic hash.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Not_Update_Invalid_Customer_Model_Validation()
        {
            var nameLengthException = await Assert.ThrowsAsync<AbpValidationException>(
                async () =>
                {
                    var customers = await _repo.GetListAsync();
                    var bob = customers.Where(c => c.Name == "Bob").FirstOrDefault();
                    var improperBob = new EditCustomerDto
                    {
                        LegacyId = bob.LegacyId,
                        Name =
                            "0bnvp39wtc08noecmwsr8ng15sqv4fw0qe6dttmut4rgicfbqf5uokqjd33emhnvtkd5sw2qe9olp7th4i0px2zrvptcu8zohskniwoiygxgifijwr7do0jmdmsp68woo07pv5mbxxsue1b2cmgmy444n1tnk0umatd9wvf0xkm843i4id1a1sic5btaar4mmw88k2saoxvyccjzxhiqfylvuah5awn1gooir5uyx6qj6hf1pr6gz7mhlxb0k6o7dxlkfh1h11p0m5nkpckowstiw04m17ls6mmupw5mrya8s"
                    };
                    await _customerAppService.UpdateAsync(bob.Id, improperBob);
                }
            );

            nameLengthException.ValidationErrors.ShouldContain(
                err => err.MemberNames.Any(mem => mem == "Name")
            );
        }

        /// <summary>
        /// The Should_Delete_Valid_Customer.
        /// Bob is a valid customer, it is valid to delete him.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Delete_Valid_Customer()
        {
            var customers = await _repo.GetListAsync();
            var bob = customers.Where(c => c.Name == "Bob").FirstOrDefault();

            await _customerAppService.DeleteAsync(bob.Id);
            var remainingCustomers = await _repo.GetListAsync();
            remainingCustomers.ShouldNotContain(c => c.Id == bob.Id);
        }

        /// <summary>
        /// The Should_Not_Delete_Invalid_Customer.
        /// Delete does not return anything when valid or invalid.
        /// TBO I've always thrown an exception when trying to delete that which doesnt exist. But apparently this is the industry standard.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Not_Delete_Invalid_Customer()
        {
            var initialCustomers = await _repo.GetListAsync();

            // This guid should not be taken
            var nextSeqGuid = _guidGenerator.Create();
            initialCustomers.FirstOrDefault(c => c.Id == nextSeqGuid).ShouldBeNull();

            // Delete nothing
            await _customerAppService.DeleteAsync(nextSeqGuid);

            var remainingCustomers = await _repo.GetListAsync();

            Enumerable.SequenceEqual(
                    initialCustomers.Select(c => JsonConvert.SerializeObject(c)),
                    remainingCustomers.Select(c => JsonConvert.SerializeObject(c))
                )
                .ShouldBeTrue();
        }

        [Fact]
        public async Task Should_fail_with_user_friendly_message()
        {
            var friendlyException = await Assert.ThrowsAsync<UserFriendlyException>(
                async () =>
                {
                    var longName = new EditCustomerDto
                    {
                        LegacyId = 5,
                        Name =
                            "0bnvp39wtc08noecmwsr8ng15sqv4fw0qe6dttmut4rgicfbqf5uokqjd33emhnvtkd5sw2qe9olp7th4i0px2zrvptcu8zohskniwoiygxgifijwr7do0jmdmsp68woo07pv5mbxxsue1b2cmgmy444n1tnk0umatd9wvf0xkm843i4id1a1sic5btaar4mmw88k2saoxvyccjzxhiqfylvuah5awn1gooir5uyx6qj6hf1pr6gz7mhlxb0k6o7dxlkfh1h11p0m5nkpckowstiw04m17ls6mmupw5mrya8s"
                    };
                    await _customerAppService.CreateAsync(longName);
                }
            );
        }
    }
}
