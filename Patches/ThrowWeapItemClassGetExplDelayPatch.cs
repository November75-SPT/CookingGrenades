using System;
using System.Reflection;
using SPT.Reflection.Patching;
using EFT.UI.Map;
using HarmonyLib;
using EFT.UI.WeaponModding;
using UnityEngine;
using System.Collections.Generic;
using CookingGrenades.Config;
using System.Threading.Tasks;
using EFT;
using SPT.Custom.Utils;
using EFT.InputSystem;
using CookingGrenades.Utils;

namespace CookingGrenades.Patches;

public class ThrowWeapItemClassGetExplDelayPatch : ModulePatch
{
    // Used WeakReference to avoid memory leaks and allow garbage collection.
    static Dictionary<WeakReference<ThrowWeapItemClass>, float> _explDelay = new Dictionary<WeakReference<ThrowWeapItemClass>, float>(new WeakReferenceComparer());
    public static Dictionary<WeakReference<ThrowWeapItemClass>, float> ExplDelay => _explDelay;
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.PropertyGetter(typeof(ThrowWeapItemClass), nameof(ThrowWeapItemClass.GetExplDelay));
    }
    static HashSet<string> uiTypes = new HashSet<string>
    {
        typeof(EFT.UI.InfoWindow).FullName,
        typeof(EFT.UI.ItemUiContext).FullName,
        typeof(EFT.UI.ItemSpecificationPanel).FullName,
        typeof(EFT.UI.CompactCharacteristicPanel).FullName
    };
    [PatchPostfix]
    public static void PatchPostfix(ThrowWeapItemClass __instance, ref float __result)
    {
        if (ConfigManager.RealisticFuseTimeEnable.Value)
        {
            
            // ignore when call from iventory view
            if (ConfigManager.ShowDefaultFuseTimeInInventoryUI.Value)
            {
                var stackTrace = new System.Diagnostics.StackTrace(2, false);
                for (int i = 0; i < stackTrace.FrameCount; i++)
                {                
                    if (uiTypes.Contains(stackTrace.GetFrame(i).GetMethod().DeclaringType.FullName))
                    {
                        return;
                    }            
                }
            }

            var key = new WeakReference<ThrowWeapItemClass>(__instance);
            if (_explDelay.TryGetValue(key, out var existDelay))
            {
                __result = existDelay;
            }
            else
            {
                var delay = MathUtils.GenerateNormalRandomBoxMuller(__result, __result * ConfigManager.FuseTimeSpreadFactor.Value);
                // Plugin.log.LogInfo($"__result: {__result:F1}, FuseTimeSpreadFactor: {ConfigManager.FuseTimeSpreadFactor.Value}, Delay: {delay}");
                _explDelay.Add(key, delay);
                __result = delay;
            }
        }
    }
    public class WeakReferenceComparer : IEqualityComparer<WeakReference<ThrowWeapItemClass>>
    {
        public bool Equals(WeakReference<ThrowWeapItemClass> x, WeakReference<ThrowWeapItemClass> y)
        {
            return x.TryGetTarget(out var xTarget) && y.TryGetTarget(out var yTarget) && xTarget == yTarget;
        }

        public int GetHashCode(WeakReference<ThrowWeapItemClass> obj)
        {
            return obj.TryGetTarget(out var target) ? target.GetHashCode() : 0;
        }
    }
}