using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBagTextNet : MonoBehaviour
{
    float minSize = 0.45f, maxSize = 0.6f;
    float lerpTime = 0.6f;
    float currentDt = 0.5f;
    bool rise = true;
    Text[] text;

    // Use this for initialization
    void Start()
    {
        text = GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ChestBossNet.instance)
        {
            text[0].text = "" + ChestBossNet.instance.NbMoneyBag;

            if (ChestBossNet.instance.NbMoneyBag <= 2)
            {
                currentDt += Time.deltaTime;

                if (currentDt >= lerpTime)
                {
                    currentDt = 0.0f;
                    rise = !rise;
                }

                if (ChestBossNet.instance.NbMoneyBag <= 1 && DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
                {
                    text[1].text = "restant";
                }
                else
                {
                    text[1].text = "restants";
                }

                if (ChestBossNet.instance.NbMoneyBag <= 0)
                {
                    text[0].color = new Color(200 / 255f, 48 / 255f, 48 / 255f); //   C83030FF
                    text[1].color = new Color(200 / 255f, 48 / 255f, 48 / 255f);
                }
                else
                {
                    text[0].color = new Color(204 / 255f, 204 / 255f, 204 / 255f); //   CCCCCCFF
                    text[1].color = new Color(204 / 255f, 204 / 255f, 204 / 255f);
                }
            }
        }

        if (rise)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, currentDt / lerpTime);
        }
        else
        {
            transform.localScale = Vector3.one * Mathf.Lerp(maxSize, minSize, currentDt / lerpTime);
        }
    }

}
