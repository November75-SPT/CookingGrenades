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

namespace CookingGrenades.Patches
{
    public class PlayerGrenadeHandsControllerHandleFireInputPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.GrenadeHandsController), nameof(Player.GrenadeHandsController.HandleFireInput));
        }

        [PatchPostfix]
        public static void PatchPostfix(Player.GrenadeHandsController __instance, ref float __state)
        {
            // pulling pin off animation state is fullPathHash: 220670101, hash: -1433640516
            // 		{ -1433640516, "FIRE START" },
            // you can find StateByName in GClass736
            // also check WeaponAnimEventsQueueDebug
            var animator = __instance.FirearmsAnimator.Animator;

            // from WeaponAnimEventsQueueDebug.OnGUI
            // __instance.AnimationEventsEmitter.EventsSequenceData.AnimationEventsDebugQueue;       
            // foreach (AnimationEventSystem.AnimationEventsSequenceData.GStruct115 item in __instance.AnimationEventsEmitter.EventsSequenceData.AnimationEventsDebugQueue)
            // {
                
            // }
            // there is item.EventName is sound but i don't know how to do that


            var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();
            if (__instance.WaitingForLowThrow && !cookingTimer.IsCooking && Utils.AnimationUtils.IsRemovePullRingCompleted(animator))
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