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
            string message = 
            "[WARNING]\n"+
            "Cooking a grenade is extremely dangerous in real life. "+
            "Time-delay setting may vary and fuzes may function before prescribed times. "+
            "DO NOT “COOK OFF” the safety lever after pull ring with safety pin extraction. "+
            "This action can lead to premature detonation of the grenade leading to severe injury, death.\n"+
            "Do you acknowledge this warning and accept responsibility?";
            EFT.UI.ItemUiContext.Instance.ShowMessageWindow(
                message,
                () => ConfigManager.UserWarningConfirmed.Value = true,
                () => ConfigManager.UserWarningConfirmed.Value = false,
                "Cooking Grenades Warning"
            );            
        }
    }
}