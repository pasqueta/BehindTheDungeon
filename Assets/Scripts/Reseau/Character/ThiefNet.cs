using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class ThiefNet : EnemyNet
{
    [SerializeField]
    AudioSource stealSound;

    float stealingTime = 5.0f;

    bool isInit = false;

    new void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponentInChildren<RagdollController>();
        defaultMat = GetComponentInChildren<Renderer>().material;

        Invoke("InitTarget", 3.0f);
        //StartCoroutine(CoroutineSetTarget());
    }

    void InitTarget()
    {
        StartCoroutine(CoroutineSetTarget());

        isInit = true;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!isServer)
        {
            return;
        }
        
        if (agent.isOnNavMesh && target == extractingPoint.transform && Vector3.Distance(target.transform.position, transform.position) < 1.0f)
        {
            if (haveSteal)
            {
                BagStealedSound();
            }
            NetworkServer.Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (isServer)
        {
            if (collider.GetComponent<Gold>() && !collider.GetComponent<ChestBossNet>() && !haveSteal)
            {
                idInstance = collider.GetInstanceID();
                if (SpawnerManagerNet.instance.EntityisStealing(this))
                {
                    NetworkServer.Destroy(collider.gameObject);

                    if (isStealing)
                    {
                        isStealing = false;
                    }
                    haveSteal = true;
                    target = extractingPoint.transform;
                    agent.SetDestination(target.position);
                }
            }
        }
    }

    public void StartStealing()
    {
        if (!HaveSteal && !isStealing)
        {
            StartCoroutine(CoroutineStealing());
        }
    }

    private IEnumerator CoroutineStealing()
    {
        isStealing = true;
        agent.isStopped = true;

        GetComponent<Animator>().SetBool("Loot", true);
        GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(stealingTime);

        if (gameObject && ChestBossNet.instance.NbMoneyBag > 0)
        {
            GetComponent<Animator>().SetBool("Loot", false);

            stealSound.Play();
            if(BossNet.instance.isServer || PeonNet.instance.isServer)
                ChestBossNet.instance.NbMoneyBag--;
            isStealing = false;
            //haveSteal = true;
            if (isServer)
            {
                haveSteal = true;
                target = extractingPoint.transform;
                if (target)
                {
                    agent.SetDestination(target.position);
                }
            }
            agent.isStopped = false;
            GetComponent<Rigidbody>().isKinematic = false;
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
            if (!taunt && isInit)
            {
                Transform tmpTarget = ChestBossNet.instance.transform;
                for (int i = 0; i < ChestBossNet.instance.transform.childCount; i++)
                {
                    tmpTarget = Vector3.Distance(transform.position, tmpTarget.position) < Vector3.Distance(transform.position, ChestBossNet.instance.transform.GetChild(i).position) ? tmpTarget : ChestBossNet.instance.transform.GetChild(i);
                }
                target = tmpTarget;

                agent.SetDestination(target.position);
            }
            yield return new WaitForSeconds(dtCheckTarget);
        }
    }

    [ClientRpc]
    void RpcSetInfo()
    {
        isStealing = false;
        haveSteal = true;
    }
    [Command]
    void CmdSetInfo()
    {
        isStealing = false;
        haveSteal = true;
    }
}
