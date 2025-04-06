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

namespace CookingGrenades.Patches;

public class GrenadeHandsControllermethod_2Patch : ModulePatch
{
    private static FieldInfo gparam_0FieldInfo = AccessTools.Field(typeof(Player.Class1152<Player.GrenadeHandsController>), "gparam_0");
    private static FieldInfo transform_1FieldInfo = AccessTools.Field(typeof(Player.BaseGrenadeHandsController), "transform_1");
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(Player.GrenadeHandsController.Class1156), nameof(Player.GrenadeHandsController.Class1156.method_2));
    }

    [PatchPrefix]
    public static bool PatchPrefix(Player.GrenadeHandsController.Class1156 __instance, bool low = false)
    {
        var gparam_0 = gparam_0FieldInfo.GetValue(__instance) as Player.GrenadeHandsController;
        var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();
        float cookingTime = 0f;
        if (cookingTimer.IsCooking)
        {
            cookingTime = cookingTimer.GetCookingTime();
            cookingTimer.Reset(gparam_0);                
        }

        var transform_1 = transform_1FieldInfo.GetValue(gparam_0) as Transform;

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