using System;
using System.Collections.Generic;
using System.Linq;
using CookingGrenades.Config;

namespace CookingGrenades.Utils;
internal class FuseTimeTester
{
    public static void Init()
    {
        var timeSimulationOutput = ConfigManager.TimeSimulationOutput;
        timeSimulationOutput.SettingChanged += (sender, args) =>
        {
            if (timeSimulationOutput.Value)
            {
                RunFuseTimeTest(ConfigManager.TimeSimulationValue.Value);
                timeSimulationOutput.Value = false; // Run once and reset
            }
        };
    }
    public static void RunFuseTimeTest(float tsetValue)
    {
        float[] fuseTimes = new float[ConfigManager.FuseTimeTestCount.Value];
        Dictionary<float, int> fuseTimes2 = new Dictionary<float, int>();
        float minTime = float.MaxValue;
        float maxTime = float.MinValue;
        float sum = 0f;

        for (int i = 0; i < ConfigManager.FuseTimeTestCount.Value; i++)
        {
            float fuseTime = CookingGrenades.Utils.MathUtils.GenerateNormalRandomFast(tsetValue, tsetValue * ConfigManager.FuseTimeSpreadFactor.Value);
            fuseTimes[i] = fuseTime;

            float roundTime = (float)Math.Round(fuseTime, 1);
            if (fuseTimes2.ContainsKey(roundTime))
            {
                fuseTimes2[roundTime]++;
            }
            else
            {
                fuseTimes2.Add(roundTime, 1);
            }

            minTime = UnityEngine.Mathf.Min(minTime, fuseTime);
            maxTime = UnityEngine.Mathf.Max(maxTime, fuseTime);
            sum += fuseTime;

            // Plugin.log.LogInfo($"Fuse Time {i + 1}: {fuseTime:F3} seconds");
        }

        float average = sum / ConfigManager.FuseTimeTestCount.Value;
        float varianceSum = 0f;
        for (int i = 0; i < ConfigManager.FuseTimeTestCount.Value; i++)
        {
            varianceSum += UnityEngine.Mathf.Pow(fuseTimes[i] - average, 2);
        }
        float standardDeviation = UnityEngine.Mathf.Sqrt(varianceSum / ConfigManager.FuseTimeTestCount.Value);

        Plugin.log.LogInfo("=== Distribution Test Results ===");
        Plugin.log.LogInfo($"Target Value: {tsetValue},FuseTime Spread Factor: {ConfigManager.FuseTimeSpreadFactor.Value}, Test Count {ConfigManager.FuseTimeTestCount.Value}");
        Plugin.log.LogInfo($"Minimum Time: {minTime:F3} seconds");
        Plugin.log.LogInfo($"Maximum Time: {maxTime:F3} seconds");
        Plugin.log.LogInfo($"Average Time: {average:F3} seconds");
        Plugin.log.LogInfo($"Standard Deviation: {standardDeviation:F3} seconds");

        foreach (var (time, count) in fuseTimes2.OrderBy(x => x.Key))
        {
            Plugin.log.LogInfo($"Time: {time:F2}, Count: {count}");
        }
    }
}
