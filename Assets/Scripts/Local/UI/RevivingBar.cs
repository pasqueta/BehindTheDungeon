using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevivingBar : MonoBehaviour
{
    [SerializeField]
    Image image;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Boss.instance)
        {
            image.fillAmount = ((float)Boss.instance.HealthPoint / (float)Boss.instance.BossPdvMax);
        }
        else if (BossNet.instance)
        {
            image.fillAmount = ((float)BossNet.instance.HealthPoint / (float)BossNet.instance.BossPdvMax);
        }
	}
}
