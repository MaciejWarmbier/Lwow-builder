using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : Building
{
    public override void PassiveEffect()
    {
        if(CheckForNeighbor(Tile.TileType.Tree) > 0)
            VillageResources.villageResources.ChangeResourcesProduction(10);
        //+10 jesli obok drzewo
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
