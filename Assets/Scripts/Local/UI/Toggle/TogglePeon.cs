using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePeon : FixMouseInput
{

	// Use this for initialization
	void Start ()
    {
        if (Peon.instance)
        {
            controller = Peon.instance.Controller;
        }
        else if(PeonNet.instance)
        {
            controller = PeonNet.instance.Controller;
        }
	}
}
