using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("J1_A"))
            Debug.Log("type : button | name : J1_A");

        if (Input.GetButtonDown("J1_B"))
            Debug.Log("type : button | name : J1_B");

        if (Input.GetButtonDown("J1_X"))
            Debug.Log("type : button | name : J1_X");

        if (Input.GetButtonDown("J1_Y"))
            Debug.Log("type : button | name : J1_Y");

        if (Input.GetButtonDown("J1_LB"))
            Debug.Log("type : button | name : J1_LB");

        if (Input.GetButtonDown("J1_RB"))
            Debug.Log("type : button | name : J1_RB");

        if (Input.GetButtonDown("J1_Start"))
            Debug.Log("type : button | name : J1_Start");

        if (Input.GetButtonDown("J1_Back"))
            Debug.Log("type : button | name : J1_Back");

        if (Input.GetButtonDown("J1_LStick"))
            Debug.Log("type : button | name : J1_LStick");

        if (Input.GetButtonDown("J1_RStick"))
            Debug.Log("type : button | name : J1_RStick");


        if (Input.GetAxis("J1_LXaxis") > 0.1f)
            Debug.Log("type : axis | name : J1_LXaxis | value > 0.1f");
        else if (Input.GetAxis("J1_LXaxis") < -0.1f)
            Debug.Log("type : axis | name : J1_LXaxis | value < -0.1f");

        if (Input.GetAxis("J1_LYaxis") > 0.1f)
            Debug.Log("type : axis | name : J1_LYaxis | value > 0.1f");
        else if (Input.GetAxis("J1_LYaxis") < -0.1f)
            Debug.Log("type : axis | name : J1_LYaxis | value < -0.1f");

        if (Input.GetAxis("J1_RXaxis") > 0.1f)
            Debug.Log("type : axis | name : J1_RXaxis | value > 0.1f");
        else if (Input.GetAxis("J1_RXaxis") < -0.1f)
            Debug.Log("type : axis | name : J1_RXaxis | value < -0.1f");

        if (Input.GetAxis("J1_RYaxis") > 0.1f)
            Debug.Log("type : axis | name : J1_RYaxis | value > 0.1f");
        else if (Input.GetAxis("J1_RYaxis") < -0.1f)
            Debug.Log("type : axis | name : J1_RYaxis | value < -0.1f");

        if (Input.GetAxis("J1_PXaxis") > 0.1f)
            Debug.Log("type : axis | name : J1_PXaxis | value > 0.1f");
        else if (Input.GetAxis("J1_PXaxis") < -0.1f)
            Debug.Log("type : axis | name : J1_PXaxis | value < -0.1f");

        if (Input.GetAxis("J1_PYaxis") > 0.1f)
            Debug.Log("type : axis | name : J1_PYaxis | value > 0.1f");
        else if (Input.GetAxis("J1_PYaxis") < -0.1f)
            Debug.Log("type : axis | name : J1_PYaxis | value < -0.1f");

        if (Input.GetAxis("J1_LT") != 0)
            Debug.Log("type : axis | name : J1_LT | value != 0");

        if (Input.GetAxis("J1_RT") != 0)
            Debug.Log("type : axis | name : J1_RT | value != 0");
    }
}
