using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour {

	public enum ButtonsDirection
    {
        droite,
        gauche
    }

    public void onButtonClick(int aled)
    {
        if (aled == 0)
            Debug.Log("Tamere");
        else
            Debug.Log("TonPere");
    }
}
