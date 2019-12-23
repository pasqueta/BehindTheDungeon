using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

/********************************************************
 * Auteur : Paul
 * 
 * Resumé : poulet qui aggro les mobs
 * condition de trigger : ennemie passe à 1 m au moins du poulet
 * Au déclanchement : aggro les mobs dans un zone de 5 m
 * réutilisable : non
 * condition de fin : meurt et/ou temps
 * 
 * MaJ futur : déplacment random du poulet quand déclanché
 *             animations
********************************************************/
public class ChickenNet : TrapNet
{
    [SerializeField]
    AudioSource soundPoulay;

    [SyncVar]
    int pdv = 1000;
    public int Pdv
    {
        get
        {
            return pdv;
        }

        set
        {
            pdv = value;
        }
    }

    bool isTrigger = false;
    float dtDestroySphereCollider = 0.1f;
    float ttl = 3.0f;
    SphereCollider sc;
    
    List<EnemyNet> enemiesFooledNet;

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        enemiesFooledNet = new List<EnemyNet>();
        sc = GetComponent<SphereCollider>();
        maxUse = PeonNet.instance.GiveTrapRecipe(1).GetTrapMaxUses();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isServer)
            return;

        if (isTrigger)
        {
            ttl -= Time.deltaTime;
            dtDestroySphereCollider -= Time.deltaTime;
            if (dtDestroySphereCollider <= 0.0f)
            {
                Destroy(sc);
            }
        }

        if (pdv <= 0 || ttl <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider collider)        
    {
        if (isActive && collider.transform.CompareTag("Enemy"))
        {
            soundPoulay.Play();

            if (sc && sc.radius < 5.0f)
            {
                isTrigger = true;
                sc.radius = 5.0f;
            }

            collider.gameObject.GetComponent<EnemyNet>().SetTaunt(2.0f, transform);
            enemiesFooledNet.Add(collider.gameObject.GetComponent<EnemyNet>());
        }
    }
}
