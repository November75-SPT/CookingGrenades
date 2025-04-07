namespace CookingGrenades.Utils;

public static class AnimationUtils
{
    
    // check finsh remove the pull ring animation
    public static bool IsRemovePullRingCompleted(IAnimator animator)
    {        
        // pulling pin off animation state is fullPathHash: 220670101, hash: -1433640516
        // 		{ -1433640516, "FIRE START" },
        // you can find StateByName in GClass736
        // also check WeaponAnimEventsQueueDebug

        // FIRE START | ALT FIRE START is pulling pin off
        // if complete then go to FIRE IDLE | ALT FIRE IDLE and loop in FIRE IDLE | ALT FIRE IDLE
        var currentStateName = GClass736.GetAnimStateByNameHash(animator.GetCurrentAnimatorStateInfo(1).shortNameHash);
        return currentStateName != "ALT FIRE START" && currentStateName != "FIRE START";
    }
}
