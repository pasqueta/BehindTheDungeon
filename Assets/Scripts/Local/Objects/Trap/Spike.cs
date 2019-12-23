using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/********************************************************
 * Auteur : Paul
 * 
 * Resumé : piques qui sortent du sol
 * condition de trigger : ennemie/boss passe dessus
 * Au déclanchement : fait des dégats
 * réutilisable : oui
 * cooldown : 5.0 s
 * condition de fin : maxUse
 * MaJ futur : ..?
********************************************************/
public class Spike : Trap {

    [SerializeField]
    GameObject spike;

    [SerializeField]
    AudioSource soundSpikes;

    List<Collider> entityAliveOnTrap;

    int currentUse = 0;

    bool onCooldown = false;
    float dtCooldown = 5.0f;

    new void Start()
    {
        base.Start();
        entityAliveOnTrap = new List<Collider>();
        damages = Peon.instance.GiveTrapRecipe(4).GetTrapDamages();
        maxUse = Peon.instance.GiveTrapRecipe(4).GetTrapDamages();
    }

    private void Update()
    {
        if (onCooldown)
        {
            dtCooldown -= Time.deltaTime;
            spike.transform.localPosition = new Vector3(0, -1 + dtCooldown / 5.0f, 0);
            if (dtCooldown <= 0.0f)
            {
                
                onCooldown = false;
                dtCooldown = 5.0f;
            }
        }


        if (entityAliveOnTrap.Count > 0 && !onCooldown && currentUse != maxUse)
        {
            spike.transform.localPosition = new Vector3(0, 0, 0);
            onCooldown = true;
            currentUse++;

            soundSpikes.Play();

            if (entityAliveOnTrap.Count > 0)
            {
                foreach (Collider c in entityAliveOnTrap)
                {
                    if (c)
                    {
                        if (c.transform.CompareTag("Boss"))
                        {
c.gameObject.GetComponent<Boss>().HealthPoint -= damages;                        }
                        else if (c.gameObject.GetComponent<BossNet>() != null)
                        {
                            c.gameObject.GetComponent<BossNet>().HealthPoint -= damages;
                        }

                        if (c.transform.CompareTag("Enemy"))
                        {
                            if (c.gameObject.GetComponent<Enemy>() != null)
                            {
                                c.gameObject.GetComponent<Enemy>().HealthPoint -= damages;
                            }
                            else if (c.gameObject.GetComponent<EnemyNet>() != null)
                            {
                                c.gameObject.GetComponent<EnemyNet>().HealthPoint -= damages;
                            }
                        }
                    }

                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        
        if (isActive && (collider.transform.CompareTag("Boss") || collider.transform.CompareTag("Enemy")))
        {  
            entityAliveOnTrap.Add(collider);
        }
    }

    new void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);
        if (collider.transform.CompareTag("Boss") || collider.transform.CompareTag("Enemy"))
        {
            entityAliveOnTrap.Remove(collider);
        }
    }


}
