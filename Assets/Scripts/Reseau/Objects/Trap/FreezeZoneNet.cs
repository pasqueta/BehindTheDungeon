using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FreezeZoneNet : NetworkBehaviour
{
    List<Collider> entityAliveInZone;

    //[SyncVar] bool onCooldown = false;
    [SyncVar] float dtCooldown = 1.2f;

    void Update ()
    {
        //if (onCooldown)
        {
            dtCooldown -= Time.deltaTime;
        }
        if (dtCooldown <= 0.0f)
        {
            NetworkServer.Destroy(transform.parent.gameObject);
        }
	}

    private void OnTriggerEnter(Collider collider)
    {
        //if (collider.transform.CompareTag("Enemy"))
        {
            if (collider.gameObject.GetComponent<EnemyNet>() != null)
            {
                collider.gameObject.GetComponent<EnemyNet>().HealthPoint--;
                collider.gameObject.GetComponent<EnemyNet>().SetFreeze(2.0f);
            }
            //onCooldown = true;
        }
    }
}
