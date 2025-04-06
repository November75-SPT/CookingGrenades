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
        return AccessTools.GetDeclaredMethods(typeof(MenuScreen))
                    .FirstOrDefault(m => m.ReturnType == typeof(void) &&
                                         m.GetParameters().Length == 1 &&
                                         m.GetParameters()[0].ParameterType == typeof(EMenuType));
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

//