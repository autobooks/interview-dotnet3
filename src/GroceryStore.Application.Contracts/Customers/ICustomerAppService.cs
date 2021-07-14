namespace GroceryStore.Customers
{
    using System;
    using System.Threading.Tasks;
    using Volo.Abp.Application.Dtos;
    using Volo.Abp.Application.Services;

    /// <summary>
    /// Defines the <see cref="ICustomerAppService" />.
    /// </summary>
    public interface ICustomerAppService
        : ICrudAppService<ViewCustomerDto, Guid, PagedAndSortedResultRequestDto, EditCustomerDto>
    {
        public Task ThisWillFailGracefullly();
    }
}
