using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat_field : Building
{
    public override void PassiveEffect()
    {
        base.PassiveEffect();
        //+5 mil
    }

    public bool CheckIfNearMil()
    {
        return false;
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
