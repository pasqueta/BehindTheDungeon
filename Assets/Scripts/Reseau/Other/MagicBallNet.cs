using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicBallNet : NetworkBehaviour
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
	GameObject impact;
    
    public Transform target;
    Vector3 offset;

    float projectileSpeed = 10.0f;
    int damages;

    [SyncVar]
    float dtImpact = 0.0f;

	// Use this for initialization
	void Start ()
	{
		offset = Vector3.up * 2;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (target)
        {
		    transform.LookAt (target.position+offset);
		    GetComponent<Rigidbody> ().velocity =  Vector3.Normalize((target.position + offset) - transform.position)*projectileSpeed;
        }

        if (impact.activeInHierarchy)
        {
            dtImpact += Time.deltaTime;
            if (dtImpact >= 1.0f)
            {
                if(isServer)
                {
                    NetworkServer.Destroy(gameObject);
                }
            }
        }
    }

	void OnTriggerEnter(Collider collider)
	{
        if (collider.isTrigger)
        {
            return;
        }
        if (collider.CompareTag("Enemy"))
        {
            return;
        }
        
        projectile.SetActive(false);
        impact.SetActive(true);

        if (target != null && BossNet.instance.isServer || PeonNet.instance.isServer)
        {
            if (target == BossNet.instance.transform && !BossNet.instance.GetComponent<BossNet>().IsDead)
            {
                BossNet.instance.HealthPoint -= damages;
                BossNet.instance.RpcDealDamage(0, damages);
            }
            else if (target == ChestBossNet.instance.transform)
            {
                ChestBossNet.instance.Pdv -= damages;
                BossNet.instance.RpcDealDamage(1, damages);
            }
            //else if (target.GetComponent<ChickenNet>() != null)
            //{
            //    target.GetComponent<ChickenNet>().Pdv -= damages;
            //}
        }

        projectileSpeed = 0.0f;
    }

    public void SetTarget(Transform _t)
    {
        target = _t;
    }

    public void SetDamages(int _i)
    {
        damages = _i;
    }
}
