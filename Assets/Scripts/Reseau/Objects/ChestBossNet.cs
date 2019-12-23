using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChestBossNet : NetworkBehaviour
{
    public static ChestBossNet instance;

    [SerializeField]
    GameObject moneyBagModel;

    [SerializeField]
    int lifePerMoneyBag = 100;

    [SyncVar]
    int currentLife;
    
    [SerializeField, SyncVar]
    int nbMoneyBag = 0;

    public bool chestEmpty = false;

    GameObject obj;

    public int NbMoneyBag
    {
        get
        {
            return nbMoneyBag;
        }

        set
        {
            nbMoneyBag = value;
        }
    }

    public int Pdv
    {
        get
        {
            return currentLife;
        }

        set
        {
            currentLife = value;
            if (currentLife<=0)
            {
                NbMoneyBag--;

                if (nbMoneyBag >= 0)
                {
                    if(isServer)
                    {
                        obj = Instantiate(moneyBagModel, transform.position + transform.right * 1.5f * Random.Range(1.0f, 3) + Random.Range(-2, 3.0f) * transform.forward, Quaternion.identity, transform);
                        NetworkServer.Spawn(obj);
                    }
                    foreach(Gold go in FindObjectsOfType<Gold>())
                    {
                        go.gameObject.transform.SetParent(transform);
                    }
                    //else
                    //{
                    //    PeonNet.instance.CmdSpawnAChosenObject(moneyBagModel.GetComponent<NetworkIdentity>().assetId, transform.position + transform.right * 1.5f * Random.Range(1.0f, 3) + Random.Range(-2, 3.0f) * transform.forward, Quaternion.identity);
                    //}
                }

                currentLife = lifePerMoneyBag;
            }

            if (nbMoneyBag == 0)
            {
                chestEmpty = true;
            }
        }
    }

    void Start () {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentLife = lifePerMoneyBag;
    }

    void Update()
    {
        if(PeonNet.instance && BossNet.instance)
        if (PeonNet.instance.isServer || BossNet.instance.isServer)
        {
            if (NbMoneyBag <= 0 && BossNet.instance.NbMoneyBag <= 0)
            {
                if (SpawnerManagerNet.instance.EntityHaveSteal())
                    return;

                if (transform.childCount > 0)
                    return;

                if (WinLooseManagerNet.instance != null)
                {
                    WinLooseManagerNet.instance.EndGame(false, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 82]);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<ThiefNet>())
        {
            collider.GetComponent<ThiefNet>().StartStealing();
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<BossNet>() && BossNet.instance.NbMoneyBag > 0)
        {
            if (BossNet.instance.isLocalPlayer)
            {
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(true);
                if ((BossNet.instance.Controller == Controller.K1 && Input.GetButtonDown(BossNet.instance.Controller + "_E")) ||
                    ((BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2) && Input.GetButtonDown(BossNet.instance.Controller + "_A")))
                {
                    nbMoneyBag += BossNet.instance.NbMoneyBag;
                    BossNet.instance.NbMoneyBag = 0;
                }
            }
        }
        else
        {
            CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<BossNet>())
        {
            CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
        }
    }
}
