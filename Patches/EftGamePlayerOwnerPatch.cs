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

public class EftGamePlayerOwnerTranslateCommandPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(EftGamePlayerOwner), nameof(EftGamePlayerOwner.TranslateCommand));
    }

    [PatchPrefix]
    public static bool PatchPrefix(EftGamePlayerOwner __instance, ECommand command, ref InputNode.ETranslateResult __result)
    {
        // var tempCommand = command;
        // Utils.DebugDisplay.Instance.InsertDisplayObject($"{Time.time}", () => tempCommand);

        // Prevents reholstering the grenade while cooking
        if (CookingGrenades.GrenadeCookingManager.Timer.IsCooking)
        {
            switch (command)
            {
                case ECommand.ToggleInventory:
                case ECommand.SelectFirstPrimaryWeapon:
                case ECommand.SelectSecondPrimaryWeapon:
                case ECommand.SelectSecondaryWeapon:
                case ECommand.QuickSelectSecondaryWeapon:
                case ECommand.SelectFastSlot4:
                case ECommand.SelectFastSlot5:
                case ECommand.SelectFastSlot6:
                case ECommand.SelectFastSlot7:
                case ECommand.SelectFastSlot8:
                case ECommand.SelectFastSlot9:
                case ECommand.SelectFastSlot0:
                case ECommand.SelectKnife:
                case ECommand.QuickKnifeKick:
                case ECommand.Escape:
                    __result = InputNode.ETranslateResult.BlockAll;
                    return false;
            }
        }
        return true;
    }
}



