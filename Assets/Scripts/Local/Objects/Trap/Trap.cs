using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************************************
 * Auteur : Paul
 * 
 * Resumé : classe de base des pièges
 * fonctions : positionnement/rotation des pièges
 * 
 * MaJ futur : Je vous le demande ?
********************************************************/
public class Trap : MonoBehaviour
{
    [SerializeField] protected int Width;
    [SerializeField] protected int Height;

    [SerializeField] public AudioSource trapOnSound;

    [SerializeField] protected LayerMask mask;

    [SerializeField] protected bool isActive = false;
    [SerializeField] protected GameObject particle;
    protected bool placed = false;
    List<Collider> colliders;
    //List<Collision> collisions;


    protected int maxUse;
    protected int damages;


    public Trap()
    {
        colliders = new List<Collider>();
        //collisions = new List<Collision>();

        maxUse = 0;
        damages = 0;
    }

    protected void Start()
    {
        if (GetComponent<Renderer>())
            GetComponent<Renderer>().material.color = Color.green;
        foreach (Renderer c in GetComponentsInChildren<Renderer>())
            c.material.color = Color.green;

    }

    virtual public void Move(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            Vector3 unitedPosition = hit.point;

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

    }

    //Surcharge Manette
    virtual public void Move(Vector3 unitedPosition)
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

    void TrapOnSound()
    {
        if (trapOnSound && AudioTypeBehaviour.voiceIsPlaying == false)
        {
            trapOnSound.Play();
            AudioTypeBehaviour.voiceIsPlaying = true;
            Debug.Log(trapOnSound);
            Debug.Log(trapOnSound.GetComponent<AudioTypeBehaviour>());
            trapOnSound.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", trapOnSound.clip.length);
        }
    }

    public bool PlaceTrap()
    {
        if (colliders.Count != 0)
            return false;
        GetComponent<Collider>().isTrigger = true;
        placed = true;
        if (GetComponent<Renderer>())
            GetComponent<Renderer>().material.color = Color.white;
        foreach (Renderer c in GetComponentsInChildren<Renderer>())
            c.material.color = Color.white;
        if (particle)
            particle.SetActive(true);

        TrapOnSound();
        return true;
    }

    public void AddCollider(Collider c)
    {
        if (!placed)
        {
            if (colliders.Count > 0)
                foreach (Collider c2 in colliders)
                {
                    if (c2 == c)
                    {
                        return;
                    }
                }
            colliders.Add(c);
            if (GetComponent<Renderer>())
                GetComponent<Renderer>().material.color = Color.red;
            foreach (Renderer c3 in GetComponentsInChildren<Renderer>())
                c3.material.color = Color.red;
        }
    }

    public void RemoveCollider(Collider c)
    {
        if (!placed)
        {
            if (colliders.Contains(c))
            {
                colliders.Remove(c);
            }

            if (colliders.Count == 0 /*&& collisions.Count == 0*/)
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material.color = Color.green;
                else
                    foreach (Renderer c3 in GetComponentsInChildren<Renderer>())
                        c3.material.color = Color.green;
        }
    }

    virtual public void Rotate(float _f)
    {
        if (_f <= 0.0f)
            transform.Rotate(transform.up, 90);
        else
            transform.Rotate(transform.up, -90);

        int tmp = Width;
        Width = Height;
        Height = tmp;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<Trap>())
        {
            GetComponent<Trap>().AddCollider(GetComponent<Collider>());
        }
    }

    protected void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Trap>())
        {
            GetComponent<Trap>().RemoveCollider(GetComponent<Collider>());
        }
    }
    /*
    protected void OnCollisionEnter(Collision collision)
    {
        if (!placed)
        {
            if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Gold")) && !collisions.Contains(collision))
            {
                collisions.Add(collision);
            }
            if (GetComponent<Renderer>())
                GetComponent<Renderer>().material.color = Color.red;
            foreach (Renderer c3 in GetComponentsInChildren<Renderer>())
                c3.material.color = Color.red;
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (!placed)
        {
            if (collisions.Contains(collision))
            {
                collisions.Remove(collision);
                if (collisions.Count == 0 && colliders.Count == 0)
                {
                    if (GetComponent<Renderer>())
                        GetComponent<Renderer>().material.color = Color.green;
                    foreach (Renderer c3 in GetComponentsInChildren<Renderer>())
                        c3.material.color = Color.green;
                }
            }
        }
    }
    */

    virtual public bool IsGroundedTrap()
    {
        return true;
    }

    public void SetIsActive(bool _b)
    {
        isActive = _b;
    }

    //ONLY BECAUSE THERE ARE NOT THIS EXAMPLAR OF TRAP IN THE INVENTORY !!!!!!!!!!!!!!!!!
    public void SetRedColor()
    {
        if (GetComponent<Renderer>())
            GetComponent<Renderer>().material.color = Color.red;
        foreach (Renderer c3 in GetComponentsInChildren<Renderer>())
            c3.material.color = Color.red;
    }

    public int GetDamages()
    {
        return damages;
    }

    public int GetmaxUse()
    {
        return maxUse;
    }
}
