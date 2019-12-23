using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBoss : MonoBehaviour {

    public static ChestBoss instance;

    [SerializeField]
    GameObject moneyBagModel;

    [SerializeField]
    int lifePerMoneyBag = 100;

    int currentLife;

    [SerializeField]
    int nbMoneyBag = 0;

    [HideInInspector]
    public bool chestEmpty = false;

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
                    Instantiate(moneyBagModel, transform.position + transform.right * 1.5f * Random.Range(1.0f, 3) + Random.Range(-2, 3.0f) * transform.forward, Quaternion.identity, transform);
                }
                else
                {
                    NbMoneyBag = 0;
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
        if (NbMoneyBag <= 0 && Boss.instance.NbMoneyBag <= 0)
        {
            if (SpawnerManager.instance.EntityHaveSteal())
                return;

            if (transform.childCount > 0)
                return;

            if (WinLooseManager.instance != null)
                WinLooseManager.instance.EndGame(false, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 82]);
            else if (WinLooseManagerNet.instance != null)
                WinLooseManagerNet.instance.EndGame(false, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 82]);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {       
        if (collider.GetComponent<Thief>())
        {          
            collider.GetComponent<Thief>().StartStealing();
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<Boss>() && Boss.instance.NbMoneyBag > 0)
        {
            CanvasManager.instance.GetActionTextBoss().SetActive(true);
            if ((Boss.instance.Controller == Controller.K1 && Input.GetButtonDown(Boss.instance.Controller + "_E")) ||
                ((Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2) && Input.GetButtonDown(Boss.instance.Controller + "_A")))
            {
                nbMoneyBag += Boss.instance.NbMoneyBag;
                Boss.instance.NbMoneyBag = 0;
            }
        }
        else
        {
            CanvasManager.instance.GetActionTextBoss().SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<Boss>())
        {
            CanvasManager.instance.GetActionTextBoss().SetActive(false);
        }
    }
}
