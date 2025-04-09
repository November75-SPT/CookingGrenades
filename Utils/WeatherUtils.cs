namespace CookingGrenades.Utils;

public static class WeatherUtils
{
    private const float StandardTemperature = 25.0f; // 기준 온도 (°C)
    private const float TempCoefficient = 0.0013f;   // 온도 계수 (초/°C)

    /// <summary>
    /// Adjusts fuse time based on current in-game temperature.
    /// </summary>
    /// <param name="fuseTime">Base fuse time without temperature adjustment.</param>
    /// <returns>Adjusted fuse time considering temperature.</returns>
    public static float AdjustFuseTimeForTemperature(float fuseTime, float temperature = 25.0f)
    {
        float tempDifference = temperature - StandardTemperature;
        float adjustmentFactor = 1 - (TempCoefficient * tempDifference);
        float adjustedFuseTime = fuseTime * adjustmentFactor;

        Plugin.log.LogInfo($"temperature: {temperature:F3}, FuseTime: {fuseTime:F5} => {adjustedFuseTime:F5}");
        return adjustedFuseTime;
    }
}
