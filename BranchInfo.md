Remove way is fail
each grenade type share controller.AnimationEventsEmitter._animationEventsStateBehaviours

```c#
AnimationEventSystem.AnimationEventsStateBehaviour fuseEvent1 = controller.AnimationEventsEmitter._animationEventsStateBehaviours
    .OfType<AnimationEventSystem.AnimationEventsStateBehaviour>()
    .FirstOrDefault(x => x.AnimationEvents.FirstOrDefault(y => y._functionName == "SoundAtPoint" && y.Parameter.StringParam == "SndFuse") != null);
fuseEvent1.AnimationEvents.Remove(fuseEvent);
```