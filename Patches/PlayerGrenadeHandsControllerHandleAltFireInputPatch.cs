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
using System.Linq;

namespace CookingGrenades.Patches
{
    public class PlayerGrenadeHandsControllerHandleAltFireInputPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.GrenadeHandsController), nameof(Player.GrenadeHandsController.HandleAltFireInput));
        }

        [PatchPostfix]
        public static void PatchPostfix(Player.GrenadeHandsController __instance, ref float __state)
        {
            var animator = __instance.FirearmsAnimator.Animator;
            var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();
            if (__instance.WaitingForHighThrow && !cookingTimer.IsCooking && Utils.AnimationUtils.IsRemovePullRingCompleted(animator))
            {
                cookingTimer.SetCookingItem(__instance.Item);
                if (!cookingTimer.IsCooking)
                {
                    // make sound
                    // find lever sound                                        
                    AnimationEventSystem.AnimationEvent leverEvent = null;
                    leverEvent = __instance.AnimationEventsEmitter._animationEventsStateBehaviours
                        // Only consider objects that are of type AnimationEventsStateBehaviour
                        .OfType<AnimationEventSystem.AnimationEventsStateBehaviour>()
                        // Each AnimationEventsStateBehaviour contains a list of AnimationEvents
                        // SelectMany flattens all those lists into a single sequence for easy searching
                        .SelectMany(x => x.AnimationEvents)
                        .FirstOrDefault(evt => evt._functionName == "Sound" && evt.Parameter.StringParam == "Lever");
                    if (leverEvent != null)
                    {
                        var currentState = animator.GetCurrentAnimatorStateInfo(1);
                        // add sound event 
                        __instance.AnimationEventsEmitter.ReceiveEvent(leverEvent, animator, currentState, currentState.normalizedTime);
                    }
                    else
                    {
                        Plugin.log.LogError($"Can't Find Sound Lever Event");
                    }

                    NotificationManagerClass.DisplayMessageNotification("Cook Start");
                    cookingTimer.StartCooking(__instance);
                    Plugin.log.LogInfo($"start cooking {cookingTimer.CookingStartTime}");
                }
            }
        }
    }
}


/* 3.11 spt

        public virtual void HandleAltFireInput()
        {
            if (WaitingForLowThrow)
            {
                LowThrow();
                return;
            }

            Class1153 currentOperation = CurrentOperation;
            if (!(currentOperation is Class1158))
            {
                if (currentOperation is Class1159 @class)
                {
                    @class.HandleAltFireInput();
                }
            }
            else
            {
                PullRingForLowThrow();
            }
        }


*/