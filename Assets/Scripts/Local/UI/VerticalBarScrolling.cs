using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalBarScrolling : MonoBehaviour {

    Scrollbar laScrollBarerhino;
	// Use this for initialization
	void Start ()
    {
        laScrollBarerhino = GetComponent<Scrollbar>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Peon.instance)
        {
            if ((Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2))
            {
                laScrollBarerhino.value -= Input.GetAxisRaw(Peon.instance.Controller + "_RYaxis") / 10.0f;
            }
        }
        else if(PeonNet.instance)
        {
            if ((PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2))
            {
                laScrollBarerhino.value -= Input.GetAxisRaw(PeonNet.instance.Controller + "_RYaxis") / 10.0f;
            }
        }
	}
}
