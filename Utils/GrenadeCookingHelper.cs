using EFT;
using UnityEngine;
using CookingGrenades.Config;
using System.Linq;
using System.Collections.Generic;

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
        var baseSoundPlayer = controller.ControllerGameObject.GetComponent<BaseSoundPlayer>();

        AnimationEventSystem.AnimationEvent fuseSoundEvent = controller.AnimationEventsEmitter._animationEventsStateBehaviours
            .OfType<AnimationEventSystem.AnimationEventsStateBehaviour>()
            .SelectMany(x => x.AnimationEvents)
            .FirstOrDefault(evt => evt._functionName == "SoundAtPoint" && evt.Parameter.StringParam == "SndFuse");

        bool shouldPlayAlternativeSound = fuseSoundEvent == null || 
            (ConfigManager.UseAlternativePinSound.Value && ShouldSkipFuseSound(controller.Item.StringTemplateId));
        // check fuse sound and play the ping sound. Similar one is "TripwirePin"
        if (shouldPlayAlternativeSound)
        {                       
            // there is no TripwirePin Event in current _animationEventsStateBehaviours so have to play on baseSoundPlayer          
            baseSoundPlayer.SoundEventHandler("TripwirePin");
        }
        // Otherwise, play fuze sound
        else 
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(1);
            controller.AnimationEventsEmitter.method_3(fuseSoundEvent, animator, currentState, fuseSoundEvent.Time);
        }

        // mark to disable fuze sound event after throw one time
        Patches.BaseSoundPlayerOnSoundAtPointPatch.HaveToNotRunFuseSound = baseSoundPlayer;
    }    
    
    private static readonly HashSet<string> SkipFuseSoundGrenades = new HashSet<string>
    {
        GrenadeIDs.GRENADE_M67_HAND,
        GrenadeIDs.GRENADE_V40_MINI,
        GrenadeIDs.GRENADE_M18_SMOKE_GRENADE_GREEN,
        GrenadeIDs.GRENADE_MODEL_7290_FLASH_BANG,
        GrenadeIDs.GRENADE_RDG2B_SMOKE
    };
    private static bool ShouldSkipFuseSound(string input)
    {
        return SkipFuseSoundGrenades.Contains(input);
    }

    // from server ItemTpl
    public static class GrenadeIDs
    {

        public const string  GRENADE_F1_HAND = "5710c24ad2720bc3458b45a3";
        public const string  GRENADE_M18_SMOKE_GRENADE_GREEN = "617aa4dd8166f034d57de9c5";
        public const string  GRENADE_M67_HAND = "58d3db5386f77426186285a0";
        public const string  GRENADE_MODEL_7290_FLASH_BANG = "619256e5f8af2c1a4e1f5d92";
        public const string  GRENADE_RDG2B_SMOKE = "5a2a57cfc4a2826c6e06d44a";
        public const string  GRENADE_RGD5_HAND = "5448be9a4bdc2dfd2f8b456a";
        public const string  GRENADE_RGN_HAND = "617fd91e5539a84ec44ce155";
        public const string  GRENADE_RGO_HAND = "618a431df1eb8e24b8741deb";
        public const string  GRENADE_V40_MINI = "66dae7cbeb28f0f96809f325";
        public const string  GRENADE_VOG17_KHATTABKA_IMPROVISED_HAND = "5e32f56fcb6d5863cc5e5ee4";
        public const string  GRENADE_VOG25_KHATTABKA_IMPROVISED_HAND = "5e340dcdcb6d5863cc5e5efb";
        public const string  GRENADE_ZARYA_STUN = "5a0c27731526d80618476ac4";
    }
}



/*      
-		[0]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDraw"	string
-		[1]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHolster"	string
-		[2]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndSafety"	string
-		[3]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndThrow"	string
-		[4]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndPin"	string
-		[5]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndLever"	string
-		[6]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDropBackpack"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000003]}	UnityEngine.AudioClip[]
-		[7]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndFuse"	string
+		SoundClips	{UnityEngine.AudioClip[0x00000003]}	UnityEngine.AudioClip[]
-		[8]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDefuseEnd"	string
-		[9]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndDefuseLoop"	string
-		[10]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePin"	string
-		[11]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlanting"	string
-		[12]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlantingConcrete"	string
-		[13]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlantingSoil"	string
-		[14]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwirePlantingWire"	string
-		[15]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwireUnplanting"	string
-		[16]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndTripwireUnplantingWire"	string
-		[17]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHands1"	string
-		[18]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHands2"	string
-		[19]	{BaseSoundPlayer.SoundElement}	BaseSoundPlayer.SoundElement
		EventName	"SndHands3"	string
*/