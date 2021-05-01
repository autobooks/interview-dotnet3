using GroceryStoreAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroceryStoreAPI.Infrastructure
{
    class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> customerConfiguration)
        {
            customerConfiguration.ToTable("customers", GroceryStoreContext.DEFAULT_SCHEMA);
            customerConfiguration.HasKey(c => c.Id);

            customerConfiguration.Property<string>("Name").IsRequired();
        }
    }
}