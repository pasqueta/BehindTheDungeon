using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/********************************************************
 * Auteur : Paul
 * 
 * Resumé : flêche par du mur pour se planter dans un truc
 * condition de trigger : ennemie passe sur une trappe
 * Au déclanchement : tir la flèche
 * réutilisable : oui
 * cooldown : 4 s
 * 
 * MaJ futur : voir class Arrow
 *             animations
 *             bien refaire les normals (faites pas chier je me comprend)
********************************************************/
public class ArrowslitNet : TrapNet
{
    [SerializeField]
    GameObject arrow;

    [SerializeField]
    AudioSource soundArrow;

    float dtfireRate = 0.0f;
    float cooldown = 5.0f;
    float _time = 0.1f;         // dt entre chaque flèche d'une même salve

    new void Start()
    {
        base.Start();
        maxUse = PeonNet.instance.GiveTrapRecipe(0).GetTrapMaxUses();
    }

    override public void Move(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            Vector3 unitedPosition = hit.point;

            unitedPosition.y = 1.2f;

            if (Mathf.Abs(hit.normal.x) >= Mathf.Abs(hit.normal.z))
            {
                unitedPosition.z -= unitedPosition.z % 1;
            }
            else
            {
                unitedPosition.x -= unitedPosition.x % 1;
            }

            transform.LookAt(transform.position + hit.normal);
            transform.position = unitedPosition;
        }
    }

    //Surcharge Manette
    override public void Move(Vector3 unitedPosition)
    {
        unitedPosition.x -= unitedPosition.x % 1;
        unitedPosition.z -= unitedPosition.z % 1;

        if (Width % 2 != 0)
        {
            if (Width % 2 <= 1)
                unitedPosition.x -= 0.5f;
            else
                unitedPosition.x += 0.5f;
        }

        if (Height % 2 != 0)
        {
            if (Height % 2 <= 1)
                unitedPosition.z -= 0.5f;
            else
                unitedPosition.z += 0.5f;
        }
        transform.position = unitedPosition;
    }

    private void Update()
    {
        if (!isServer)
            return;

        if (!isActive)
            return;

        dtfireRate -= Time.deltaTime;

        if (dtfireRate <= 0.0f)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50))
            {
                if (hit.transform.CompareTag("Boss") || hit.transform.CompareTag("Enemy"))
                {
                    StartCoroutine(Shoot());
                    dtfireRate = cooldown;
                    maxUse--;
                    if (maxUse <= 0)
                    {
                        NetworkServer.Destroy(gameObject);
                    }
                }
            }
        }
    }

    override public void Rotate(float _f)
    {
        //ne peut pas être tourné, regarde à la perpendiculaire du mur
    }


    override public bool IsGroundedTrap()
    {
        return false;
    }
    
    private IEnumerator Shoot()
    {
        //CmdShoot(new Vector3(0, 0.17f, 0));
        ShootArrowTrap(new Vector3(0, 0.17f, 0));
        yield return new WaitForSeconds(_time);
        //CmdShoot(new Vector3(0, 0.72f, 0));
        ShootArrowTrap(new Vector3(0, 0.72f, 0));
        yield return new WaitForSeconds(_time);
        //CmdShoot(new Vector3(0, 1.21f, 0));
        ShootArrowTrap(new Vector3(0, 1.21f, 0));
    }

    public void ShootArrowTrap(Vector3 arrowPosition)
    {
        GameObject obj = Instantiate(arrow, transform.position + transform.forward + arrowPosition, Quaternion.Euler(transform.forward));
        NetworkServer.Spawn(obj);
        obj.GetComponent<Rigidbody>().velocity = transform.forward * 30 + transform.up * 5;
        //soundArrow.Play();
    }

    [Command]
    public void CmdShoot(Vector3 arrowPosition)
    {
        GameObject obj = Instantiate(arrow, transform.position + transform.forward + arrowPosition, Quaternion.Euler(transform.forward));
        NetworkServer.Spawn(obj);
        obj.GetComponent<Rigidbody>().velocity = transform.forward * 30 + transform.up * 5;
    }
}
