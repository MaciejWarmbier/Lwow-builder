using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingConfig;

public class Wheat_field : Building
{
    public override void PassiveEffect()
    {
        int mills = CheckForNeighbor(BuildingType.Mill);
        if (WorldController.worldController.isWheatBetter)
        {
            VillageResources.villageResources.ChangeFoodProduction(mills * 10);
        }
        else
        {
            VillageResources.villageResources.ChangeFoodProduction(mills * 5);
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
