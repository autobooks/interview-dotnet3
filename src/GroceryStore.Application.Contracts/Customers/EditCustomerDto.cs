namespace GroceryStore.Customers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="EditCustomerDto" />.
    /// </summary>
    public class EditCustomerDto
    {
        /// <summary>
        /// Gets or sets the LegacyId.
        /// </summary>
        [Required]
        public int LegacyId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
    }
}
