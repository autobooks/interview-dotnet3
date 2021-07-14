namespace GroceryStore.Customers
{
    using System;
    using System.Threading.Tasks;
    using Volo.Abp;
    using Volo.Abp.Application.Dtos;
    using Volo.Abp.Application.Services;
    using Volo.Abp.Domain.Repositories;

    /// <summary>
    /// Defines the <see cref="CustomerAppService" />.
    /// </summary>
    public class CustomerAppService
        : CrudAppService<
              Customer,
              ViewCustomerDto,
              Guid,
              PagedAndSortedResultRequestDto,
              EditCustomerDto
          >,
          ICustomerAppService
    {
        private readonly IRepository<Customer, Guid> _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IRepository{Customer, Guid}"/>.</param>
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
            _repo = repository;
        }

        /// <summary>
        /// The ThisWillFailGracefullly.
        /// This is for demo purposes. Abp implements unit of work around all its methods.
        /// Insert is valid. But an exception is thrown. The insert will not be committed.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ThisWillFailGracefullly()
        {
            var snoop = new Customer { LegacyId = 420, Name = "Snoop D-o-double-g" };
            await _repo.InsertAsync(snoop);

            throw new UserFriendlyException(
                "Five-oh! Snoop is a valid customer but we're throwing an exception which automatically rolls this back. Try get: /api/app/customer to verify."
            );
        }
    }
}
