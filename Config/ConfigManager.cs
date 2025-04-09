using BepInEx.Configuration;

namespace CookingGrenades.Config;
internal static class ConfigManager
{
    public static ConfigEntry<bool> EnableCookingNotification;
    public static ConfigEntry<bool> ShowDefaultFuseTimeInInventoryUI;
    #region RealisticFuseTime
    public static ConfigEntry<bool> RealisticFuseTimeEnable;
    public static ConfigEntry<float> FuseTimeSpreadFactor;
    #endregion RealisticFuseTime
    
    #region RealisticFuseTimeTester
    public static ConfigEntry<float> TimeSimulationValue;
    public static ConfigEntry<int> FuseTimeTestCount;
    public static ConfigEntry<bool> TimeSimulationOutput;
    #endregion RealisticFuseTimeTester

    #region Debug
    public static ConfigEntry<bool> DebugGUI;
    public static ConfigEntry<bool> UserWarningConfirmed;
    #endregion Debug

    public static void Init(ConfigFile configFile)
    {
        EnableCookingNotification = configFile.Bind(
            "0. Cooking Grenades", 
            "Enable Cooking Notification", 
            false, 
            new ConfigDescription(
                "Show a notification when grenade cooking starts",
                null,
                new ConfigurationManagerAttributes {Order=1}));
        ShowDefaultFuseTimeInInventoryUI = configFile.Bind(
            "0. Cooking Grenades", 
            "Show Default Fuse Time In Inventory UI", 
            true, 
            new ConfigDescription(
                "If enabled, shows the default fuse time in inventory UI instead of randomized value.",
                null,
                new ConfigurationManagerAttributes {}));

                
        #region RealisticFuseTime

        RealisticFuseTimeEnable = configFile.Bind(
            "1. Realistic Fuse Time", 
            "Realistic Fuse Time Enable", 
            true, 
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes {Order=2}));
        FuseTimeSpreadFactor = configFile.Bind(
            "1. Realistic Fuse Time", 
            "Fuse Time Spread Factor", 
            0.085f, 
            new ConfigDescription(
                "Controls how much grenade fuse times vary (0.001 = almost fixed, 0.6 = wide range).", 
                new AcceptableValueRange<float>(0.001f, 0.6f),
                new ConfigurationManagerAttributes {Order=1}));       
                                        
        #endregion RealisticFuseTime
        
        #region RealisticFuseTimeTester
        
        TimeSimulationValue = configFile.Bind(
            "2. Realistic Fuse Iime Tester", 
            "Simulation Target Value", 
            5f, 
            new ConfigDescription(
                "The value you want to simulate",
                new AcceptableValueRange<float>(1f, 10f),
                new ConfigurationManagerAttributes {}));
        FuseTimeTestCount = configFile.Bind(
            "2. Realistic Fuse Iime Tester", 
            "Fuse Time Test Count", 
            10000, 
            new ConfigDescription(
                "Number of iterations for fuse time distribution test.",
                new AcceptableValueRange<int>(1, 100000),
                new ConfigurationManagerAttributes {}));
        TimeSimulationOutput = configFile.Bind(
            "2. Realistic Fuse Iime Tester", 
            "Time Simulation To Output", 
            false, 
            new ConfigDescription(
                "The simulation will run once when the value is set to true.\n"+
                "You can check the results in BepInEx/LogOutput.log.",
                null,
                new ConfigurationManagerAttributes {}));            
                                     
        #endregion RealisticFuseTimeTester

        
        #region Debug

        DebugGUI = configFile.Bind(
            "3. Debug", 
            "Enable Cooking Time GUI", 
            false, 
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes {}));
        UserWarningConfirmed = configFile.Bind(
            "3. Debug", 
            "User Warning Confirmed", 
            false, 
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes {IsAdvanced = true}));
        #endregion Debug
    }
}
