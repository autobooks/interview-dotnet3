namespace GroceryStore
{
    using AutoMapper;
    using GroceryStore.Customers;

    /// <summary>
    /// Defines the <see cref="GroceryStoreApplicationAutoMapperProfile" />.
    /// </summary>
    public class GroceryStoreApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroceryStoreApplicationAutoMapperProfile"/> class.
        /// </summary>
        public GroceryStoreApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Customer, ViewCustomerDto>();
            CreateMap<EditCustomerDto, Customers.Customer>();
        }
    }
}
