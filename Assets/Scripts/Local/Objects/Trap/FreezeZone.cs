using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeZone : MonoBehaviour {

    List<Collider> entityAliveInZone;

    bool onCooldown = false;
    float dtCooldown = 1.2f;

    void Update () {

        if (onCooldown)
        {
            dtCooldown -= Time.deltaTime;
        }
        if (dtCooldown <= 0.0f)
        {
            Destroy(transform.parent.gameObject);
        }
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Enemy"))
        {
            if (collider.gameObject.GetComponent<Enemy>() != null)
            {
                collider.gameObject.GetComponent<Enemy>().HealthPoint--;
                collider.gameObject.GetComponent<Enemy>().SetFreeze(2.0f);
            }
            else if (collider.gameObject.GetComponent<Enemy>() != null)
            {
                collider.gameObject.GetComponent<EnemyNet>().HealthPoint--;
                collider.gameObject.GetComponent<EnemyNet>().SetFreeze(2.0f);
            }
            onCooldown = true;
        }
    }
}
