using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************************************
 * Auteur : Paul
 * 
 * Resumé : 3 flêches partent du mur pour se planter dans un truc
 * condition de trigger : boss ou ennemies passent devant la meurtrière
 * Au déclanchement : tir 3 flèches
 * réutilisable : oui
 * cooldown : 5 s
 * 
 * MaJ futur : bien refaire les normals (faites pas chier je me comprend)
********************************************************/
public class Arrowslit : Trap {
    
    [SerializeField]
    GameObject arrow;
    
    [SerializeField]
    AudioSource soundArrow;

    float dtfireRate = 0.0f;    
    float cooldown = 5.0f;      
    float _time = 0.1f;         // dt entre chaque flèche d'une même salve    

    override public void Move(Ray ray)
    {
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            //Debug.DrawRay(hit.point, hit.normal, Color.red);
            Vector3 unitedPosition = hit.point;

            unitedPosition.y = 1.2f;
            
            if (Mathf.Abs(hit.normal.x) >= Mathf.Abs(hit.normal.z))
                unitedPosition.z -= unitedPosition.z % 1;
            else
                unitedPosition.x -= unitedPosition.x % 1;

            /*
            if (Width % 2 != 0)
            {
                if (Width % 2 <= 1) unitedPosition.z -= 0.5f;
                else                unitedPosition.z += 0.5f;
            }
            
            if (Height % 2 != 0) 
            {
                if (Height % 2 <= 1)    unitedPosition.z -= 0.5f;
                else                    unitedPosition.z += 0.5f;
            }
            */

            //if (hit.normal != Vector3.forward || hit.normal == Vector3.back || hit.normal == Vector3.right || hit.normal == Vector3.left)
            
            transform.LookAt(transform.position + hit.normal);
            transform.position = unitedPosition;
        }
    }

    //Surcharge Manette
    override
    public void Move(Vector3 unitedPosition)
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
        if (isActive)
        {
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
                            Destroy(gameObject);
                        }
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
        GameObject arrowB = Instantiate(arrow, transform.position + transform.forward + new Vector3(0, 0.17f, 0), Quaternion.Euler(transform.forward));
        arrowB.GetComponent<Rigidbody>().velocity = transform.forward * 30 + transform.up * 5;
        soundArrow.Play();
        yield return new WaitForSeconds(_time);
        GameObject arrowM = Instantiate(arrow, transform.position + transform.forward+ new Vector3(0, 0.72f, 0), Quaternion.Euler(transform.forward));
        arrowM.GetComponent<Rigidbody>().velocity = transform.forward * 30 + transform.up * 5;
        soundArrow.Play();
        yield return new WaitForSeconds(_time);
        GameObject arrowT = Instantiate(arrow, transform.position + transform.forward + new Vector3(0, 1.21f, 0), Quaternion.Euler(transform.forward));
        arrowT.GetComponent<Rigidbody>().velocity = transform.forward * 30 + transform.up * 5;
        soundArrow.Play();
    }


}
