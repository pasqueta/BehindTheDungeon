using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBoss : FixMouseInput {

    // Use this for initialization
    void Start()
    {
        if (Boss.instance)
        {
            controller = Boss.instance.Controller;
        }
        else if(BossNet.instance)
        {
            controller = BossNet.instance.Controller;
        }
    }
}
