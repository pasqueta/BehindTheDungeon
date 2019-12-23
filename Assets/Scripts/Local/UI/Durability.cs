using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Durability : MonoBehaviour
{
    [SerializeField]
    GameObject durabilityUI;

    [SerializeField]
    Image durability;
    

    void Update()
    {
        if(Boss.instance != null)
        {
            if (Boss.instance.HaveWeapon)
            {
                durabilityUI.SetActive(true);
                durability.fillAmount = ((float)Boss.instance.WeaponDurability / Boss.instance.WeaponDurabilityMax);
            }
            else
            {
                durabilityUI.SetActive(false);
            }            
        }
        else if (BossNet.instance != null)
        {
            if (BossNet.instance.HaveWeapon)
            {
                durabilityUI.SetActive(true);
                durability.fillAmount = ((float)BossNet.instance.WeaponDurability / BossNet.instance.WeaponDurabilityMax);
            }
            else
            {
                durabilityUI.SetActive(false);
            }
        }
    }
}
