using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

[System.Serializable]
public struct SpawnerInfo
{
    public Spawner spawner;
    public int entities;
}

public class SpawnerManager : MonoBehaviour {

    public static SpawnerManager instance; // singleton

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

    bool betweenWave = false;   // true : between two waves
                                // false: currently in wave
    float currentdeltaTime = 0.0f; // time to mesurate each phase

    [SerializeField]
    float entityIncrease = 2;

    // All spawners in the scenes and the number of entities they should instantiate per wave
    [SerializeField]
    SpawnerInfo [] spawnerInfo;

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
            instance = this;
        else
            Destroy(gameObject);

        entitiesAlive = new List<GameObject>();

        CanvasManager.instance.GetTextTimeBetweenWaves().enabled = true;

        //Invoke("StartSpawners", timeBeforeFirstWave);
    }


    private void Update()
    {
        // flème de faire du if et du propre
        CanvasManager.instance.GetTextRemainingWave().text = DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS ? // SI (1)
            (numberOfWaves - currentWave) <= 1 ?                    // ALORS (1) SI (2)                                       
        (numberOfWaves - currentWave) + " vague restante" :         // ALORS (2)
        (numberOfWaves - currentWave) + " vagues restantes" :       // SINON (2)
            (numberOfWaves - currentWave) <= 1 ?                    // SINON (1) SI (3)
        (numberOfWaves - currentWave) + " wave remaning" :          // ALORS (3)
        (numberOfWaves - currentWave) + " waves remaning";          // SINON (3)


        if (timeBeforeFirstWave <= 0)
        {
            if (WinLooseManager.instance && !WinLooseManager.instance.gameIsAlreadyEnd)
            {
                if (betweenWave) // between two waves
                {
                    currentdeltaTime += Time.deltaTime;
                    /*
                    if (timeBeforeFirstWave > 0.0f)
                    {
                        CanvasManager.instance.GetTextTimeBetweenWaves().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 114] + " " + ((int)(timeBeforeFirstWave - currentdeltaTime) / 1);
                    }
                    */
                    CanvasManager.instance.GetTextTimeBetweenWaves().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 81] + " " + ((int)(timeBetweenWaves - currentdeltaTime) / 1);
                    if(currentdeltaTime <= timeBetweenWaves/2.0f)
                    {
                        ChansonPeon();
                    }
                    if (currentdeltaTime >= timeBetweenWaves)
                    {
                        CanvasManager.instance.GetTextTimeBetweenWaves().enabled = false;



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
                            // numberOfWaves--;
                            if (currentWave >= numberOfWaves) // victoire
                            {
                                if (WinLooseManager.instance)
                                    WinLooseManager.instance.EndGame(true, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 80]);
                                else if (WinLooseManagerNet.instance)
                                    WinLooseManagerNet.instance.EndGame(true, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 80]);
                            }
                            else // commencer une nouvelle vague
                            {
                                currentdeltaTime = -100000.0f;

                                CanvasManager.instance.GetTextEndOfWave().GetComponent<Animator>().SetTrigger("playPopAnim");
                                //CanvasManager.instance.GetTextEndOfWave().enabled = true;
                                Invoke("DisableEndOfWave", 1.0f);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            timeBeforeFirstWave -= Time.deltaTime;

            CanvasManager.instance.GetTextTimeBetweenWaves().text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 115] + " " + ((int)(timeBeforeFirstWave - currentdeltaTime) / 1);

            if (timeBeforeFirstWave <= 0)
            {
                CanvasManager.instance.GetTextTimeBetweenWaves().enabled = false;
                StartSpawners();
            }
        }
    }

    void DisableEndOfWave()
    {
        // CanvasManager.instance.GetTextEndOfWave().enabled = false;
        currentdeltaTime = 0.0f;
        betweenWave = true;
        CanvasManager.instance.GetTextTimeBetweenWaves().enabled = true;
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
            Destroy(item);
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
            if (entitiesAlive[i].GetComponent<Enemy>().HaveSteal)
            {
                return true;
            }
        }
        return false;
    }

    public bool EntityisStealing(Enemy en)
    {
        GetEntitiesCount();
        for (int i = 0; i < entitiesAlive.Count; i++)
        {
            if (entitiesAlive[i] != en.gameObject && entitiesAlive[i].GetComponent<Enemy>().HaveSteal)
            {
                if (en.idInstance == entitiesAlive[i].GetComponent<Enemy>().idInstance)
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
