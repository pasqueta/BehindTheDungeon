using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lootingBar : MonoBehaviour
{
    //------------SERIALIZE FIELDS------------//
    [SerializeField]
    Image LootBar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Boss.instance != null)
        {
            if (Boss.instance.DtLoot > 0.01f)
            {
                LootBar.transform.parent.gameObject.SetActive(true);
                LootBar.fillAmount = Boss.instance.DtLoot / Boss.instance.LootTime;
            }
            else
            LootBar.transform.parent.gameObject.SetActive(false);
        }
        //version network
        else if (BossNet.instance != null)
        {
            if (BossNet.instance.DtLoot > 0.01f)
            {
                LootBar.transform.parent.gameObject.SetActive(true);
                LootBar.fillAmount = BossNet.instance.DtLoot / BossNet.instance.LootTime;
            }
            else
            {
                LootBar.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
