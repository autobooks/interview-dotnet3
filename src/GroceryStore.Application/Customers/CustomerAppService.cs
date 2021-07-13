namespace GroceryStore.Customers
{
    using System;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IRepository{Customer, Guid}"/>.</param>
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository) { }
    }
}
