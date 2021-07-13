namespace GroceryStore.Customers
{
    using System;
    using Volo.Abp.Application.Dtos;

    /// <summary>
    /// Defines the <see cref="ViewCustomerDto" />.
    /// </summary>
    public class ViewCustomerDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Gets or sets the LegacyId.
        /// </summary>
        public int LegacyId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }
    }
}
