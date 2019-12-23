using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throne : MonoBehaviour {

    float regenTimer = 0;
    int regen = 5;
    [SerializeField]
    GameObject inverseMesh;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<Boss>())
        {
            if (!Boss.instance.IsDead)
            {
                Boss.instance.nearOfInteractibleObject = true;
                if (!Boss.instance.GetComponent<Animator>().GetBool("Sit"))
                {
                    inverseMesh.GetComponent<MeshRenderer>().enabled = true;
                    CanvasManager.instance.GetActionTextBoss().SetActive(true);
                }
                else
                {
                    inverseMesh.GetComponent<MeshRenderer>().enabled = false;
                    CanvasManager.instance.GetActionTextBoss().SetActive(false);
                }

                if ((Boss.instance.Controller == Controller.K1 && Input.GetButtonDown("K1_E")) ||
                   ((Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2) && Input.GetButtonDown(Boss.instance.Controller + "_A")))
                {
                    Boss.instance.GetComponent<Animator>().SetBool("Sit", !Boss.instance.GetComponent<Animator>().GetBool("Sit"));
                    GetComponent<BoxCollider>().enabled = !Boss.instance.GetComponent<Animator>().GetBool("Sit");
                    Boss.instance.isBusy = Boss.instance.GetComponent<Animator>().GetBool("Sit");
                    Boss.instance.GetComponent<Rigidbody>().isKinematic = Boss.instance.GetComponent<Animator>().GetBool("Sit");
                    Boss.instance.GetComponent<Rigidbody>().useGravity = !Boss.instance.GetComponent<Animator>().GetBool("Sit");
                    Boss.instance.transform.position = Boss.instance.GetComponent<Animator>().GetBool("Sit") ? new Vector3(-17.808f, 1.71f, -0.29f) : new Vector3(-16.883f, 1.164f, -0.1f);
                    Boss.instance.transform.rotation = Quaternion.Euler(0, 90, 0);

                    if (Boss.instance.GetComponent<Animator>().GetBool("Sit"))
                    {
                        Boss.instance.HideWeapon(true);
                    }
                    else
                    {
                        Boss.instance.HideWeapon(false);
                    }

                    // Boss.instance.equippedWeapon = Boss.instance.GetComponent<Animator>().GetBool("Sit") ? WeaponRecipe.Weapon.NONE : Boss.instance.weaponList[Boss.instance.listIndex];
                    // Boss.instance.InstantiateBossWeapon();
                }

                if (Boss.instance.GetComponent<Animator>().GetBool("Sit"))
                {
                    if (Boss.instance.HealthPoint >= Boss.instance.BossPdvMax)
                    {
                        Boss.instance.HealthPoint = Boss.instance.BossPdvMax;
                    }
                    else
                    {
                        regenTimer += Time.deltaTime;
                        if (regenTimer >= 1.0f)
                        {
                            regenTimer = 0;
                            Boss.instance.HealthPoint += regen;
                        }
                    }
                }
            }
            else
            {
                Boss.instance.GetComponent<Animator>().SetBool("Sit", false);
                GetComponent<BoxCollider>().enabled = true;
                Boss.instance.GetComponent<Rigidbody>().isKinematic = false;
                Boss.instance.GetComponent<Rigidbody>().useGravity = true;
                Boss.instance.transform.position = new Vector3(-16.883f, 1.164f, -0.1f);
                Boss.instance.transform.rotation = Quaternion.Euler(0, 90, 0);
                Boss.instance.HideWeapon(false);
            }
	    }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Boss"))
        {
            Boss.instance.nearOfInteractibleObject = false;
            inverseMesh.GetComponent<MeshRenderer>().enabled = false;
            CanvasManager.instance.GetActionTextBoss().SetActive(false);
        }
    }
}
