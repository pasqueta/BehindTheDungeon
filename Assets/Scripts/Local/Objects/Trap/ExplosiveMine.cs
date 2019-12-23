using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : Trap {

    [SerializeField]
    GameObject explosionParticle;

    [SerializeField]
    float explosionForce = 40;

    [SerializeField]
    int damage = 30;

    private void OnTriggerEnter(Collider collider)
	{
        if(collider.CompareTag("Enemy"))
            Boom();
    }

    private void Boom()
    {
        explosionParticle.SetActive(true);
        explosionParticle.transform.parent = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<Enemy>())
            {
                hit.GetComponent<Enemy>().SetBump(3.0f);
                hit.GetComponent<Enemy>().HealthPoint -= damage;
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(explosionForce, transform.position, 5, 3.0f);
            }
        }
        Destroy(gameObject);
    }
}
