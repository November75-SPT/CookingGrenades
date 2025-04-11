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
using AnimationEventSystem;

namespace CookingGrenades.Patches;

public class BaseSoundPlayerOnSoundAtPointPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        // 60sec and 600call not good performance
        return AccessTools.Method(typeof(BaseSoundPlayer), nameof(BaseSoundPlayer.OnSoundAtPoint));
    }

    public static BaseSoundPlayer HaveToNotRunFuseSound = null;
    [PatchPrefix]
    public static bool PatchPrefix(BaseSoundPlayer __instance, string StringParam)
    {
        if (HaveToNotRunFuseSound == __instance && 
            StringParam == "SndFuse")
        {
            HaveToNotRunFuseSound = null;
            return false; // skip original
        }
        return true; // run original
    }
}



