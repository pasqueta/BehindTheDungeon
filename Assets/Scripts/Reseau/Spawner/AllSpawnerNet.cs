using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSpawnerNet : MonoBehaviour
{
    public static AllSpawnerNet instance = null;

    [SerializeField]
    PopThingsNet[] spawners;

    public int entitiesCount = 0;
    public bool nextWave = false;

    private void Start()
    {
        if (instance == null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        spawners = GetComponentsInChildren<PopThingsNet>();
    }

    private void Update()
    {
        nextWave = true;
        entitiesCount = 0;
        foreach (PopThingsNet pt in spawners)
        {
            if (!pt.stopSpawn)
            {
                nextWave = false;
            }
            entitiesCount += pt.entities.Count;
        }
    }

    public PopThingsNet[] GetSpawners()
    {
        return spawners;
    }

    public PopThingsNet GetSpawner(int i)
    {
        if (i >= 0 && i < spawners.Length)
        {
            return spawners[i];
        }
        else
        {
            return null;
        }
    }
}
