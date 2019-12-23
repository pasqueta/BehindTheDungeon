using UnityEngine.EventSystems;
using UnityEngine;

public class CustomEventSystem : EventSystem
{

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        EventSystem originalCurrent = EventSystem.current;
        current = this;
        base.Update();
        current = originalCurrent;
    }
}
