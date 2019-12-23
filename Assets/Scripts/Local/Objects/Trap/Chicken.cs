using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
public class Chicken : Trap {

    [SerializeField]
    AudioSource soundPoulay;

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

    List<Enemy> enemiesFooled;
    List<EnemyNet> enemiesFooledNet;

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        enemiesFooled = new List<Enemy>();
        enemiesFooledNet = new List<EnemyNet>();
        sc = GetComponent<SphereCollider>();
        maxUse = Peon.instance.GiveTrapRecipe(1).GetTrapMaxUses();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isTrigger)
        {
            /*
            if (enemiesFooled.Count > 0)
            {
                foreach (Enemy enemy in enemiesFooled)
                {
                    enemy.SetTaunt(2.0f, transform);
                }
            }
            */
            ttl -= Time.deltaTime;
            dtDestroySphereCollider -= Time.deltaTime;
            if (dtDestroySphereCollider <= 0.0f)
            {
                Destroy(sc);
            }
        }

        if (pdv <= 0 || ttl <= 0)
        {
            Destroy(gameObject);
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
            if (collider.gameObject.GetComponent<Enemy>() != null)
            {
                collider.gameObject.GetComponent<Enemy>().SetTaunt(2.0f, transform);
                enemiesFooled.Add(collider.gameObject.GetComponent<Enemy>());
            }
            else if(collider.gameObject.GetComponent<EnemyNet>() != null)
            {
                collider.gameObject.GetComponent<EnemyNet>().SetTaunt(2.0f, transform);
                enemiesFooledNet.Add(collider.gameObject.GetComponent<EnemyNet>());
            }
        }
    }
}
