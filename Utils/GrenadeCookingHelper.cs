using EFT;
using UnityEngine;
using CookingGrenades.Config;
using System.Linq;

namespace CookingGrenades.Utils;
public static class GrenadeCookingHelper
{
    public static void StartCookingWithLeverSound(Player.GrenadeHandsController controller)
    {
        var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();

        // Set the item to cook
        cookingTimer.SetCookingItem(controller);

        PlaySound(controller);

        if (ConfigManager.EnableCookingNotification.Value)
        {
            NotificationManagerClass.DisplayMessageNotification("Cooking Started");
        }
        cookingTimer.StartCooking(controller);
    }

    private static void PlaySound(Player.GrenadeHandsController controller)
    {
        var animator = controller.FirearmsAnimator.Animator;
        AnimationEventSystem.AnimationEvent fuseEvent = controller.AnimationEventsEmitter._animationEventsStateBehaviours
            .OfType<AnimationEventSystem.AnimationEventsStateBehaviour>()
            .SelectMany(x => x.AnimationEvents)
            .FirstOrDefault(evt => evt._functionName == "SoundAtPoint" && evt.Parameter.StringParam == "SndFuse");

        // If there's a fuse sound event, play it first.
        if (fuseEvent != null)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(1);
            controller.AnimationEventsEmitter.method_3(fuseEvent, animator, currentState, fuseEvent.Time);
        }
        else // Otherwise, play the ping sound. Similar one is "TripwirePin"
        {
            var baseSoundPlayer = controller.ControllerGameObject.GetComponent<BaseSoundPlayer>();
            baseSoundPlayer.SoundEventHandler("TripwirePin");
        }
        // mark to disable fuze sound event after throw one time
        Patches.BaseSoundPlayerOnSoundAtPointPatch.HaveToNotRunFuseSound = controller.ControllerGameObject.GetComponent<BaseSoundPlayer>();
    }
}




/*      

-		[0]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDraw"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[1]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHolster"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[2]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndSafety"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[3]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndThrow"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[4]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndPin"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[5]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndLever"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[6]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDropBackpack"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000003]}	UnityEngine.AudioClip[]
-		[7]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndFuse"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000003]}	UnityEngine.AudioClip[]
-		[8]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDefuseEnd"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[9]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDefuseLoop"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[10]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePin"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[11]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlanting"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[12]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlantingConcrete"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[13]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlantingSoil"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[14]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlantingWire"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[15]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwireUnplanting"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[16]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwireUnplantingWire"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[17]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHands1"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[18]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHands2"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]
-		[19]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHands3"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000001]}	UnityEngine.AudioClip[]


*/