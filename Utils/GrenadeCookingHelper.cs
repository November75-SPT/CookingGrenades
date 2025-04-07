using EFT;
using UnityEngine;
using CookingGrenades.Config;
using System.Linq;

namespace CookingGrenades.Utils;
public static class GrenadeCookingHelper
{
    public static void StartCookingWithLeverSound(Player.GrenadeHandsController controller)
    {
        var animator = controller.FirearmsAnimator.Animator;
        var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();

        // Set the item to cook
        cookingTimer.SetCookingItem(controller.Item);
        // Find the lever sound event
        AnimationEventSystem.AnimationEvent leverEvent = controller.AnimationEventsEmitter._animationEventsStateBehaviours
            // Only consider objects that are of type AnimationEventsStateBehaviour
            .OfType<AnimationEventSystem.AnimationEventsStateBehaviour>()
            // Each AnimationEventsStateBehaviour contains a list of AnimationEvents
            // SelectMany flattens all those lists into a single sequence for easy searching
            .SelectMany(x => x.AnimationEvents)
            .FirstOrDefault(evt => evt._functionName == "Sound" && evt.Parameter.StringParam == "Lever");

        if (leverEvent != null)
        {
            // Get current animator state and trigger the lever sound
            var currentState = animator.GetCurrentAnimatorStateInfo(1);
            controller.AnimationEventsEmitter.ReceiveEvent(leverEvent, animator, currentState,currentState.normalizedTime);
        }
        else
        {
            Plugin.log.LogError("Can't Find Sound Lever Event");
        }

        if (ConfigManager.EnableCookingNotification.Value)
        {
            NotificationManagerClass.DisplayMessageNotification("Cooking Started");
        }
        cookingTimer.StartCooking(controller);
        Plugin.log.LogInfo($"start cooking {cookingTimer.CookingStartTime}");        
    }
}