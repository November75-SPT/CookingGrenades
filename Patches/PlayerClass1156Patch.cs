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
using System.Linq;
using Comfort.Logs;

namespace CookingGrenades.Patches;

public class GrenadeHandsControllermethod_2Patch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        //Find the nested type in GrenadeHandsController that contains the EThrowState enum
        var targetMethod = HarmonyLib.AccessTools.FirstInner(typeof(EFT.Player.GrenadeHandsController), x => HarmonyLib.AccessTools.Inner(x,"EThrowState") != null);
        // Find the correct method inside that type
        return AccessTools.GetDeclaredMethods(targetMethod)
        .FirstOrDefault(m => m.ReturnType == typeof(void) &&
                             m.GetParameters().Length == 1 &&
                             m.GetParameters()[0].ParameterType == typeof(bool) &&
                             m.GetParameters()[0].HasDefaultValue);
    }

    [PatchPrefix]
    public static bool PatchPrefix(object __instance, bool low = false)
    {
        var gparam_0 = Traverse.Create(__instance).Field("gparam_0").GetValue<Player.GrenadeHandsController>();
        var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();
        float cookingTime = 0f;
        if (cookingTimer.IsCooking)
        {
            cookingTime = cookingTimer.GetCookingTime();
            cookingTimer.Reset(gparam_0);                
        }

        var transform_1 = Traverse.Create(gparam_0).Field("transform_1").GetValue<Transform>();
        transform_1.gameObject.SetActive(value: false);
        gparam_0.vmethod_1(cookingTime, low);
        Plugin.log.LogInfo($"Player.GrenadeHandsController.Class1156.method_2 PatchPrefix");
        return false;
    }
}



/* 3.11 spt

            public void method_2(bool low = false)
            {
                gparam_0.transform_1.gameObject.SetActive(value: false);
                gparam_0.vmethod_1(0f, low);
            }

*/