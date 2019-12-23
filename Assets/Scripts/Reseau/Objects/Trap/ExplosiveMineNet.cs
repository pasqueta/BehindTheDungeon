using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosiveMineNet : TrapNet
{
    [SerializeField]
    GameObject explosionParticle;

    [SerializeField]
    float explosionForce = 100;

    [SerializeField]
    int damage = 30;

    private void OnTriggerEnter(Collider collider)
    {
        if (!isServer)
            return;

        if (collider.CompareTag("Enemy"))
        {
            //Boom();
            RpcBoom();
        }
    }

    private void Boom()
    {
        explosionParticle.SetActive(true);
        explosionParticle.transform.parent = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<EnemyNet>())
            {
                hit.GetComponent<EnemyNet>().SetBump(2.0f);
                hit.GetComponent<EnemyNet>().HealthPoint -= damage;
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, 5, 3.0f);
                }
            }
        }

        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcBoom()
    {
        explosionParticle.SetActive(true);
        explosionParticle.transform.parent = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<EnemyNet>())
            {
                hit.GetComponent<EnemyNet>().SetBump(2.0f);
                hit.GetComponent<EnemyNet>().HealthPoint -= damage;
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, 5, 3.0f);
                }
            }
        }

        NetworkServer.Destroy(gameObject);
    }
}
