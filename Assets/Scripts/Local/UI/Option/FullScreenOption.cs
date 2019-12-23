using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenOption : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Toggle>().isOn = Screen.fullScreen;
    }

    public void OnFullScreen()
    {
        Screen.fullScreen = GetComponent<Toggle>().isOn;
    }
}
