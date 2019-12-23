using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyNet : NetworkBehaviour
{
    //-------------SERIALIZE FIELD/PUBLIC VARIABLES----------------//
    [SerializeField]
    GameObject boneModel;
    [SerializeField]
    GameObject moneyBagModel;

    [SerializeField]
    protected Material iceMat;
    protected Material defaultMat;

    [SerializeField]
    protected GameObject moneyBagInChild;
    [SerializeField]
    protected AudioSource bagStealedSound;
    [SerializeField]
    protected ProgressiveDisparition weapon;
    [SerializeField]
    Image lifeBar;

    // Component
    protected NavMeshAgent agent;
    protected Animator anim;
    protected RagdollController ragdoll;

    //Moving Target
    protected Transform target;
    public GameObject extractingPoint;
    protected float dtCheckTarget = 1.0f;

    // Gestion de l'or
    protected bool isStealing = false;
    [SyncVar]
    public bool haveSteal = false;
    public int idInstance = 0;

    // Controle de foule
    protected bool stun = false;
    protected bool freeze = false;
    protected bool knockDown = false;
    protected bool taunt = false;
    protected bool bump = false;
    protected bool isDead = false;
    // Points de vie
    protected int maxHealthPoint = 100;
    [SyncVar]
    public int healthPoint = 100;

    public bool canBeAttacking = true;

    //-------------ACCESSORS----------------//
    public int HealthPoint
    {
        get
        {
            return healthPoint;
        }

        set
        {
            healthPoint = value;
        }
    }

    public bool HaveSteal
    {
        get
        {
            return haveSteal;
        }

        set
        {
            haveSteal = value;
        }
    }

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponentInChildren<RagdollController>();
        defaultMat = GetComponentInChildren<Renderer>().material;

        Invoke("InitTarget", 2.0f);
    }

    void InitTarget()
    {
        StartCoroutine(CoroutineSetTarget());
    }

    protected void Update()
    {
        if (isServer)
        {
            anim.SetFloat("Velocity", agent.velocity.magnitude);
            if (target != null)
                gameObject.transform.LookAt(target.transform);
        }

        if (haveSteal)
        {
            anim.SetBool("HaveBag", true);
            moneyBagInChild.SetActive(true);
        }
        else
        {
            moneyBagInChild.SetActive(false);
        }

        UpdateLife();
    }

    protected void BagStealedSound()
    {
        if (bagStealedSound)
        {
            bagStealedSound.Play();
        }
    }

    protected void UpdateLife()
    {
        if (HealthPoint > 0)
        {
            if (lifeBar)
            {
                lifeBar.fillAmount = (float)healthPoint / 100;
            }
        }
        else if (!isDead)
        {

            if (GetComponent<CapsuleCollider>())
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }
            if (GetComponentInChildren<Canvas>())
            {
                GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            }

            if (Random.Range(0, 10) == 5)
            {
                agent.enabled = false;
                anim.SetTrigger("Death");
                GetComponent<ProgressiveDisparition>().disparitionDuration = 4.0f;
                GetComponent<ProgressiveDisparition>().StartDisparition = true;
                if (weapon)
                {
                    weapon.disparitionDuration = 4.0f;
                    weapon.StartDisparition = true;
                }
                if (isServer)
                {
                    Invoke("DeadNet", 4.0f);
                }
                isDead = true;
            }
            else
            {
                SetBump(3.0f);
                GetComponent<ProgressiveDisparition>().disparitionDuration = 3.0f;
                GetComponent<ProgressiveDisparition>().StartDisparition = true;
                if (isServer)
                {
                    Invoke("DeadNet", 3.0f);
                }
                if (weapon)
                {
                    weapon.disparitionDuration = 3.0f;
                    weapon.StartDisparition = true;
                }
                isDead = true;
            }

        }
    }

    //[ClientRpc]
    protected void DeadNet()
    {
        GameObject go = null;
        agent.enabled = false;

        if (HaveSteal)
        {
            Vector3 temp = transform.position;
            temp.y = 0.0f;
            Vector3 tempRot = transform.rotation.eulerAngles;
            tempRot.x = 0;
            tempRot.z = 0;
            go = Instantiate(moneyBagModel, temp, transform.rotation, ChestBossNet.instance.transform);
            go.transform.Rotate(tempRot);
            NetworkServer.Spawn(go);
        }
        else
        {
            if (Random.Range(0, 5) == 3)
            {
                Vector3 temp = transform.position;
                temp.y = 0.0f;
                Vector3 tempRot = transform.rotation.eulerAngles;
                tempRot.x = 0;
                tempRot.z = 0;
                go = Instantiate(boneModel, temp, transform.rotation);
                go.transform.Rotate(tempRot);
                NetworkServer.Spawn(go);
            }
        }

        NetworkServer.Destroy(gameObject);

        BossNet.instance.KillSound();
    }

    public void SetFreeze(float time)
    {
        if (!freeze)
            StartCoroutine(CoroutineFreeze(time));
    }

    public void SetKnockDown(float time)
    {
        StartCoroutine(CoroutineKnockDown(time));
    }

    public void SetTaunt(float time, Transform _target)
    {
        StartCoroutine(CoroutineTaunt(time, _target));
    }

    public void SetBump(float time)
    {
        if (!bump)
            StartCoroutine(CoroutineBump(time));
    }

    public void SetStun(float time)
    {
        StartCoroutine(CoroutineStun(time));
    }

    private IEnumerator CoroutineStun(float _time)
    {
        stun = true;
        agent.enabled = false;
        anim.SetBool("Stun", true);

        yield return new WaitForSeconds(_time);

        anim.SetBool("Stun", false);
        stun = false;
        agent.enabled = true;
        if (target)
        {
            agent.SetDestination(target.position);
        }
    }

    private IEnumerator CoroutineFreeze(float _time)
    {
        freeze = true;
        agent.enabled = false;
        float s = anim.speed;
        anim.speed = 0;
        GetComponentInChildren<Renderer>().material = iceMat;

        yield return new WaitForSeconds(_time);

        GetComponentInChildren<Renderer>().material = defaultMat;
        anim.speed = s;
        freeze = false;
        agent.enabled = true;
    }

    private IEnumerator CoroutineKnockDown(float _time)
    {
        knockDown = true;
        agent.enabled = false;
        yield return new WaitForSeconds(_time);
        knockDown = false;
        agent.enabled = true;
    }

    private IEnumerator CoroutineTaunt(float _time, Transform _target)
    {
        taunt = true;
        agent.SetDestination(_target.position);

        yield return new WaitForSeconds(_time);

        taunt = false;
        agent.SetDestination(target.position);
    }

    private IEnumerator CoroutineBump(float _time)
    {
        bump = true;
        agent.enabled = false;
        anim.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        ragdoll.SetActiveRagdoll(true);

        yield return new WaitForSeconds(_time);

        ragdoll.SetActiveRagdoll(false);
        anim.enabled = true;
        bump = false;
        GetComponent<Rigidbody>().isKinematic = true;
        agent.enabled = true;
    }

    protected IEnumerator CoroutineSetTarget()
    {
        while (!haveSteal)
        {
            if (!taunt)
            {
                if (!ChestBossNet.instance.chestEmpty)
                {
                    if (!BossNet.instance.IsDead)
                    {
                        float distanceBoss = Vector3.Distance(transform.position, BossNet.instance.transform.position);
                        float distanceChest = Vector3.Distance(transform.position, ChestBossNet.instance.transform.position);

                        target = distanceBoss < distanceChest ? BossNet.instance.transform : ChestBossNet.instance.transform;
                    }
                    else
                    {
                        target = ChestBossNet.instance.transform;
                    }

                }
                else if (!BossNet.instance.IsDead)
                {
                    target = BossNet.instance.transform;
                }
                else
                {
                    if (ChestBossNet.instance.transform.childCount > 0)
                    {
                        Transform tmpTarget = ChestBossNet.instance.transform.GetChild(0);
                        for (int i = 1; i < ChestBossNet.instance.transform.childCount; i++)
                        {
                            tmpTarget = Vector3.Distance(transform.position, tmpTarget.position) < Vector3.Distance(transform.position, ChestBossNet.instance.transform.GetChild(i).position) ? tmpTarget : ChestBossNet.instance.transform.GetChild(i);
                        }
                        target = tmpTarget;
                    }
                }
                if (agent.enabled)
                {
                    if (target)
                    {
                        agent.SetDestination(target.position);
                    }

                }
            }
            yield return new WaitForSeconds(dtCheckTarget);
        }
    }

    public void SetExtractingPoint(GameObject _go)
    {
        extractingPoint = _go;
    }

    /*[ClientRpc]
    protected void RpcDestroy(GameObject go)
    {
        GameObject obj = null;
        agent.enabled = false;
        
        if (HaveSteal)
        {
            obj = Instantiate(moneyBagModel, go.transform.position, go.transform.rotation, ChestBossNet.instance.transform);
            NetworkServer.Spawn(obj);
        }
        else
        {
            if (Random.Range(0, 5) >= 0)
            {
                obj = Instantiate(boneModel, go.transform.position, go.transform.rotation);
                NetworkServer.Spawn(obj);
            }
        }

        NetworkServer.Destroy(go);
    }*/
}
