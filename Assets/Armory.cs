using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : Building
{
    public override bool CheckIfPossibleToBuild()
    {
        return WorldController.worldController.isArmoryUnlocked;
    }

    public override void PassiveEffect()
    {
        WorldController.worldController.GetNamedEvent(false, false, true);
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
