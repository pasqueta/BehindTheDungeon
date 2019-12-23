using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/********************************************************
 * Auteur : Paul
 * 
 * Resumé : Un flèche qui vas dans une direction
 * 
 * MaJ futur : amélioré la plantaison 
********************************************************/
public class Arrow : MonoBehaviour {

    [SerializeField]
    int damage = 5;
    
    Rigidbody rb;

    bool haveJoint = false;

    float ttl = 5.0f;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        damage = Peon.instance.GiveTrapRecipe(0).GetTrapDamages();
    }
	
	// Update is called once per frame
	void Update () {
        if (!haveJoint)
        {
            Vector3 velocity = rb.velocity;
            transform.LookAt(transform.position + velocity.normalized);
        }
        ttl -= Time.deltaTime;
        if (ttl <= 0.0f)
        {
            Destroy(gameObject);
        }

    }
    /* c moche mais je garde
    private void OnCollisionEnter(Collision collision)
    {
        if (!haveJoint && collision.gameObject.GetComponent<Rigidbody>())
        {
            haveJoint = true;

            Destroy(rb);
            Destroy(GetComponent<CapsuleCollider>());

            transform.position = collision.contacts[0].point;

            transform.parent = collision.transform;


            if (collision.transform.CompareTag("Boss"))
            {
                collision.gameObject.GetComponent<Boss>().BossPdv -= damage;
            }
            if (collision.transform.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().Pdv -= damage;
            }

        }
    }
    */

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.isTrigger)
        {
            if (!haveJoint && !collider.GetComponent<Arrow>())
            {
                haveJoint = true;

                if (collider.GetComponent<Rigidbody>())
                {
                    gameObject.AddComponent<FixedJoint>();
                    gameObject.GetComponent<FixedJoint>().connectedBody = collider.GetComponent<Rigidbody>();

                    if (collider.transform.CompareTag("Boss"))
                    {
                        collider.GetComponent<Boss>().HealthPoint -= damage;
                    }
                    if (collider.transform.CompareTag("Enemy"))
                    {
                        collider.GetComponent<Enemy>().HealthPoint -= damage;
                    }
                }
                else
                {
                    gameObject.AddComponent<FixedJoint>();
                }
            }
        }
    }
    
}
