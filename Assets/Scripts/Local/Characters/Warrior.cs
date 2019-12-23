using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : Enemy {

    [SerializeField]
    MeleeWeaponTrail weaponTrail;

    bool isAttacking = false;
    float attackRange = 2.5f;
    float attackDuration = 2.0f;
    int damage = 10;

    new void Start ()
    {
        base.Start();
        //boss = Boss.instance.gameObject;
        //chest = ChestBoss.instance.gameObject;
    }
	
	new void Update ()
    {
        base.Update();

        if (agent.enabled == true)
        {
            if (target == Boss.instance.transform || target == ChestBoss.instance.transform) // boss ou chest : attaquer
            {
                if (Vector3.Distance(target.transform.position, transform.position) < attackRange && !isAttacking)
                {
                    StartCoroutine(CoroutineAttack(attackDuration));
                }
            }
            else if (target == extractingPoint.transform)   // point d'extraction : se barrer
            {
                if (Vector3.Distance(target.transform.position, transform.position) < 1.0f)
                {
                    if (haveSteal)
                        BagStealedSound();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (target != Boss.instance.transform && target != ChestBoss.instance.transform && target != extractingPoint.transform)
        {
            if (collider.GetComponent<Gold>() && !haveSteal)
            {
                haveSteal = true;
                target = extractingPoint.transform;
                agent.SetDestination(target.position);
                Destroy(collider.gameObject);
            }
        }
    }

    private IEnumerator CoroutineAttack(float _time)
    {
        isAttacking = true;
        agent.enabled = false;

        GetComponent<Animator>().SetTrigger("Attack");

        yield return new WaitForSeconds(_time);

        isAttacking = false;
        agent.enabled = true;
    }

    private void Shoot()
    {
        if (target != null)
        {
            if (target == Boss.instance.transform && !Boss.instance.IsDead)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < attackRange + 1)
                {
                    Boss.instance.HealthPoint -= damage;
                }
            }
            else if (target == ChestBoss.instance.transform && !ChestBoss.instance.chestEmpty)
            {
                ChestBoss.instance.Pdv -= damage;
            }
            else if (target.GetComponent<Chicken>())
            {
                target.GetComponent<Chicken>().Pdv -= damage;
            }
        }
    }

    public void ActiveTrail()
    {
        weaponTrail.Emit = true;
    }

    public void DisableTrail()
    {
        weaponTrail.Emit = false;
    }
}
