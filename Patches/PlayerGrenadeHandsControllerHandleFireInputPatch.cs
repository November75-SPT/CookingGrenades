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
using EFT.Animations;
using System.Linq;

namespace CookingGrenades.Patches;
public class PlayerGrenadeHandsControllerHandleFireInputPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(Player.GrenadeHandsController), nameof(Player.GrenadeHandsController.HandleFireInput));
    }

    [PatchPostfix]
    public static void PatchPostfix(Player.GrenadeHandsController __instance, ref float __state)
    {
        var animator = __instance.FirearmsAnimator.Animator;
        var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();
        if (__instance.WaitingForLowThrow && !cookingTimer.IsCooking && Utils.AnimationUtils.IsRemovePullRingCompleted(animator))
        {
            CookingGrenades.Utils.GrenadeCookingHelper.StartCookingWithLeverSound(__instance);
        }
    }       
}



/* 3.11 spt

        public virtual void HandleFireInput()
        {
            if (WaitingForHighThrow)
            {
                HighThrow();
                return;
            }

            Class1153 currentOperation = CurrentOperation;
            if (!(currentOperation is Class1158))
            {
                if (currentOperation is Class1159 @class)
                {
                    @class.HandleFireInput();
                }
            }
            else
            {
                PullRingForHighThrow();
            }
        }


*/