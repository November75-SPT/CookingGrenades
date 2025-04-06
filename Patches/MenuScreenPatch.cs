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
using EFT.UI;

namespace CookingGrenades.Patches;

public class MenuScreenPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
            return AccessTools.Method(typeof(MenuScreen), nameof(MenuScreen.method_8));
    }

    [PatchPostfix]
    public static void PatchPostfix(MenuScreen __instance)
    {
        if (!ConfigManager.UserWarningConfirmed.Value)
        {
            string message = "Cooking a grenade in real life is extremely dangerous. Without proper training, it can explode and cause death or serious injury. Do you understand?";
            EFT.UI.ItemUiContext.Instance.ShowMessageWindow(
                message,
                () => ConfigManager.UserWarningConfirmed.Value = true,
                () => ConfigManager.UserWarningConfirmed.Value = false,
                "Cooking Grenades Warning"
            );            
        }
    }
}