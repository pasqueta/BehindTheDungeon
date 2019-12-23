using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnerNet : NetworkBehaviour
{
    // Spawn this model of enemy or not 
    [SerializeField]
    private bool thief;
    [SerializeField]
    private bool warrior;
    [SerializeField]
    private bool mage;

    // Using curve or using uniform popping
    [SerializeField]
    private bool usingCurve;

    // Blueprint of spawn dispertion
    [SerializeField]
    private AnimationCurve curve;

    // x = time to spawn (replace Vector2 by float)
    private List<Vector2> spawnTimes;

    // Duration of the wave, set by the SpawnerManager
    float waveDuration;
    // delta Time 
    float currentDeltaTime;

    private List<GameObject> models;

    int rangeRandom = 0;

    void Start ()
    {
        if (models == null)
            models = new List<GameObject>();
        if (spawnTimes == null)
            spawnTimes = new List<Vector2>();
    }

	void Update ()
    {
        if (isServer)
        {
            if (spawnTimes.Count > 0)
            {
                if (currentDeltaTime > spawnTimes[0].x * waveDuration)
                {
                    AddEntity();
                    spawnTimes.RemoveAt(0);
                }
                currentDeltaTime += Time.deltaTime;
            }
        }
	}

    public void SetDispertion(int nbEntities)
    {
        if (spawnTimes == null)
            spawnTimes = new List<Vector2>();

        if (usingCurve)
        {
            for (int i = 0; i < nbEntities; i++)
            {
                bool b = false;
                do
                {
                    Vector2 v = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    if (v.y < curve.Evaluate(v.x))
                    {
                        spawnTimes.Add(v);
                        b = true;
                    }
                } while (!b);
            }
            IEnumerable<Vector2> TMPpoints = spawnTimes.OrderBy(v => v.x);
            spawnTimes = TMPpoints.ToList<Vector2>();
        }
        else
        {
            for (int i = 0; i < nbEntities; i++)
            {
                Vector2 v = new Vector2(i*1.0f/nbEntities ,0);
                spawnTimes.Add(v);
            }
        }
    }

    private void SetLotterie()
    {
        // Ajout des models pour lotterie
        if (models == null)
            models = new List<GameObject>();

        if (thief)
        {
            models.Add(SpawnerManagerNet.instance.thiefModel);
            rangeRandom++;
        }
        if (warrior)
        {
            models.Add(SpawnerManagerNet.instance.warriorModel);
            rangeRandom++;
        }
        if (mage)
        {
            models.Add(SpawnerManagerNet.instance.mageModel);
            rangeRandom++;
        }
    }

    private void AddEntity()
    {
        int lotterie = Random.Range(0, rangeRandom);
        GameObject go = Instantiate(models[lotterie], gameObject.transform.position, Quaternion.identity);
        
        SpawnerManagerNet.instance.AddEntites(go);

        go.GetComponent<EnemyNet>().SetExtractingPoint(gameObject);
        go.GetComponent<EnemyNet>().HaveSteal = false;

        NetworkServer.Spawn(go);
    }

    public void Play(float duration, int entities)
    {
        SetLotterie();
        waveDuration = duration;
        SetDispertion(entities);
        currentDeltaTime = 0.0f;
    }

    public void Stop()
    {
        while (spawnTimes.Count > 0)
        {
            spawnTimes.RemoveAt(0);
        }
    }
}
