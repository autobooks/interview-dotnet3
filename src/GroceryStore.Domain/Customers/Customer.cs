namespace GroceryStore.Customers
{
    using System;
    using Volo.Abp.Domain.Entities.Auditing;

    /// <summary>
    /// Defines the <see cref="Customer" />.
    /// </summary>
    public class Customer : AuditedAggregateRoot<Guid>
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