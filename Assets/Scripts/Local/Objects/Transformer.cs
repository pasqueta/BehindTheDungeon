using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Transformer : MonoBehaviour
{

    public UtilityStruct.MINERALS acquiredMinerals;

    [HideInInspector]
    public bool isUIActive;

    [SerializeField]
    AudioSource clearInventory;

    [SerializeField]
    Canvas transformerCanvasSplitScreen;

    GameObject InversedUVGameObject;

    CameraController cameraController;

    TransformerUI transformerUI;

    bool init = false;

    void Init()
    {
        init = true;
        cameraController = CameraController.camPeon;

        InversedUVGameObject = GetComponentInChildren<MeshInverseUV>().gameObject;

        if (!cameraController)
        {
            init = false;
        }

        transformerUI = transformerCanvasSplitScreen.GetComponent<TransformerUI>();
        transformerCanvasSplitScreen.gameObject.SetActive(true);

        acquiredMinerals.wood = 10;
        acquiredMinerals.iron = 10;
        acquiredMinerals.diamond = 5;
        acquiredMinerals.mithril = 0;
        acquiredMinerals.bones = 0;
        acquiredMinerals.dust = 0;

        isUIActive = false;
    }

    private void Update()
    {
        if (!init)
        {
            Init();
        }

        if (DataManager.instance.isUiActive != isUIActive)
        {
            DataManager.instance.isUiActive = isUIActive;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Peon")
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
            CanvasManager.instance.GetActionTextPeon().SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Peon")
        {
            collision.GetComponent<Peon>().enabled = true;
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
            CanvasManager.instance.GetActionTextPeon().SetActive(false);
            transformerCanvasSplitScreen.GetComponent<Canvas>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!isUIActive)
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
            CanvasManager.instance.GetActionTextPeon().SetActive(true);
        }

        if (collision.transform.tag == "Peon")
        {
            //Local
            if (Peon.instance != null && !WinLooseManager.instance.gameIsAlreadyEnd)
            {
                PeonGiveMinerals();
                //clavier
                if (Peon.instance.Controller == Controller.K1)
                {
                    if (isUIActive && Input.GetKeyDown(KeyCode.Escape) && !transformerUI.isCrafting)
                    {
                        isUIActive = false;
                        CameraController.camPeon.EnableRotation = true;

                        transformerCanvasSplitScreen.GetComponent<Canvas>().enabled = false;
                        InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
                        CanvasManager.instance.GetActionTextPeon().SetActive(true);
                        Peon.instance.isBusy = false;

                        transformerUI.boolEnteringMenu = true;

                        //Apparition Magique
                        switch (transformerUI.lastWeaponCrafted)
                        {
                            case WeaponRecipe.Weapon.WOODSWORD:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Sword")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.WOODSCYTHE:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Scythe")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.WOODHAMMER:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Hammer")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.IRONSWORD:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Sword")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.IRONSCYTHE:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Scythe")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.IRONHAMMER:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Hammer")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.DIAMONDSWORD:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Sword")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Scythe")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.DIAMONDHAMMER:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Hammer")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.MITHRILSWORD:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Sword")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            case WeaponRecipe.Weapon.MITHRILSCYTHE:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Scythe")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;

                                break;
                            case WeaponRecipe.Weapon.MITHRILHAMMER:
                                foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                {
                                    if (tf.tag == "Hammer")
                                    {

                                        tf.gameObject.SetActive(false);
                                    }
                                }
                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);
                                GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                break;
                            default:
                                break;
                        }

                        transformerUI.lastWeaponCrafted = WeaponRecipe.Weapon.NONE;
                    }

                    if (Input.GetKeyDown(KeyCode.E) && !transformerUI.isCrafting)
                    {
                        isUIActive = !isUIActive;
                        transformerUI.onClick(1);//Pour qu'on pop avec une recette active que ca fasse pas trop sale
                        transformerUI.boolEnteringMenu = !transformerUI.boolEnteringMenu;
                        transformerUI.eventSystem.SetSelectedGameObject(transformerUI.trap.gameObject);

                        CameraController.camPeon.EnableRotation = !CameraController.camPeon.EnableRotation;

                        if (isUIActive)
                        {
                            transformerCanvasSplitScreen.GetComponent<Canvas>().enabled = true;
                            transformerCanvasSplitScreen.GetComponent<Canvas>().worldCamera = CameraController.camPeon.GetComponentInChildren<Camera>();
                            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
                            CanvasManager.instance.GetActionTextPeon().SetActive(false);

                            collision.GetComponent<Peon>().GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                            Peon.instance.isBusy = true;

                        }
                        else
                        {
                            transformerCanvasSplitScreen.GetComponent<Canvas>().enabled = false;
                            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
                            CanvasManager.instance.GetActionTextPeon().SetActive(true);
                            collision.GetComponent<Peon>().GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                            Peon.instance.isBusy = false;

                            //Apparition Magique
                            switch (transformerUI.lastWeaponCrafted)
                            {
                                case WeaponRecipe.Weapon.WOODSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.WOODSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.WOODHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.IRONSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.IRONSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.IRONHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.DIAMONDSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.DIAMONDHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.MITHRILSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.MITHRILSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;

                                    break;
                                case WeaponRecipe.Weapon.MITHRILHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                default:
                                    break;
                            }

                            transformerUI.lastWeaponCrafted = WeaponRecipe.Weapon.NONE;

                        }
                    }
                }//manette
                else if (Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2)
                {
                    if (Input.GetButtonDown(Peon.instance.Controller + "_B") && !transformerUI.isCrafting)
                    {
                        isUIActive = false;
                        transformerUI.boolEnteringMenu = false;
                        if (!isUIActive)
                        {
                            transformerCanvasSplitScreen.GetComponent<Canvas>().enabled = false;
                            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
                            CanvasManager.instance.GetActionTextPeon().SetActive(true);
                            Peon.instance.isBusy = false;
                            CameraController.camPeon.EnableRotation = true;

                            //Apparition Magique
                            switch (transformerUI.lastWeaponCrafted)
                            {
                                case WeaponRecipe.Weapon.WOODSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.WOODSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.WOODHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.IRONSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.IRONSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.IRONHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.DIAMONDSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.DIAMONDHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);

                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.MITHRILSWORD:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Sword")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[transformerUI.indexCraftSword].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                case WeaponRecipe.Weapon.MITHRILSCYTHE:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Scythe")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[4 + transformerUI.indexCraftScythe].GetComponent<ProgressiveDisparition>().StartDisparition = true;

                                    break;
                                case WeaponRecipe.Weapon.MITHRILHAMMER:
                                    foreach (Transform tf in GetComponentsInChildren<Transform>(true))
                                    {
                                        if (tf.tag == "Hammer")
                                        {

                                            tf.gameObject.SetActive(false);
                                        }
                                    }
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].gameObject.SetActive(true);
                                    GetComponentsInChildren<Transform>(true)[8 + transformerUI.indexCraftHammer].GetComponent<ProgressiveDisparition>().StartDisparition = true;
                                    break;
                                default:
                                    break;
                            }

                            transformerUI.lastWeaponCrafted = WeaponRecipe.Weapon.NONE;

                        }
                    }

                    if (Input.GetButtonDown(Peon.instance.Controller + "_A"))
                    {
                        isUIActive = true;

                        if (!transformerUI.boolEnteringMenu)
                        {
                            transformerUI.eventSystem.SetSelectedGameObject(transformerUI.trap.gameObject);
                            transformerUI.boolEnteringMenu = true;
                            transformerUI.onClick(1);//Pour qu'on pop avec une recette active que ca fasse pas trop sale
                        }

                        if (isUIActive)
                        {
                            transformerCanvasSplitScreen.GetComponent<Canvas>().enabled = true;
                            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
                            CanvasManager.instance.GetActionTextPeon().SetActive(false);

                            transformerCanvasSplitScreen.GetComponent<Canvas>().worldCamera = CameraController.camPeon.GetComponentInChildren<Camera>();
                            collision.GetComponent<Peon>().GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                            Peon.instance.isBusy = true;
                            CameraController.camPeon.EnableRotation = false;


                        }
                    }
                }
            }
        }
    }

    public void ClearInventory()
    {
        if (clearInventory)
        {
            clearInventory.Play();
        }
    }

    private void PeonGiveMinerals()
    {

        int peonMinerals = Peon.instance.totalMinerals;
        int peonWood = Peon.instance.minerals.wood;
        int peonIron = Peon.instance.minerals.iron;
        int peonMithril = Peon.instance.minerals.mithril;
        int peonDiamond = Peon.instance.minerals.diamond;

        if (peonMinerals > 0)
        {
            ClearInventory();
        }


        acquiredMinerals.wood += peonWood;
        acquiredMinerals.iron += peonIron;
        acquiredMinerals.mithril += peonMithril;
        acquiredMinerals.diamond += peonDiamond;

        peonMinerals = 0;

        Peon.instance.totalMinerals = peonMinerals;
        Peon.instance.minerals.wood = 0;
        Peon.instance.minerals.iron = 0;
        Peon.instance.minerals.diamond = 0;
        Peon.instance.minerals.mithril = 0;
    }
}
