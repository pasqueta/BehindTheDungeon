using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using UnityEngine.Networking;

[System.Serializable]
public struct SpawnerInfoNet
{
    public SpawnerNet spawner;
    public int entities;
}

public class SpawnerManagerNet : NetworkBehaviour
{
    public static SpawnerManagerNet instance; // singleton

    [SerializeField]
    int numberOfWaves = 2; // Number of waves
    int currentWave = 0;   // Current wave  
    [SerializeField]
    float waveDuration = 10.0f; // Time during which enemies spawn
    [SerializeField]
    float timeBetweenWaves = 10.0f; // Time between two waves
    [SerializeField]
    float timeBeforeFirstWave = 10.0f; // Time before the first wave
    [SerializeField]
    AudioSource chansonPeon; // La chanson du péon

    [SyncVar] bool betweenWave = false;   // true : between two waves
                                          // false: currently in wave
    [SyncVar] float currentdeltaTime = 0.0f; // time to mesurate each phase

    [SerializeField]
    float entityIncrease = 2;

    // All spawners in the scenes and the number of entities they should instantiate per wave
    [SerializeField]
    SpawnerInfoNet [] spawnerInfo;

    // Ref off all enemies alive
    List<GameObject> entitiesAlive;
    
    // Models of enemies
    [SerializeField]
    public GameObject thiefModel;
    [SerializeField]
    public GameObject warriorModel;
    [SerializeField]
    public GameObject mageModel;

    void Start ()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        entitiesAlive = new List<GameObject>();

        CanvasManagerNet.instance.GetTextTimeBetweenWaves().enabled = true;

        //StartSpawners();
    }


    private void Update()
    {
        if (isServer)
        {
            CanvasManagerNet.instance.GetTextRemainingWave().text = DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? // SI (1)
            (numberOfWaves - currentWave) <= 1 ?                    // ALORS (1) SI (2)                                       
            (numberOfWaves - currentWave) + " vague restante" :         // ALORS (2)
            (numberOfWaves - currentWave) + " vagues restantes" :       // SINON (2)
            (numberOfWaves - currentWave) <= 1 ?                    // SINON (1) SI (3)
            (numberOfWaves - currentWave) + " wave remaning" :          // ALORS (3)
            (numberOfWaves - currentWave) + " waves remaning";          // SINON (3)

            BossNet.instance.RpcSetTextRemainingWave(true, CanvasManagerNet.instance.GetTextRemainingWave().text);

            if (timeBeforeFirstWave <= 0)
            {
                if (WinLooseManagerNet.instance)
                {
                    if (!WinLooseManagerNet.instance.gameIsAlreadyEnd)
                    {
                        if (betweenWave) // between two waves
                        {
                            currentdeltaTime += Time.deltaTime;
                            CanvasManagerNet.instance.GetTextTimeBetweenWaves().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 81] + " " + ((int)(timeBetweenWaves - currentdeltaTime) / 1);
                            BossNet.instance.RpcSetTextBetweenToWaves(betweenWave, CanvasManagerNet.instance.GetTextTimeBetweenWaves().text);
                            if (currentdeltaTime <= timeBetweenWaves / 2.0f)
                            {
                                ChansonPeon();
                            }
                            if (currentdeltaTime >= timeBetweenWaves)
                            {
                                BossNet.instance.RpcSetTextBetweenToWaves(false, CanvasManagerNet.instance.GetTextTimeBetweenWaves().text);

                                CanvasManagerNet.instance.GetTextTimeBetweenWaves().enabled = false;
                                betweenWave = false;
                                currentdeltaTime = 0.0f;
                                StartSpawners();
                            }
                        }
                        else // during a wave
                        {
                            // laisser tourner le timer jusqu'à ce que tout les ennemies aient pop
                            currentdeltaTime += Time.deltaTime;

                            // faire le traitement après
                            if (currentdeltaTime >= waveDuration)
                            {
                                if (GetEntitiesCount() == 0) // si tout les ennemies sont morts ou partis
                                {
                                    currentWave++;
                                    //numberOfWaves--;
                                    if (currentWave >= numberOfWaves) // victoire
                                    {
                                        WinLooseManagerNet.instance.EndGame(true, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 80]);
                                    }
                                    else // commencer une nouvelle vague
                                    {
                                        currentdeltaTime = -10000.0f;

                                        //betweenWave = true;
                                        CanvasManagerNet.instance.GetTextTimeBetweenWaves().enabled = true;
                                        BossNet.instance.RpcSetTextBetweenToWaves(true, CanvasManagerNet.instance.GetTextTimeBetweenWaves().text);
                                        Invoke("DisableEndOfWave", 1.0f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (BossNet.instance && CanvasManagerNet.instance)
                {
                    timeBeforeFirstWave -= Time.deltaTime;

                    CanvasManagerNet.instance.GetTextTimeBetweenWaves().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 115] + " " + ((int)(timeBeforeFirstWave - currentdeltaTime) / 1);
                    BossNet.instance.RpcSetTextBetweenToWaves(true, CanvasManagerNet.instance.GetTextTimeBetweenWaves().text);

                    if (timeBeforeFirstWave <= 0)
                    {
                        CanvasManagerNet.instance.GetTextTimeBetweenWaves().enabled = false;
                        BossNet.instance.RpcSetTextBetweenToWaves(false, CanvasManagerNet.instance.GetTextTimeBetweenWaves().text);
                        StartSpawners();
                    }
                }
            }
        }
    }

    void DisableEndOfWave()
    {
        currentdeltaTime = 0.0f;
        betweenWave = true;
        CanvasManager.instance.GetTextTimeBetweenWaves().enabled = true;
        BossNet.instance.RpcSetTextRemainingWave(true, CanvasManagerNet.instance.GetTextRemainingWave().text);
    }

    private void StartSpawners()
    {
        for (int i = 0; i < spawnerInfo.Length; i++)
        {
            spawnerInfo[i].spawner.Play(waveDuration, (int)(spawnerInfo[i].entities + (currentWave * entityIncrease)));
        }
    }

    public void ChansonPeon()
    {
        if (chansonPeon)
        {
            chansonPeon.Play();
        }
    }

    public void AddEntites(GameObject entity)
    {
        entitiesAlive.Add(entity);
    }


    public void StopSpawners()
    {
        for (int i = 0; i < spawnerInfo.Length; i++)
        {
            spawnerInfo[i].spawner.Stop();
        }
    }

    public void KillAllEnemies()
    {
        foreach (GameObject item in entitiesAlive)
        {
            NetworkServer.Destroy(item);
        }
        GetEntitiesCount();
    }

    public int GetEntitiesCount()
    {
        // Remove all enemies killed 
        entitiesAlive.RemoveAll(item => item == null);

        return entitiesAlive.Count;
    }

    public List<GameObject> GetAllEntities()
    {
        entitiesAlive.RemoveAll(item => item == null);
        return entitiesAlive;
    }

    public bool EntityHaveSteal()
    {
        GetEntitiesCount();
        for (int i = 0; i < entitiesAlive.Count; i++)
        {
            if (entitiesAlive[i].GetComponent<EnemyNet>().HaveSteal)
            {
                return true;
            }
        }
        return false;
    }

    public bool EntityisStealing(EnemyNet en)
    {
        GetEntitiesCount();
        for (int i = 0; i < entitiesAlive.Count; i++)
        {
            if (entitiesAlive[i] != en.gameObject && entitiesAlive[i].GetComponent<EnemyNet>().HaveSteal)
            {
                if (en.idInstance == entitiesAlive[i].GetComponent<EnemyNet>().idInstance)
                {
                    en.idInstance = 0;
                    return false;
                }
            }
        }
        return true;
    }

    public bool IsBetweenTwoWaves()
    {
        return betweenWave;
    }
}
