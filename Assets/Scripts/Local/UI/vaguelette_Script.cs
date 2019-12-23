using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vaguelette_Script : MonoBehaviour {

    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        image.transform.localPosition = new Vector3(0, -803 +788 * GetComponent<Image>().fillAmount, 0);
	}
}
