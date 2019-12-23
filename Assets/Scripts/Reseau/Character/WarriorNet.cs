using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class WarriorNet : EnemyNet
{
    [SerializeField]
    MeleeWeaponTrail weaponTrail;
    

    bool isAttacking = false;
    float attackRange = 2.5f;
    float attackDuration = 2.0f;
    int damage = 10;

    bool isInit = false;

    new void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        ragdoll = GetComponentInChildren<RagdollController>();
   
        Invoke("InitTarget", 3.0f);
    }

    void InitTarget()
    {
        StartCoroutine(CoroutineSetTarget());

        if (target != null)
        {
            isInit = true;
        }
    }

    new void Update()
    {
        if (!isInit)
        {
            InitTarget();
        }

        base.Update();
        
        if (agent.enabled == true && isInit)
        {
            // Si ni refroidi, ni à terre, ni en train d'attaquer : Fait des trucs

            if (target == BossNet.instance.transform || target == ChestBossNet.instance.transform) // boss ou chest : attaquer
            {
                if (Vector3.Distance(target.transform.position, transform.position) < attackRange && !isAttacking)
                {
                    StartCoroutine(CoroutineAttack(attackDuration));
                }
            }
            else if ((PeonNet.instance.isServer || BossNet.instance.isServer) && target == extractingPoint.transform)   // point d'extraction : se barrer
            {
                if (Vector3.Distance(target.transform.position, transform.position) < 1.0f)
                {
                    if (haveSteal)
                    {
                        BagStealedSound();
                    }

                    NetworkServer.Destroy(gameObject);
                }
            }

        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if(isServer)
        {
            if (target != BossNet.instance.transform && target != ChestBossNet.instance.transform && target != extractingPoint.transform)
            {
                if (collider.GetComponent<Gold>() && !haveSteal)
                {
                    haveSteal = true;
                    target = extractingPoint.transform;
                    agent.SetDestination(target.position);
                    NetworkServer.Destroy(collider.gameObject);
                }
            }
        }
    }

    private IEnumerator CoroutineAttack(float _time)
    {
        isAttacking = true;
        agent.isStopped = true;

        GetComponent<Animator>().SetTrigger("Attack");

        yield return new WaitForSeconds(_time);

        isAttacking = false;
        agent.isStopped = false;
    }

    private void Shoot()
    {
        if (target != null)
        {
            if (target == BossNet.instance.transform && !BossNet.instance.IsDead)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < attackRange + 1)
                {
                    BossNet.instance.HealthPoint -= damage;
                }
            }
            else if (target == ChestBossNet.instance.transform && !ChestBossNet.instance.chestEmpty)
            {
                ChestBossNet.instance.Pdv -= damage;
            }
            else if (target.GetComponent<ChickenNet>())
            {
                target.GetComponent<ChickenNet>().Pdv -= damage;
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
