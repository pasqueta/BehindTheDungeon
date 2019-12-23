using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class spawnPosPlayerNet : NetworkBehaviour
{
    private NetworkStartPosition[] spawnPoints;

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();

            // Vector3 spawnPoint = Vector3.zero;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                foreach (NetworkStartPosition nsp in spawnPoints)
                {
                    if (nsp.tag == transform.tag)
                    {
                        transform.position = nsp.transform.position;
                    }
                }
            }
        }
    }
}
