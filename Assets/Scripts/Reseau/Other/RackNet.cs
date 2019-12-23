using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RackNet : NetworkBehaviour
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
        PeonNet.instance.hasCraftSmth = true;
        WeaponUpInChest.weaponCrafted = 0;
        GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
        GetComponentsInChildren<Transform>(true)[1 + swordIndex].GetComponent<ProgressiveDisparition>().StartDisparition = true;

        weaponList.Add(PeonNet.instance.GiveWeaponRecipe(0));
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);

        DisplayWeapons();
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
                Debug.Log("3");
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

        /*foreach (Transform tf in GetComponentsInChildren<Transform>(true))
        {
            if (tf.tag == "Weapon")
            {
                tf.gameObject.SetActive(false);
            }
        }

        if (swordIndex != 0)
        {
            GetComponentsInChildren<Transform>(true)[1 + swordIndex].gameObject.SetActive(true);
        }
        if (scytheIndex != 0)
        {
            GetComponentsInChildren<Transform>(true)[5 + scytheIndex].gameObject.SetActive(true);
        }
        if (hammerIndex != 0)
        {
            GetComponentsInChildren<Transform>(true)[9 + hammerIndex].gameObject.SetActive(true);
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (!BossNet.instance.isLocalPlayer && !isLocalPlayer)
        {
            return;
        }

        if (other.GetComponent<BossNet>())
        {
            BossNet.instance.nearOfInteractibleObject = true;
            if (weaponList.Count > 0)
            {
                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(true);

                if (((BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2) && (Input.GetButtonDown(BossNet.instance.Controller + "_A"))
                    || (BossNet.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.E))))
                {
                    
                    if (BossNet.instance.isServer)
                    {
                        BossNet.instance.RpcGetWeapon();
                    }
                    else
                    {
                        BossNet.instance.CmdGetWeapon();
                    }

                    foreach (WeaponRecipe weapon in weaponList)
                    {
                        BossNet.instance.AddWeapon(weapon);
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
                    PeonNet.instance.hasCraftSmth = false;
                    weaponList.Clear();
                }
            }
            else
            {
                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
            }
        }

        /*if (other.GetComponent<BossNet>())
        {
            if (weaponList.Count > 0)
            {
                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(true);

                if (((BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2) && (Input.GetButtonDown(BossNet.instance.Controller + "_A"))
                    || (BossNet.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.E))))
                {
                    if (BossNet.instance.isServer)
                    {
                        BossNet.instance.RpcGetWeapon();
                    }
                    else
                    {
                        BossNet.instance.CmdGetWeapon();
                    }
                    foreach (WeaponRecipe weapon in weaponList)
                    {
                        BossNet.instance.AddWeapon(weapon);
                        swordIndex = 0;
                        scytheIndex = 0;
                        hammerIndex = 0;
                        foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                        {
                            if (tf.tag == "Weapon")
                            {
                                tf.gameObject.SetActive(false);
                            }
                        }
                    }

                    PeonNet.instance.hasCraftSmth = false;
                    weaponList.Clear();
                }
            }
            else
            {
                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
            }
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boss")
        {
            BossNet.instance.nearOfInteractibleObject = false;
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
            CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
        }
    }
}
