using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
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
    // UI
    [SerializeField]
    Image lifeBar;

    // Components
    protected NavMeshAgent agent;
    protected Animator anim;
	protected RagdollController ragdoll;

    // Targets
    protected Transform target;
    //protected GameObject chest;
    //protected GameObject boss;
    public GameObject extractingPoint;
    protected float dtCheckTarget = 1.0f;

    // Gestion de l'or
    protected bool isStealing = false;
    protected bool haveSteal = false;
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
    protected int healthPoint = 100;

    [HideInInspector]
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

        StartCoroutine(CoroutineSetTarget());
    }

    protected void Update()
    {
        anim.SetFloat("Velocity",agent.velocity.magnitude);

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
                lifeBar.fillAmount = (float)healthPoint / 100;
        }
        else if(!isDead)
        {
            lifeBar.transform.parent.gameObject.SetActive(false);

            if (Random.Range(0,10) == 5) 
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
                Invoke("Dead", 4.0f);
                isDead = true;
            }
            else
            {
                SetBump(3.0f);
                GetComponent<ProgressiveDisparition>().disparitionDuration = 3.0f;
                GetComponent<ProgressiveDisparition>().StartDisparition = true;
                Invoke("Dead", 3.0f);
                if (weapon)
                {
                    weapon.disparitionDuration = 3.0f;
                    weapon.StartDisparition = true;
                }
                isDead = true;
            }
        }
    }

    protected void Dead()
    {
        agent.enabled = false;

        if (haveSteal)
            Instantiate(moneyBagModel, transform.position, Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0), ChestBoss.instance.transform);
        else
            if (Random.Range(0,5) == 0)
                Instantiate(boneModel, transform.position, Quaternion.Euler(0, Random.Range(0.0f,360.0f), 0));
        Destroy(gameObject);
		Boss.instance.KillSound();
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
		ragdoll.SetActiveRagdoll (true);

        yield return new WaitForSeconds(_time);

		ragdoll.SetActiveRagdoll (false);
		anim.enabled = true;
        //anim.speed = s;
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
                if (!ChestBoss.instance.chestEmpty)
                {
                    if (!Boss.instance.IsDead)
                    {
                        float distanceBoss = Vector3.Distance(transform.position, Boss.instance.transform.position);
                        float distanceChest = Vector3.Distance(transform.position, ChestBoss.instance.transform.position);

                        target = distanceBoss < distanceChest ? Boss.instance.transform : ChestBoss.instance.transform;
                    }
                    else
                    {
                        target = ChestBoss.instance.transform;
                    }

                }
                else if (!Boss.instance.IsDead)
                {
                    target = Boss.instance.transform;
                }
                else
                {
                    if (ChestBoss.instance.transform.childCount > 0)
                    {
                        Transform tmpTarget = ChestBoss.instance.GetComponentInChildren<Gold>().transform;
                        for (int i = 1; i < ChestBoss.instance.transform.childCount; i++)
                        {
                            tmpTarget = Vector3.Distance(transform.position, tmpTarget.position) < Vector3.Distance(transform.position, ChestBoss.instance.transform.GetChild(i).position) ? tmpTarget : ChestBoss.instance.transform.GetChild(i);
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
}
