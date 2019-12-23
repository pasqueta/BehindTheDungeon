using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpInChest : MonoBehaviour {
    [SerializeField]
    Sprite[] Weapon;

    public static int weaponCrafted = 0;
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update () {

        //GetComponent<Image>().color = new Color(1, 1, 1, 1);

        if (Peon.instance)
        {
            if (Peon.instance.hasCraftSmth)
            {
                image.enabled = true;
                image.sprite = Weapon[weaponCrafted];
                //timer += Time.deltaTime;
                //GetComponent<Image>().color = new Color(1, 1, 1, 1);
                //if (timer % 2 <= 1)
                //{
                //    GetComponent<Image>().color = new Color(1, 1, 1, 1);
                //}
                //else if (timer % 2 > 1)
                //{
                //    GetComponent<Image>().color = new Color(1, 1, 1, 0);
                //}
            }
            else
            {
                image.enabled = false;
                // GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
        else if(PeonNet.instance)
        {
            if (PeonNet.instance.hasCraftSmth)
            {
                image.enabled = true;
                image.sprite = Weapon[weaponCrafted];
            }
            else
            {
                image.enabled = false;
            }
        }
    }
}
