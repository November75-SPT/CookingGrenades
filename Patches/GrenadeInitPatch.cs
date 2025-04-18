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

namespace CookingGrenades.Patches;
public class GrenadeInitPatch : ModulePatch
{
    private static readonly FieldInfo ImpactFuseTimeField = AccessTools.Field(typeof(Grenade), "float_3");
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.DeclaredMethod(typeof(Grenade), nameof(Grenade.Init));
    }
    [PatchPostfix]
    public static void PatchPostfix(Grenade __instance, float timeSpent)
    {
        ImpactFuseTimeField.SetValue(__instance, timeSpent);
    }
}
