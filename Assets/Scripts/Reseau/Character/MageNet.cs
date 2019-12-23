using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class MageNet : EnemyNet
{
    [SerializeField]
    Transform staffHead;

    [SerializeField]
    GameObject magicBall;
   

    bool isAttacking = false;
    float attackRange = 10f;
    float attackDuration = 3.0f;
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
        if (target)
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

        if (agent.enabled && isInit && target)
        {
            if (target == BossNet.instance.transform || target == ChestBossNet.instance.transform) // boss ou chest : attaquer
            {
                if (Vector3.Distance(transform.position, target.transform.position) < attackRange && !isAttacking)
                {
                    StartCoroutine(CoroutineAttack(attackDuration));
                }
            }
            else if (target == extractingPoint.transform && (PeonNet.instance.isServer || BossNet.instance.isServer))   // point d'extraction : se barrer
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
                if (collider.GetComponent<Gold>() && !collider.GetComponent<ChestBossNet>() && !haveSteal)
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
        agent.enabled = false;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(_time);

        isAttacking = false;
        agent.enabled = true;
    }

    public void Shoot()
    {
        if(isServer)
        {
            GameObject mb = Instantiate(magicBall, staffHead.position, Quaternion.identity);
            mb.GetComponent<MagicBallNet>().SetTarget(target);
            mb.GetComponent<MagicBallNet>().SetDamages(damage);
            NetworkServer.Spawn(mb);
        }
    }
}
