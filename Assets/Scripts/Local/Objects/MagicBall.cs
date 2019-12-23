using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour {


    [SerializeField]
    GameObject projectile;
    [SerializeField]
	GameObject impact;

    Transform target;
    Vector3 offset;

    float projectileSpeed = 15.0f;
    int damages;

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
                Destroy(gameObject);
            }
        }
    }

	void OnTriggerEnter(Collider collider)
	{
        if (collider.isTrigger)
            return;
        if (collider.CompareTag("Enemy"))
            return;

        projectile.SetActive(false);
        impact.SetActive(true);
        if (collider.GetComponent<Boss>() && !Boss.instance.GetComponent<Boss>().IsDead)
        {
            Boss.instance.HealthPoint -= damages;
        }
        else if (collider.GetComponent<ChestBoss>())
        {
            ChestBoss.instance.Pdv -= damages;
        }
        else if (collider.GetComponent<Chicken>())
        {
            collider.GetComponent<Chicken>().Pdv -= damages;
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
