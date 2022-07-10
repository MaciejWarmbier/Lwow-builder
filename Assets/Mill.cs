using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building
{
    public override void PassiveEffect()
    {
        int wheats = CheckForWheat();
        if (WorldController.worldController.isWheatBetter)
        {
            VillageResources.villageResources.ChangeFoodProduction(wheats * 10);
        }
        else
        {
            VillageResources.villageResources.ChangeFoodProduction(wheats * 5);
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
