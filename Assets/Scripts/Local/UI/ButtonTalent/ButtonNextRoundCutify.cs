using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextRoundCutify : MonoBehaviour {
        
    ColorBlock tempOriginalColor;

    private void Start()
    {
        tempOriginalColor = GetComponent<Toggle>().colors;
        GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnValueChange(); });
    }

    // Use this for initialization
    void OnValueChange () {

        Color color = new Color(0, 1, 0, 0.3f);
        ColorBlock cb = GetComponent<Toggle>().colors;
        
        if (!GetComponent<Toggle>().isOn)
        {
            cb.highlightedColor = color;
            GetComponent<Toggle>().colors = cb;
        }
        else
        {
            GetComponent<Toggle>().colors = tempOriginalColor;
        }
	}	
}
