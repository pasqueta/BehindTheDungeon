using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class spawnPosEnemyNet : NetworkBehaviour
{
    private NetworkStartPosition[] spawnPoints;
    private List<NetworkStartPosition> correctSpawn = new List<NetworkStartPosition>();

    void Start()
    {
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            foreach (NetworkStartPosition nsp in spawnPoints)
            {
                if (nsp.tag == transform.tag)
                {
                    correctSpawn.Add(nsp);
                }
            }
            int index = Random.Range(0, correctSpawn.Count);
            transform.position = correctSpawn[index].transform.position;
        }
    }
}
