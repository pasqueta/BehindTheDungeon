using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftFillBar : MonoBehaviour {

    [SerializeField]
    TransformerUI transformerUI;

    [SerializeField]
    TransformerUINet transformerUINet;

    [SerializeField]
    Image fillImage;

    Text textCrafting;
    float craftingTime;
	// Use this for initialization
	void Start ()
    {
        textCrafting = GetComponentInChildren<Text>();
        if (Peon.instance)
        {
            craftingTime = Peon.instance.stats.craftingSpeed;
        }
        else if (PeonNet.instance)
        {
            craftingTime = 2.0f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Peon.instance)
        {
            GetComponent<Image>().enabled = true;
            fillImage.enabled = true;
            textCrafting.GetComponent<PulsingText>().enabled = enabled;
            textCrafting.enabled = true;

            if (transformerUI.isCrafting)
            {
                fillImage.fillAmount += Time.deltaTime / craftingTime;
            }
            else
            {
                fillImage.fillAmount = 0.0f;
                GetComponent<Image>().enabled = false;
                fillImage.enabled = false;
                textCrafting.enabled = false;
                textCrafting.GetComponent<PulsingText>().enabled = false;
            }
        }
        else if(PeonNet.instance)
        {
            GetComponent<Image>().enabled = true;
            fillImage.enabled = true;
            textCrafting.GetComponent<PulsingText>().enabled = enabled;
            textCrafting.enabled = true;

            if (transformerUINet.isCrafting)
            {
                fillImage.fillAmount += Time.deltaTime / craftingTime;
            }
            else
            {
                fillImage.fillAmount = 0.0f;
                GetComponent<Image>().enabled = false;
                fillImage.enabled = false;
                textCrafting.enabled = false;
                textCrafting.GetComponent<PulsingText>().enabled = false;
            }
        }
	}
}
