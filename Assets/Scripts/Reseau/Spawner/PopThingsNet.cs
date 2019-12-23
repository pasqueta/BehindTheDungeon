using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PopThingsNet : NetworkBehaviour
{
    [SerializeField]
    bool thief;
    [SerializeField]
    bool warrior;
    [SerializeField]
    bool mage;

    [SerializeField]
    GameObject thiefModel;
    [SerializeField]
    GameObject warriorModel;
    [SerializeField]
    GameObject mageModel;

    [SerializeField, SyncVar]
    float timer;

    [SerializeField, SyncVar]
    int nbwave;

    [SerializeField, SyncVar]
    int nbEntityPerWave;

    [SerializeField, SyncVar]
    float timeBetweenWaves = 2.0f;

    [SerializeField, SyncVar]
    bool isInfinite;

    [SyncVar]
    int entityInCurrentWave;
    [SyncVar]
    public bool stopSpawn = false;
    [SyncVar]
    float tempTimer;
    [SyncVar]
    float dtwave;

    public List<GameObject> entities;

    List<GameObject> models;

    int rangeRandom = 0, thiefId, warriorId, mageId;

    Text textNextWave;

    // Use this for initialization
    void Start()
    {
        dtwave = 0.0f;
        tempTimer = timer;
        entityInCurrentWave = nbEntityPerWave;
        entities = new List<GameObject>();
        models = new List<GameObject>();
        if (thief)
        {
            models.Add(thiefModel);
            rangeRandom++;
        }
        if (warrior)
        {
            models.Add(warriorModel);
            rangeRandom++;
        }
        if (mage)
        {
            models.Add(mageModel);
            rangeRandom++;
        }

        textNextWave = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (AllSpawnerNet.instance.nextWave)
        {
            if (AllSpawnerNet.instance.entitiesCount == 0 && nbwave == 0)
            {
                WinLooseManagerNet.instance.EndGame(true, "All adventurers have been killed");
            }
            else if (AllSpawnerNet.instance.entitiesCount == 0 && nbwave != 0)
            {
                stopSpawn = false;
                dtwave = timeBetweenWaves;
                entityInCurrentWave = nbEntityPerWave;
                nbwave--;
            }
            else
            {
                foreach (GameObject go in entities)
                {
                    if (!go)
                    {
                        entities.Remove(go);
                        break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("J1_Back") || Input.GetButtonDown("J2_Back"))
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(item.gameObject);
            }
        }

        if (isInfinite)
        {
            tempTimer -= Time.deltaTime;
            if (tempTimer <= 0.0f)
            {
                AddEntity();
                tempTimer = timer;
            }
        }
        else
        {
            if (dtwave <= 0.0f)
            {
                textNextWave.text = "";
             //   TrapPadNet.instance.SetBetweenTwoWaves(false);
                if (timer <= 0)
                {
                    Debug.LogError("Timer is equal or inferior to 0.");
                }
                else if (!isInfinite && !stopSpawn)
                {
                    tempTimer -= Time.deltaTime;
                    if (tempTimer <= 0.0f)
                    {
                        AddEntity();
                        entityInCurrentWave--;
                        tempTimer = timer;
                    }

                    if (entityInCurrentWave <= 0)
                    {
                        stopSpawn = true;
                    }
                }
            }
            else
            {
                dtwave -= Time.deltaTime;
                textNextWave.text = "Next wave in " + ((int)dtwave / 1);
              //  TrapPadNet.instance.SetBetweenTwoWaves(true);
            }
        }
    }
    
    private void AddEntity()
    {
        int lotterie = Random.Range(0, rangeRandom);
        GameObject go = Instantiate(models[lotterie], gameObject.transform.position, Quaternion.identity);
        entities.Add(go);
        NetworkServer.Spawn(go);

        go.GetComponent<EnemyNet>().SetExtractingPoint(gameObject);
        go.GetComponent<EnemyNet>().HaveSteal = false;
    }
}

