using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;


public class Thief : Enemy
{

    [SerializeField]
    AudioSource stealSound;

    //public Transform extractingPoint;

    float stealingTime = 5.0f;

    new void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponentInChildren<RagdollController>();
        defaultMat = GetComponentInChildren<Renderer>().material;

        StartCoroutine(CoroutineSetTarget());
    }

    new void Update()
    {
        base.Update();



        if (agent.isOnNavMesh && target == extractingPoint.transform && Vector3.Distance(target.transform.position, transform.position) < 1.0f)
        {
            if (haveSteal)
                BagStealedSound();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<Gold>() && !collider.GetComponent<ChestBoss>() && !haveSteal)
        {
            idInstance = collider.GetInstanceID();
            if (SpawnerManager.instance.EntityisStealing(this))
            {
                Destroy(collider.gameObject);

                if (isStealing)
                {
                    isStealing = false;

                }
                haveSteal = true;
                target = extractingPoint.transform;
                if (agent.enabled)
                    agent.SetDestination(target.position);
            }
        }
    }

    public void StartStealing()
    {
        if (!haveSteal && !isStealing)
        {
            StartCoroutine(CoroutineStealing());
        }
    }

    private IEnumerator CoroutineStealing()
    {
        
        isStealing = true;
        agent.enabled = false;

        GetComponent<Animator>().SetBool("Loot", true);
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(stealingTime);

        if (gameObject && ChestBoss.instance.NbMoneyBag > 0)
        {
            //idInstance = ChestBoss.instance.NbMoneyBag;
            //if (SpawnerManager.instance.EntityisStealing(this))
            {
                GetComponent<Animator>().SetBool("Loot", false);
                stealSound.Play();
                ChestBoss.instance.NbMoneyBag--;
                isStealing = false;
                haveSteal = true;
                target = extractingPoint.transform;
                agent.enabled = true;
                if (target)
                    agent.SetDestination(target.position);
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("Loot", false);
            isStealing = false;
            agent.enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }


    new private IEnumerator CoroutineSetTarget()
    {
        while (!haveSteal)
        {
            if (!taunt)
            {
                Transform tmpTarget = ChestBoss.instance.transform;
                for (int i = 0; i < ChestBoss.instance.transform.childCount; i++)
                {
                    tmpTarget = Vector3.Distance(transform.position, tmpTarget.position) < Vector3.Distance(transform.position, ChestBoss.instance.transform.GetChild(i).position) ? tmpTarget : ChestBoss.instance.transform.GetChild(i);
                }
                target = tmpTarget;
                if (agent.enabled)
                    agent.SetDestination(target.position);
            }
            yield return new WaitForSeconds(dtCheckTarget);
        }
    }

}
