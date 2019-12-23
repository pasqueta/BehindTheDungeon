using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCurrentWeaponDisplay : MonoBehaviour
{
    // Use this for initialization
    Color test = new Color(0.83f, 0.83f, 0.83f, 1);
	void Start ()
    {
		if(GetComponent<Image>().sprite == null)
        {
            GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Boss.instance)
        {
            if (Boss.instance.HaveWeapon)
            {
                GetComponent<Image>().enabled = true;
                GetComponent<Image>().sprite = Boss.instance.CurrentWeapon.iconCarre;
            }
            else
            {
                GetComponent<Image>().enabled = false;
            }

            if (GetComponent<Image>().sprite != null)
            {
                GetComponent<Image>().color = test;
            }
        }
        else if(BossNet.instance)
        {
            if (BossNet.instance.HaveWeapon)
            {
                GetComponent<Image>().enabled = true;
                GetComponent<Image>().sprite = BossNet.instance.CurrentWeapon.iconCarre;
            }
            else
            {
                GetComponent<Image>().enabled = false;
            }

            if (GetComponent<Image>().sprite != null)
            {
                GetComponent<Image>().color = test;
            }
        }
    }
}
