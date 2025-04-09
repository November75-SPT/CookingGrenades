namespace CookingGrenades.Config;
public static class ConfigEventHandler
{
    public static void Init()
    {
        ConfigManager.FuseTimeSpreadFactor.SettingChanged += (sender, args) =>
        {
            Patches.ThrowWeapItemClassGetExplDelayPatch.ExplDelay.Clear();
        };
    }
}
