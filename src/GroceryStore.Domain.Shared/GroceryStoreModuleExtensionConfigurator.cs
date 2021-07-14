namespace GroceryStore
{
    using Volo.Abp.Threading;

    /// <summary>
    /// Defines the <see cref="GroceryStoreModuleExtensionConfigurator" />.
    /// </summary>
    public static class GroceryStoreModuleExtensionConfigurator
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
                () =>
                {
                    ConfigureExistingProperties();
                    ConfigureExtraProperties();
                }
            );
        }

        /// <summary>
        /// The ConfigureExistingProperties.
        /// </summary>
        private static void ConfigureExistingProperties() { }

        /// <summary>
        /// The ConfigureExtraProperties.
        /// </summary>
        private static void ConfigureExtraProperties() { }
    }
}
