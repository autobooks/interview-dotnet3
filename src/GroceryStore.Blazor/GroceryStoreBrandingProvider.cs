using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace GroceryStore.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class GroceryStoreBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "GroceryStore";
    }
}
