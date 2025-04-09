using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using UnityEngine;

namespace CookingGrenades;
public class GrenadeCookingTimer 
{
    private float _cookingStartTime = 0f;
    public float CookingStartTime => _cookingStartTime;
    public bool IsCooking => _cookingStartTime > 0f && CookingItem != null;
    public ThrowWeapItemClass CookingItem { get; private set; }
    public Coroutine existingCoroutine =null;
    public GrenadeCookingTimer()
    {
        CookingItem = null;
    }
    public GrenadeCookingTimer(ThrowWeapItemClass item)
    {
        CookingItem = item;
    }

    public void SetCookingItem(ThrowWeapItemClass item)
    {
        CookingItem = item;
        _cookingStartTime = 0f;
    }
    public void StartCooking(Player.GrenadeHandsController controller)
    {
        _cookingStartTime = Time.time;
        existingCoroutine = controller.StartCoroutine(ForceThrowCoroutine(controller));

        if (Config.ConfigManager.DebugGUI.Value)
        {
            CookingGrenades.Utils.DebugDisplay.Instance.InsertDisplayObject($"Cooking Time ", () => 
            { 
                var player = Singleton<GameWorld>.Instance.MainPlayer;
                var explDelay = 0f;
                if (player.HandsController.Item is ThrowWeapItemClass throwWeapItemClass)
                {
                    explDelay = throwWeapItemClass.GetExplDelay;
                }
                return $"{GetCookingTime():F3}/{explDelay:F3} sec ({GetCookingTime() / explDelay * 100:F1}%)"; 
            });            
        }
    }

    public float GetCookingTime()
    {
        return IsCooking ? Time.time - _cookingStartTime : 0f;
    }

    public void Reset(Player.GrenadeHandsController oldController)
    {
        CookingItem = null;
        _cookingStartTime = 0f;
        oldController.StopCoroutine(existingCoroutine);
        oldController = null;
    }


    private IEnumerator ForceThrowCoroutine(Player.GrenadeHandsController controller)
    {
        Plugin.log.LogInfo("StartForceThrowAfterDelay " + controller.GetInstanceID() + $" Wait: {controller.Item.GetExplDelay}");
        yield return new WaitForSeconds(controller.Item.GetExplDelay - 0.44f); // throw 0.44second little faster, about 0.48 is animation throw time

        if (IsCooking && controller != null )
        {
            Plugin.log.LogInfo($"StartForceThrowAfterDelay {controller.GetInstanceID()} main:{Singleton<GameWorld>.Instance.MainPlayer.HandsController.GetInstanceID()}");
            ForceThrow(controller);
        }
        else
        {
            Plugin.log.LogInfo($"StartForceThrowAfterDelay else {controller.GetInstanceID()} main:{Singleton<GameWorld>.Instance.MainPlayer.HandsController.GetInstanceID()}");
        }
    }

    private void ForceThrow(Player.GrenadeHandsController controller)
    {
        MethodInfo method = controller.WaitingForHighThrow
            ? AccessTools.Method(typeof(Player.GrenadeHandsController), nameof(Player.GrenadeHandsController.HandleFireInput))
            : AccessTools.Method(typeof(Player.GrenadeHandsController), nameof(Player.GrenadeHandsController.HandleAltFireInput));
        // maybe EftGamePlayerOwner.TranslateCommand() is better?

        if (method != null)
        {
            method.Invoke(controller, null);
            Plugin.log.LogInfo($"Forced throw for {CookingItem.Id} after 5 seconds");
        }
        else
        {
            Plugin.log.LogError("Throw method not found");
        }
    }
}


// Was thinking of managing it with a collection,
// but switched to a singleton due to memory concerns.
public static class GrenadeCookingManager
{
    private static GrenadeCookingTimer _timer = new GrenadeCookingTimer();
    public static GrenadeCookingTimer Timer => _timer;

    public static GrenadeCookingTimer GetCookingTimer()
    {
        return _timer;
    }
}
