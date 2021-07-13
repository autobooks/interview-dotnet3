using GroceryStore.Customers;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GroceryStore.EntityFrameworkCore
{
    public static class GroceryStoreDbContextModelCreatingExtensions
    {
        public static void ConfigureGroceryStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            builder.Entity<Customer>(
                b =>
                {
                    b.ToTable("Customers", GroceryStoreConsts.DbSchemaApp);
                    b.ConfigureByConvention();

                    b.HasAlternateKey(n => n.LegacyId);
                    b.Property(n => n.Name).IsRequired().HasMaxLength(255);
                }
            );
        }
    }
}
