namespace GroceryStore
{
    using Volo.Abp.Threading;

    /// <summary>
	/// Defines the <see cref="GroceryStoreDtoExtensions" />.
	/// </summary>
    public static class GroceryStoreDtoExtensions
    {
        /// <summary>
		/// Defines the OneTimeRunner.
		/// </summary>
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        /// <summary>
		/// The Configure.
		/// </summary>
        public static void Configure()
        {
            OneTimeRunner.Run(
                () => {
                    /* You can add extension properties to DTOs
                 * defined in the depended modules.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */
                }
            );
        }
    }
}
