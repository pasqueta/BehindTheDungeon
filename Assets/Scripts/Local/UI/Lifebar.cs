using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{

    [SerializeField]
    Image life;
    

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Boss.instance != null)
        {
            life.fillAmount = (float)Boss.instance.HealthPoint / (float)Boss.instance.BossPdvMax;
        }
        else if (BossNet.instance != null)
        {
            life.fillAmount = (float)BossNet.instance.HealthPoint / (float)BossNet.instance.BossPdvMax;
        }
    }
}
