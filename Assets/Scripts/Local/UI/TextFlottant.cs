using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlottant : MonoBehaviour {

    public float timer = 2.0f;
    public float Y_Step = 0.2f;
	// Use this for initialization
	void Start () {
        //timer = 2.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;

        if (timer > 0.0f)
        {
            transform.localPosition += new Vector3(0.0f, Y_Step, 0.0f);

            if (GetComponentInChildren<Image>())
            {
                Color tempImageColor = GetComponentInChildren<Image>().color;
                tempImageColor.a = Mathf.Lerp(0.0f, 1.0f, timer / 2.0f);
                GetComponentInChildren<Image>().color = tempImageColor;
            }

            if (GetComponentInChildren<Text>())
            {
                Color tempTextColor = GetComponentInChildren<Text>().color;
                tempTextColor.a = Mathf.Lerp(0.0f, 1.0f, timer / 2.0f);
                GetComponentInChildren<Text>().color = tempTextColor;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
