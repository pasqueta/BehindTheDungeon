using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrapPad : MonoBehaviour {

    public static TrapPad instance; 

    [SerializeField]
    GameObject cam;

    GameObject InversedUVGameObject;

    Transform peon;

    public UtilityStruct.TRAPS inventoryTrap;

    List<GameObject> trapsAlive;

    // Use this for initialization
    void Start () {
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
	void Update () {
        if (peon != null)
        {
            if (Peon.instance != null)
            {
                if (Peon.instance.Controller == Controller.K1)
                {
                    if (SpawnerManager.instance.IsBetweenTwoWaves() && Input.GetButtonDown(Peon.instance.Controller + "_E"))
                    {
                        Peon.instance.GetCameraController().EnableRotation = false;
                        Peon.instance.isBusy = true;
                        cam.SetActive(true);
                    }
                }
                else
                {
                    if (SpawnerManager.instance.IsBetweenTwoWaves() && Input.GetButtonDown(Peon.instance.Controller + "_A"))
                    {
                        Peon.instance.GetCameraController().EnableRotation = false;
                        Peon.instance.isBusy = true;
                        cam.SetActive(true);
                    }
                }
            }
            else if (PeonNet.instance != null)
            {
                if ((PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2) && Input.GetButton(PeonNet.instance.Controller + "_A"))
                {
                    PeonNet.instance.isBusy = true;
                    cam.SetActive(true);

                }
                if (PeonNet.instance.Controller == Controller.K1 && Input.GetButton(PeonNet.instance.Controller + "_E"))
                {
                    PeonNet.instance.isBusy = true;
                    cam.SetActive(true);
                }
            }
        }

        trapsAlive.RemoveAll(item => item == null);

        foreach (GameObject t in trapsAlive)
        {
            if (SpawnerManager.instance.IsBetweenTwoWaves())
                t.GetComponent<Trap>().SetIsActive(false);
            else
                t.GetComponent<Trap>().SetIsActive(true);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (SpawnerManager.instance.IsBetweenTwoWaves() && collider.tag == "Peon")
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = true;
            CanvasManager.instance.GetActionTextPeon().SetActive(true);
            peon = collider.transform;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Peon")
        {
            InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;

            CanvasManager.instance.GetActionTextPeon().SetActive(false);
            peon = null;
        }
    }


    public void AddTrapInList(GameObject go)
    {
        trapsAlive.Add(go);
    }
}
