namespace GroceryStore.HttpApi.Client.ConsoleTestApp
{
    using System;
    using System.Threading.Tasks;
    using Volo.Abp.DependencyInjection;
    using Volo.Abp.Identity;

    /// <summary>
    /// Defines the <see cref="ClientDemoService" />.
    /// </summary>
    public class ClientDemoService : ITransientDependency
    {
        /// <summary>
        /// Defines the _profileAppService.
        /// </summary>
        private readonly IProfileAppService _profileAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDemoService"/> class.
        /// </summary>
        /// <param name="profileAppService">The profileAppService<see cref="IProfileAppService"/>.</param>
        public ClientDemoService(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        /// <summary>
        /// The RunAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task RunAsync()
        {
            var output = await _profileAppService.GetAsync();
            Console.WriteLine($"UserName : {output.UserName}");
            Console.WriteLine($"Email    : {output.Email}");
            Console.WriteLine($"Name     : {output.Name}");
            Console.WriteLine($"Surname  : {output.Surname}");
        }
    }
}