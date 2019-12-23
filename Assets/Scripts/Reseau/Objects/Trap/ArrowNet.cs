using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/********************************************************
 * Auteur : Paul
 * 
 * Resumé : Un flèche qui vas dans une direction
 * 
 * MaJ futur : amélioré la plantaison 
********************************************************/
public class ArrowNet : NetworkBehaviour
{
    [SerializeField]
    int damage = 5;

    Rigidbody rb;

    bool haveJoint = false;

    float ttl = 5.0f;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        damage = PeonNet.instance.GiveTrapRecipe(0).GetTrapDamages();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!haveJoint)
        {
            Vector3 velocity = rb.velocity;
            //transform.forward = Vector3.Slerp(transform.forward, velocity.normalized, Time.deltaTime);
            transform.LookAt(transform.position + velocity.normalized);
        }
        ttl -= Time.deltaTime;
        if (ttl <= 0.0f)
        {
            if (isServer)
                NetworkServer.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!haveJoint && !collider.GetComponent<ArrowNet>())
        {
            haveJoint = true;

            if (collider.GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<FixedJoint>();
                gameObject.GetComponent<FixedJoint>().connectedBody = collider.GetComponent<Rigidbody>();

                if (collider.gameObject.CompareTag("Boss"))
                {
                    collider.GetComponent<BossNet>().HealthPoint -= damage;
                }
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    collider.GetComponent<EnemyNet>().HealthPoint -= damage;
                }
            }
            else
            {
                gameObject.AddComponent<FixedJoint>();
            }
        }
    }
}
