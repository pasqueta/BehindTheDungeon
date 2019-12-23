using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TrapControllerNet : NetworkBehaviour
{
    public static TrapControllerNet instance;

    Controller controller;

    [SerializeField]
    Text trapText;

    [SerializeField]
    Text textCurrentTrapCount;

    [SerializeField]
    Image imageL;
    [SerializeField]
    Image imageM;
    [SerializeField]
    Image imageR;

    [SerializeField]
    List<GameObject> models;

    [SerializeField]
    List<Image> images;

    [SerializeField]
    Vector3 targetOffset;
    [SerializeField]
    Vector3 cameraPosition;
    [SerializeField]
    LayerMask maskCamera;

    int index = 0;
    int nbCurrentTrap;

    GameObject trapToPlace;

    bool joystickTriggerPressed = false;

    public bool destroyTheOptionCanvas = false;

    float maxX = 26.9f;
    float minX = -20.9f;
    float maxZ = 14.9f;
    float minZ = -14.9f;

    // Use this for initialization
    void Start()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        controller = PeonNet.instance.Controller;
        GetComponentInChildren<Camera>().rect = PeonNet.instance.GetCameraController().GetCameraScene().rect;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer)
        //    return;
        UpdateDeplacement();
        UpdateRotation();
        UpdateRecalage();
        UpdateImage();
        UpdateTrapModel();
        UpdateExit();

        trapText.text = images[index].gameObject.name;
    }

    void UpdateDeplacement()
    {
        Vector3 velocity = Vector3.zero;

        transform.position += (transform.forward * Input.GetAxisRaw(controller + "_LYaxis")) / 4;
        transform.position += (transform.right * Input.GetAxisRaw(controller + "_LXaxis")) / 4;

        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float z = Mathf.Clamp(transform.position.z, minZ, maxZ);

        transform.position = new Vector3(x, transform.position.y, z);
    }

    void UpdateRotation()
    {
        if (controller == Controller.K1)
        {
            if (Input.GetMouseButton(1))
            {
                transform.Rotate(0, Input.GetAxis(controller + "_MXaxis") * 4, 0);
            }
        }
        else
        {
            transform.Rotate(0, Input.GetAxisRaw(controller + "_RXaxis") * -4, 0);
        }
    }

    private void UpdateRecalage()
    {
        Ray ray = new Ray(transform.position + targetOffset, transform.rotation * (cameraPosition));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, cameraPosition.magnitude, maskCamera))
        {
            GetComponentInChildren<Camera>().transform.position = hit.point;
        }
        else
        {
            GetComponentInChildren<Camera>().transform.localPosition = cameraPosition + targetOffset;
        }
    }

    void UpdateImage()
    {
        if (controller == Controller.K1)
        {
            if (Input.GetAxis("K1_MouseWheel") < 0 && !Input.GetMouseButton(1))
            {
                index++;
                if (index >= models.Count)
                    index = 0;
                if (trapToPlace)
                {
                    Destroy(trapToPlace);
                    trapToPlace = null;
                }
            }
            if (Input.GetAxis("K1_MouseWheel") > 0 && !Input.GetMouseButton(1))
            {
                index--;
                if (index <= -1)
                    index = models.Count - 1;
                if (trapToPlace)
                {
                    Destroy(trapToPlace);
                    trapToPlace = null;
                }
            }
        }
        else
        {
            if (Input.GetButtonDown(controller + "_RB"))
            {
                index++;
                if (index >= models.Count)
                    index = 0;
                if (trapToPlace)
                {
                    Destroy(trapToPlace);
                    trapToPlace = null;
                }
            }
            if (Input.GetButtonDown(controller + "_LB"))
            {
                index--;
                if (index <= -1)
                    index = models.Count - 1;
                if (trapToPlace)
                {
                    Destroy(trapToPlace);
                    trapToPlace = null;
                }
            }
        }

        if (index == 0)
        {
            imageL.sprite = images[images.Count - 1].sprite;
            imageM.sprite = images[index].sprite;
            imageR.sprite = images[index + 1].sprite;
        }
        else if (index == models.Count - 1)
        {
            imageL.sprite = images[index - 1].sprite;
            imageM.sprite = images[index].sprite;
            imageR.sprite = images[0].sprite;
        }
        else
        {
            imageL.sprite = images[index - 1].sprite;
            imageM.sprite = images[index].sprite;
            imageR.sprite = images[index + 1].sprite;
        }

        switch (images[index].name)
        {
            case "spike":
                nbCurrentTrap = TrapPadNet.instance.inventoryTrap.spike;
                break;
            case "arrowslit":
                nbCurrentTrap = TrapPadNet.instance.inventoryTrap.arrowslit;
                break;
            case "chicken":
                nbCurrentTrap = TrapPadNet.instance.inventoryTrap.chicken;
                break;
            case "frozen mine":
                nbCurrentTrap = TrapPadNet.instance.inventoryTrap.ice;
                break;
            case "explosive mine":
                nbCurrentTrap = TrapPadNet.instance.inventoryTrap.mine;
                break;
            default:
                break;
        }
        textCurrentTrapCount.text = "" + nbCurrentTrap;

        if (nbCurrentTrap <= 0)
        {
            imageM.color = Color.gray;
            if (trapToPlace)
            {
                trapToPlace.GetComponent<TrapNet>().SetRedColor();
            }
        }
        else
        {
            imageM.color = Color.white;
        }
    }

    void UpdateTrapModel()
    {
        if (controller == Controller.K1) // CLAVIER
        {
            // DEPLACMENT
            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
            if (!trapToPlace)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("BossGround")))
                {
                    trapToPlace = Instantiate(models[index], hit.point, Quaternion.identity);
                }
            }
            else
            {
                trapToPlace.GetComponent<TrapNet>().Move(ray);
            }

            // ROTATION
            if (Input.GetAxis("K1_MouseWheel") != 0 && Input.GetMouseButton(1))
            {
                trapToPlace.GetComponent<TrapNet>().Rotate(Input.GetAxis("K1_MouseWheel"));
            }

            // POSAGE
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("trapToPlace: " + trapToPlace.name);
                if (nbCurrentTrap > 0 && trapToPlace.GetComponent<TrapNet>().PlaceTrap())
                {
                    DecreaseTrapNumber();
                    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAh
                    if (PeonNet.instance.isServer)
                    {
                        TrapPadNet.instance.AddTrapInList(trapToPlace);
                        NetworkServer.Spawn(trapToPlace);
                        PeonNet.instance.RpcPlaceTrapNet(trapToPlace);
                        trapToPlace = null;
                    }
                    else
                    {
                        PeonNet.instance.CmdSpawnTrapNet(trapToPlace.GetComponent<NetworkIdentity>().assetId, trapToPlace.transform.position, trapToPlace.transform.rotation);

                        Destroy(trapToPlace);
                        trapToPlace = null;
                    }
                }
            }
        }
        else // MANETTE
        {
            // DEPLACEMENT
            /*if (!trapToPlace)
            {
                trapToPlace = Instantiate(models[index], transform.position, Quaternion.identity);
            }
            else
            {
                if (trapToPlace.GetComponent<TrapNet>().IsGroundedTrap())
                {
                    trapToPlace.GetComponent<TrapNet>().Move(transform.position);
                }
                else
                {
                    Ray ray = new Ray(GetComponentInChildren<Camera>().transform.position, (transform.position - GetComponentInChildren<Camera>().transform.position + Vector3.up));
                    trapToPlace.GetComponent<TrapNet>().Move(ray);
                }
            }*/
            // DEPLACMENT
            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
            if (!trapToPlace)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("BossGround")))
                {
                    trapToPlace = Instantiate(models[index], hit.point, Quaternion.identity);
                }
            }
            else
            {
                trapToPlace.GetComponent<TrapNet>().Move(ray);
            }

            // ROTATION
            if (Input.GetAxisRaw(controller + "_RT") != 0)
            {
                if (!joystickTriggerPressed)
                {
                    trapToPlace.GetComponent<TrapNet>().Rotate(1);
                    joystickTriggerPressed = true;
                }
            }
            else if (Input.GetAxisRaw(controller + "_LT") != 0)
            {
                if (!joystickTriggerPressed)
                {
                    trapToPlace.GetComponent<TrapNet>().Rotate(-1);
                    joystickTriggerPressed = true;
                }
            }
            else
            {
                joystickTriggerPressed = false;
            }


            // POSAGE
            if (Input.GetButtonDown(controller + "_A"))
            {
                if (nbCurrentTrap > 0 && trapToPlace.GetComponent<TrapNet>().PlaceTrap())
                {
                    DecreaseTrapNumber();
                    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAh
                    if (PeonNet.instance.isServer)
                    {
                        TrapPadNet.instance.AddTrapInList(trapToPlace);
                        NetworkServer.Spawn(trapToPlace);
                        PeonNet.instance.RpcPlaceTrapNet(trapToPlace);
                        trapToPlace = null;
                    }
                    else
                    {
                        PeonNet.instance.CmdSpawnTrapNet(trapToPlace.GetComponent<NetworkIdentity>().assetId, trapToPlace.transform.position, trapToPlace.transform.rotation);

                        Destroy(trapToPlace);
                        trapToPlace = null;
                    }
                }
            }
        }
    }

    void UpdateExit()
    {
        if (controller == Controller.K1)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                if (PeonNet.instance)
                {
                    PeonNet.instance.isBusy = false;
                }
                Destroy(trapToPlace);
                trapToPlace = null;
                gameObject.SetActive(false);
                if (PeonNet.instance)
                    PeonNet.instance.GetCameraController().EnableRotation = true;
                gameObject.SetActive(false);

                DataManager.instance.destroyTheOptionCanvas = true;
            }
        }
        else
        {
            if (Input.GetButtonDown(controller + "_B"))
            {
                if (PeonNet.instance)
                {
                    PeonNet.instance.isBusy = false;
                }
                Destroy(trapToPlace);
                trapToPlace = null;
                gameObject.SetActive(false);
                if (PeonNet.instance)
                    PeonNet.instance.GetCameraController().EnableRotation = true;
                gameObject.SetActive(false);

                DataManager.instance.destroyTheOptionCanvas = true;
            }
        }

        if (!SpawnerManagerNet.instance.IsBetweenTwoWaves())
        {
            if (PeonNet.instance)
            {
                PeonNet.instance.isBusy = false;
            }
            Destroy(trapToPlace);
            trapToPlace = null;
            gameObject.SetActive(false);
            if (PeonNet.instance)
                PeonNet.instance.GetCameraController().EnableRotation = true;
            gameObject.SetActive(false);
        }
    }

    private void DecreaseTrapNumber()
    {
        switch (images[index].name)
        {
            case "spike":
                TrapPadNet.instance.inventoryTrap.spike--;
                break;
            case "arrowslit":
                TrapPadNet.instance.inventoryTrap.arrowslit--;
                break;
            case "chicken":
                TrapPadNet.instance.inventoryTrap.chicken--;
                break;
            case "frozen mine":
                TrapPadNet.instance.inventoryTrap.ice--;
                break;
            case "explosive mine":
                TrapPadNet.instance.inventoryTrap.mine--;
                break;
            default:
                break;
        }
    }
}

