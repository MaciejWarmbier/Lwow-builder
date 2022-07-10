using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Building
{

    public override void PassiveEffect()
    {

        WorldController.worldController.GetNamedEvent(true, false, false);
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
