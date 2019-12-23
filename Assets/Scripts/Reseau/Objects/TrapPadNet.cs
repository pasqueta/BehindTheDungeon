using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPadNet : MonoBehaviour
{
    public static TrapPadNet instance;

    [SerializeField]
    GameObject cam;

    GameObject InversedUVGameObject;

    Transform peon;

    public UtilityStruct.TRAPS inventoryTrap;

    List<GameObject> trapsAlive;

    // Use this for initialization
    void Start()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        trapsAlive = new List<GameObject>();

        peon = null;
        InversedUVGameObject = GetComponentInChildren<MeshInverseUV>().gameObject;

        inventoryTrap.arrowslit = 0;
        inventoryTrap.mine = 1;
        inventoryTrap.ice = 1;
        inventoryTrap.chicken = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (PeonNet.instance)
        {
            if (PeonNet.instance.isLocalPlayer)
            {
                if (peon != null)
                {
                    Vector3 look;

                    look = PeonNet.instance.PivotCam.GetChild(0).position;
                    look.y = 3;

                    if (PeonNet.instance.Controller == Controller.K1)
                    {
                        if (SpawnerManagerNet.instance.IsBetweenTwoWaves() && Input.GetButtonDown(PeonNet.instance.Controller + "_E"))
                        {
                            if (Time.timeScale > 0)
                            {
                                PeonNet.instance.GetCameraController().EnableRotation = false;
                                PeonNet.instance.isBusy = true;
                                cam.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        if (SpawnerManagerNet.instance.IsBetweenTwoWaves() && Input.GetButtonDown(PeonNet.instance.Controller + "_A"))
                        {
                            PeonNet.instance.isBusy = true;
                            cam.SetActive(true);
                            PeonNet.instance.GetCameraController().EnableRotation = false;
                        }
                    }
                }
            }
            //if (BossNet.instance)
            //{
            //    if (BossNet.instance.isServer)
            //    {
            //        trapsAlive.RemoveAll(item => item == null);
            //
            //        foreach (GameObject t in trapsAlive)
            //        {
            //            if (betweenTwoWaves)
            //            {
            //                t.GetComponent<TrapNet>().SetIsActive(false);
            //            }
            //            else
            //            {
            //                t.GetComponent<TrapNet>().SetIsActive(true);
            //            }
            //        }
            //    }
            //}

            trapsAlive.RemoveAll(item => item == null);

            foreach (GameObject t in trapsAlive)
            {
                if (SpawnerManagerNet.instance.IsBetweenTwoWaves())
                {
                    t.GetComponent<TrapNet>().SetIsActive(false);
                }
                else
                {
                    t.GetComponent<TrapNet>().SetIsActive(true);
                }
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (SpawnerManagerNet.instance.IsBetweenTwoWaves() && collider.tag == "Peon")
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
            CanvasManagerNet.instance.GetActionTextPeon().SetActive(true);
            peon = collider.transform;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Peon")
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
            CanvasManagerNet.instance.GetActionTextPeon().SetActive(false);
            peon = null;
        }
    }


    public void AddTrapInList(GameObject go)
    {
        trapsAlive.Add(go);
    }
}
