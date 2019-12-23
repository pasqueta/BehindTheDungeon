using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rack : MonoBehaviour
{

    public List<WeaponRecipe> weaponList;
    [HideInInspector]
    public int swordIndex = 0;
    [HideInInspector]
    public int scytheIndex = 0;
    [HideInInspector]
    public int hammerIndex = 0;
    GameObject InversedUVGameObject;
    [HideInInspector]
    public WeaponRecipe.Weapon newWeaponDisplay = WeaponRecipe.Weapon.NONE;

    private void Start()
    {
        swordIndex = 1;
        weaponList = new List<WeaponRecipe>();
        InversedUVGameObject = GetComponentInChildren<MeshInverseUV>().gameObject;
        Peon.instance.hasCraftSmth = true;
        WeaponUpInChest.weaponCrafted = 0;
        GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
        GetComponentsInChildren<Transform>(true)[1 + swordIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;

        weaponList.Add(Peon.instance.GiveWeaponRecipe(0));
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);


    }

    public void DisplayWeapons()
    {
        switch (newWeaponDisplay)
        {
            case WeaponRecipe.Weapon.WOODSWORD:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Sword")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.WOODSCYTHE:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Scythe")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.WOODHAMMER:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Hammer")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].gameObject.SetActive(true);

                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.IRONSWORD:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Sword")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.IRONSCYTHE:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Scythe")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.IRONHAMMER:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Hammer")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].gameObject.SetActive(true);

                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.DIAMONDSWORD:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Sword")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Scythe")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.DIAMONDHAMMER:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Hammer")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].gameObject.SetActive(true);

                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.MITHRILSWORD:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Sword")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[1 + swordIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            case WeaponRecipe.Weapon.MITHRILSCYTHE:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Scythe")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[5 + scytheIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;

                break;
            case WeaponRecipe.Weapon.MITHRILHAMMER:
                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                {
                    if (tf.tag == "Hammer")
                    {

                        tf.gameObject.SetActive(false);
                    }
                }
                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].gameObject.SetActive(true);
                GetComponentsInChildren<Transform>(true)[9 + hammerIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                break;
            default:
                break;                
        }
        newWeaponDisplay = WeaponRecipe.Weapon.NONE;
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
        //foreach (Transform tf in GetComponentsInChildren<Transform>(true))
        //{
        //    if (tf.tag == "Weapon")
        //    {

        //        tf.gameObject.SetActive(false);
        //    }
        //}
        //if (swordIndex != 0)
        //{
        //    GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
        //}
        //if (scytheIndex != 0)
        //{
        //    GetComponentsInChildren<Transform>(true)[5 + scytheIndex].gameObject.SetActive(true);
        //}
        //if (hammerIndex != 0)
        //{
        //    GetComponentsInChildren<Transform>(true)[9 + hammerIndex].gameObject.SetActive(true);
        //}



    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Boss>())
        {
            Boss.instance.nearOfInteractibleObject = true;
            if (weaponList.Count > 0)
            {
                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
                CanvasManager.instance.GetActionTextBoss().SetActive(true);

                if (((Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2) && (Input.GetButtonDown(Boss.instance.Controller + "_A"))
                    || (Boss.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.E))))
                {
                    foreach (WeaponRecipe weapon in weaponList)
                    {
                        Boss.instance.AddWeapon(weapon);                       
                    }

                    if (Boss.instance.currentWeaponIndex == -1)
                    {
                        Boss.instance.NoWeaponGameObject.SetActive(false);
                        Boss.instance.currentWeaponIndex++;
                        if (Boss.instance.currentWeaponIndex == Boss.instance.InventoryWeapons.Count)
                            Boss.instance.currentWeaponIndex = 0;
                        Boss.instance.WeaponsGameObjects[(int)Boss.instance.InventoryWeapons[Boss.instance.currentWeaponIndex].weapon].SetActive(true);

                        if (Boss.instance.InventoryWeapons[Boss.instance.currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                            Boss.instance.InventoryWeapons[Boss.instance.currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                            Boss.instance.InventoryWeapons[Boss.instance.currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                        {
                            Boss.instance.MithrilWeaponSound();
                        }
                    }
                    swordIndex = 0;
                    scytheIndex = 0;
                    hammerIndex = 0;
                    foreach (Transform tf in GetComponentsInChildren<Transform>())
                    {
                        if (tf.tag == "Sword" || tf.tag == "Scythe" || tf.tag == "Hammer")
                        {
                            tf.gameObject.SetActive(false);

                        }
                    }
                    GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(false);
                    Peon.instance.hasCraftSmth = false;
                    weaponList.Clear();
                }
            }
            else
            {
                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
                CanvasManager.instance.GetActionTextBoss().SetActive(false);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boss")
        {
            Boss.instance.nearOfInteractibleObject = false;
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
            CanvasManager.instance.GetActionTextBoss().SetActive(false);
        }
    }
}