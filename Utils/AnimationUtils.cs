using System.Linq;
using System.Reflection;

namespace CookingGrenades.Utils;

public static class AnimationUtils
{
    public static readonly MethodInfo GetAnimStateMethod;
    static AnimationUtils()
    {
        GetAnimStateMethod = SPT.Reflection.Utils.PatchConstants.EftTypes
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .FirstOrDefault(m => m.Name == "GetAnimStateByNameHash" &&
                                    m.ReturnType == typeof(string) &&
                                    m.GetParameters().Length == 1 &&
                                    m.GetParameters()[0].ParameterType == typeof(int));
        if (GetAnimStateMethod == null)
        {
            Plugin.log.LogError("Failed to find GetAnimStateByNameHash in EftTypes");
        }
        else
        {
            // Plugin.log.LogInfo($"Found GetAnimStateByNameHash in {GetAnimStateMethod.DeclaringType.FullName}");
        }
    }

    /// <summary>
    /// Checks if the grenade pull ring removal animation is completed.
    /// Returns true if the current state is not "FIRE START" or "ALT FIRE START".
    /// </summary>
    public static bool IsRemovePullRingCompleted(IAnimator animator)
    {        
        // pulling pin off animation state is fullPathHash: 220670101, hash: -1433640516
        // 		{ -1433640516, "FIRE START" },
        // you can find StateByName in GClass736.GetCurrentAnimatorStateInfo()
        // also check WeaponAnimEventsQueueDebug

        // FIRE START | ALT FIRE START is pulling pin off
        // if complete then go to FIRE IDLE | ALT FIRE IDLE and loop in FIRE IDLE | ALT FIRE IDLE
        string currentStateName = (string)GetAnimStateMethod.Invoke(null, new object[] {animator.GetCurrentAnimatorStateInfo(1).shortNameHash});            
        return currentStateName != "ALT FIRE START" && currentStateName != "FIRE START";
    }
}
