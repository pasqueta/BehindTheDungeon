using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchHelpImage : MonoBehaviour {

    [SerializeField] Sprite bossen;
    [SerializeField] Sprite bossfr;
    [SerializeField] Sprite peonen;
    [SerializeField] Sprite peonfr;

    Image boss;
    Image peon;
    // Use this for initialization
    void Start () {
        boss = transform.GetChild(5).GetComponent<Image>();
        peon = transform.GetChild(6).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () {
        if (DataManager.instance.language == DataManager.LANGUAGE.ENGLISH)
        {
            boss.sprite = bossen;
            peon.sprite = peonen;
        }
        else if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
        {
            boss.sprite = bossfr;
            peon.sprite = peonfr;
        }

    }
}
