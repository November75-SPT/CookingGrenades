using BepInEx.Configuration;

namespace CookingGrenades.Config;
internal static class ConfigManager
{
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
        #region RealisticFuseTime

        RealisticFuseTimeEnable = configFile.Bind(
            "1. Realistic Fuse Iime", 
            "Realistic Fuse Iime Enable", 
            true, 
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes {}));
        FuseTimeSpreadFactor = configFile.Bind(
            "1. Realistic Fuse Iime", 
            "Fuse Time Spread Factor", 
            0.0666666f, 
            new ConfigDescription(
                "", 
                new AcceptableValueRange<float>(0.001f, 2f),
                new ConfigurationManagerAttributes {}));       
                                        
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
            1000, 
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
            true, 
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
