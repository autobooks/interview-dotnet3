namespace GroceryStore.Users
{
    using System;
    using Volo.Abp.Domain.Entities.Auditing;
    using Volo.Abp.Users;

    /* This entity shares the same table/collection ("AbpUsers" by default) with the
     * IdentityUser entity of the Identity module.
     *
     * - You can define your custom properties into this class.
     * - You never create or delete this entity, because it is Identity module's job.
     * - You can query users from database with this entity.
     * - You can update values of your custom properties.
     */

    /// <summary>
	/// Defines the <see cref="AppUser" />.
	/// </summary>
    public class AppUser : FullAuditedAggregateRoot<Guid>, IUser
    {
        /* These properties are shared with the IdentityUser entity of the Identity module.
         * Do not change these properties through this class. Instead, use Identity module
         * services (like IdentityUserManager) to change them.
         * So, this properties are designed as read only!
         */

        /// <summary>
		/// Gets the TenantId.
		/// </summary>
        public virtual Guid? TenantId { get; private set; }

        /// <summary>
		/// Gets the UserName.
		/// </summary>
        public virtual string UserName { get; private set; }

        /// <summary>
		/// Gets the Name.
		/// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
		/// Gets the Surname.
		/// </summary>
        public virtual string Surname { get; private set; }

        /// <summary>
		/// Gets the Email.
		/// </summary>
        public virtual string Email { get; private set; }

        /// <summary>
		/// Gets a value indicating whether EmailConfirmed.
		/// </summary>
        public virtual bool EmailConfirmed { get; private set; }

        /// <summary>
		/// Gets the PhoneNumber.
		/// </summary>
        public virtual string PhoneNumber { get; private set; }

        /// <summary>
		/// Gets a value indicating whether PhoneNumberConfirmed.
		/// </summary>
        public virtual bool PhoneNumberConfirmed { get; private set; }

        /* Add your own properties here. Example:
         *
         * public string MyProperty { get; set; }
         *
         * If you add a property and using the EF Core, remember these;
         *
         * 1. Update GroceryStoreDbContext.OnModelCreating
         * to configure the mapping for your new property
         * 2. Update GroceryStoreEfCoreEntityExtensionMappings to extend the IdentityUser entity
         * and add your new property to the migration.
         * 3. Use the Add-Migration to add a new database migration.
         * 4. Run the .DbMigrator project (or use the Update-Database command) to apply
         * schema change to the database.
         */

        /// <summary>
		/// Prevents a default instance of the <see cref="AppUser"/> class from being created.
		/// </summary>
        private AppUser() { }
    }
}
