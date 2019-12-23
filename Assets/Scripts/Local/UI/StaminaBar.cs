using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField]
    Image stamina;
	
	// Update is called once per frame
	void Update ()
    {
        if (Peon.instance != null)
        {
            stamina.fillAmount = Peon.instance.currentStamina / Peon.instance.maxStamina;
        }
        else if (PeonNet.instance != null)
        {
            stamina.fillAmount = PeonNet.instance.currentStamina / PeonNet.instance.maxStamina;
        }
    }
}
