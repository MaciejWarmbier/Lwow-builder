using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peruns_altar : Building
{
    public override bool CheckIfPossibleToBuild()
    {
        return WorldController.worldController.isPerunActivated;
    }

    public override void PassiveEffect()
    {
        if (WorldController.worldController.isPerunHappy)
        {
             WorldController.worldController.GetNamedEvent(false, true, false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
