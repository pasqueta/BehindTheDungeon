using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootedBags : MonoBehaviour
{
	void Update ()
    {
        if (Boss.instance)
        {
            GetComponent<Text>().text = "" + Boss.instance.NbMoneyBag;
        }
        else if(BossNet.instance)
        {
            GetComponent<Text>().text = "" + BossNet.instance.NbMoneyBag;
        }
	}
}
