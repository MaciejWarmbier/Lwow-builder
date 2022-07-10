using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_Hall : Building
{
    public override void PassiveEffect()
    {
        base.PassiveEffect();
        WorldController.worldController.isCityHallBuilt = true;
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
