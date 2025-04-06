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
using System.Linq;
using Comfort.Logs;

namespace CookingGrenades.Patches;

public class GrenadeHandsControllermethod_2Patch : ModulePatch
{
    private static FieldInfo gparam_0FieldInfo;
    private static FieldInfo transform_1FieldInfo = AccessTools.Field(typeof(Player.BaseGrenadeHandsController), "transform_1");
    protected override MethodBase GetTargetMethod()
    {
        // var type = HarmonyLib.AccessTools.FirstInner(typeof(EFT.Player), x => x.GetGenericArguments().FirstOrDefault(y => y.BaseType == typeof(EFT.Player.BaseGrenadeHandsController)) != null); ;
        // gparam_0FieldInfo = type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).FirstOrDefault(x => x.FieldType.BaseType == typeof(EFT.Player.BaseGrenadeHandsController));
        
        // Get all nested types from Player and find Class1152<>
        // Find the generic Class1152<> definition among Player's nested types
        // - We use GetNestedTypes with Public flag since Class1152<> is public
        // - Filter for a generic type definition with one generic argument where T's BaseType is BaseGrenadeHandsController
        var playerType = typeof(Player);
        var class1152GenericType = playerType.GetNestedTypes(BindingFlags.Public)
            .FirstOrDefault(t => t.IsGenericTypeDefinition &&  // Check if it's a generic type definition (e.g., Class1152<>)
                                 t.GetGenericArguments().Length == 1 &&  // Ensure it has exactly one generic parameter (T)
                                 t.GetGenericArguments()[0].BaseType == typeof(Player.BaseGrenadeHandsController));  // T's base type must be BaseGrenadeHandsController

        // Check if Class1152<> was not found and log an error
        if (class1152GenericType == null)
            Plugin.log.LogError($"Could not find class1152GenericType definition");

        // Get the GrenadeHandsController type, which is a nested type of Player
        var grenadeControllerType = typeof(Player.GrenadeHandsController);
        // Create a concrete type Class1152<GrenadeHandsController> by substituting T with GrenadeHandsController
        var concreteType = class1152GenericType.MakeGenericType(grenadeControllerType);

        // Find the gparam_0 field in Class1152<GrenadeHandsController>
        // - Use AccessTools.GetDeclaredFields to get all fields declared in the concreteType
        // - Filter for a field that:
        //   - Has FieldType matching GrenadeHandsController (exact type match)
        //   - Is protected (IsFamily)
        //   - Is readonly (IsInitOnly)
        gparam_0FieldInfo = AccessTools.GetDeclaredFields(concreteType)
            .FirstOrDefault(f => f.FieldType == grenadeControllerType && // Field type must be GrenadeHandsController
                                 f.IsFamily &&  // protected
                                 f.IsInitOnly);  // readonly

        // Check if gparam_0 was not found and log an error (optional, for debugging)
        if (gparam_0FieldInfo == null)
        {
            Plugin.log.LogError("Could not find gparam_0 field in Class1152<GrenadeHandsController>");
        }

        // Feels a bit overengineered, but I'll leave it in — respect for the effort 😓


        //Find the nested type in GrenadeHandsController that contains the EThrowState enum
        var targetMethod = HarmonyLib.AccessTools.FirstInner(typeof(EFT.Player.GrenadeHandsController), x => HarmonyLib.AccessTools.Inner(x,"EThrowState") != null);
        // Find the correct method inside that type
        return AccessTools.GetDeclaredMethods(targetMethod)
        .FirstOrDefault(m => m.ReturnType == typeof(void) &&
                             m.GetParameters().Length == 1 &&
                             m.GetParameters()[0].ParameterType == typeof(bool) &&
                             m.GetParameters()[0].HasDefaultValue);
    }

    [PatchPrefix]
    public static bool PatchPrefix(object __instance, bool low = false)
    {
        var gparam_0 = gparam_0FieldInfo.GetValue(__instance) as Player.GrenadeHandsController;
        var cookingTimer = CookingGrenades.GrenadeCookingManager.GetCookingTimer();
        float cookingTime = 0f;
        if (cookingTimer.IsCooking)
        {
            cookingTime = cookingTimer.GetCookingTime();
            cookingTimer.Reset(gparam_0);                
        }

        var transform_1 = transform_1FieldInfo.GetValue(gparam_0) as Transform;

        transform_1.gameObject.SetActive(value: false);
        gparam_0.vmethod_1(cookingTime, low);
        Plugin.log.LogInfo($"Player.GrenadeHandsController.Class1156.method_2 PatchPrefix");
        return false;
    }
}



/* 3.11 spt

            public void method_2(bool low = false)
            {
                gparam_0.transform_1.gameObject.SetActive(value: false);
                gparam_0.vmethod_1(0f, low);
            }

*/