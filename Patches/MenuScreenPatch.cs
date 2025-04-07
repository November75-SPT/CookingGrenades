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
using System.Linq;

namespace CookingGrenades.Patches;

public class MenuScreenPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(MainMenuControllerClass), nameof(MainMenuControllerClass.ShowScreen));
    }
    [PatchPostfix]
    public static void PatchPostfix(MainMenuControllerClass __instance)
    {
        if (!ConfigManager.UserWarningConfirmed.Value)
        {
            string message = "Cooking a grenade in real life is extremely dangerous. Without proper training, it can explode and cause death or serious injury.\n"+
                             "Do you understand?";
            EFT.UI.ItemUiContext.Instance.ShowMessageWindow(
                message,
                () => ConfigManager.UserWarningConfirmed.Value = true,
                () => ConfigManager.UserWarningConfirmed.Value = false,
                "Cooking Grenades Warning"
            );            
        }
    }
}