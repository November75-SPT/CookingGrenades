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

namespace CookingGrenades.Patches
{
    public class PlayerGrenadeHandsControllerPatch : ModulePatch
    {
        // private static FieldInfo grenadeEmission_0FieldInfo = AccessTools.Field(typeof(Player.BaseGrenadeHandsController), "grenadeEmission_0");
        protected override MethodBase GetTargetMethod()
        {
            // Resolve Harmony ambiguity between Grenade.Init and base Init with parameterTypes
            Type[] parameterTypes = new Type[]
            {
                typeof(GrenadeSettings),              // settings
                typeof(string),                       // profileId
                typeof(ThrowWeapItemClass),           // throwWeap
                typeof(float),                        // timeSpent
                typeof(ISharedBallisticsCalculator),  // calculator
                typeof(bool)                          // isBeingPlanted
            };
            return AccessTools.Method(typeof(Grenade), nameof(Grenade.Init), parameterTypes);
        }

        [PatchPrefix]
        public static void PatchPrefix(Grenade __instance,float timeSpent, ref float __state)
        {
            // var grenadeEmission_0 = grenadeEmission_0FieldInfo.GetValue(__instance) as GrenadeEmission;

            Plugin.log.LogInfo($"Grenade {timeSpent}");

        }        
        [PatchPostfix]
        public static void PatchPostfix(Grenade __instance,float timeSpent, float __state)
        {
            Plugin.log.LogInfo($"Grenade {__instance.Player.Nickname} {timeSpent}");
        }
    }
}
