using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Building
{
    public override void PassiveEffect()
    {
        WorldController.worldController.GetNamedEvent(EventType.DevilishWell);
    }
}
