using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mage : Enemy
{
	[SerializeField]
	Transform staffHead;

	[SerializeField]
	GameObject magicBall;

    bool isAttacking = false;
    float attackRange = 10f;
    float attackDuration = 3.0f;
    int damage = 10;

    new void Start()
    {
        base.Start();
        //boss = Boss.instance.gameObject;
        //chest = ChestBoss.instance.gameObject;
    }

    new void Update()
    {
        base.Update();

        if (agent.enabled && agent.hasPath)
        {
            if (target == Boss.instance.transform || target == ChestBoss.instance.transform) // boss ou chest : attaquer
            {
                
                if (Vector3.Distance(transform.position,target.transform.position) < attackRange && !isAttacking)
                {
                    transform.LookAt(target.transform.position);
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
            if (collider.GetComponent<Gold>() && !collider.GetComponent<ChestBoss>() && !haveSteal)
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
        agent.isStopped = true;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(_time);

        isAttacking = false;
        if(agent && agent.isActiveAndEnabled)
            agent.isStopped = false;
    }


	public void Shoot()
	{
		GameObject mb = Instantiate (magicBall, staffHead.position, Quaternion.identity);
        mb.GetComponent<MagicBall>().SetTarget(target);
        mb.GetComponent<MagicBall>().SetDamages(damage);
	}
}
