using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeBackground_Label : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<RectTransform>().sizeDelta = new Vector2(text.text.Length * 30,60);
	}
}
