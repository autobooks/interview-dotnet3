using Volo.Abp.Settings;

namespace GroceryStore.Settings
{
    public class GroceryStoreSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(GroceryStoreSettings.MySetting1));
        }
    }
}
