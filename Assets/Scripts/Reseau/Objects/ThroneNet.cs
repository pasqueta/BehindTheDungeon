using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneNet : MonoBehaviour
{
    float regenTimer = 0;

    [SerializeField]
    GameObject inverseMesh;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<BossNet>())
        {
            BossNet.instance.nearOfInteractibleObject = true;
            if (!BossNet.instance.GetComponent<Animator>().GetBool("Sit"))
            {
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(true);
            }
            else
            {
                CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
            }

            if ((BossNet.instance.Controller == Controller.K1 && Input.GetButtonDown("K1_E")) || 
               ((BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2) && Input.GetButtonDown(BossNet.instance.Controller + "_A")))
            {
                BossNet.instance.GetComponent<Animator>().SetBool("Sit", !BossNet.instance.GetComponent<Animator>().GetBool("Sit"));
                GetComponent<BoxCollider>().enabled = !BossNet.instance.GetComponent<Animator>().GetBool("Sit");
                BossNet.instance.isBusy = BossNet.instance.GetComponent<Animator>().GetBool("Sit");
                BossNet.instance.GetComponent<Rigidbody>().isKinematic = BossNet.instance.GetComponent<Animator>().GetBool("Sit");
                BossNet.instance.GetComponent<Rigidbody>().useGravity = !BossNet.instance.GetComponent<Animator>().GetBool("Sit");
                BossNet.instance.transform.position = BossNet.instance.GetComponent<Animator>().GetBool("Sit") ? new Vector3(-17.808f, 1.71f, -0.29f) : new Vector3(-16.883f, 1.164f, -0.1f);
                BossNet.instance.transform.rotation = Quaternion.Euler(0, 90, 0);

                if (BossNet.instance.GetComponent<Animator>().GetBool("Sit"))
                {
                    BossNet.instance.HideWeapon(true);
                }
                else
                {
                    BossNet.instance.HideWeapon(false);
                }
            }
            if (BossNet.instance.GetComponent<Animator>().GetBool("Sit"))
            {
                if (BossNet.instance.HealthPoint >= BossNet.instance.BossPdvMax)
                {
                    BossNet.instance.HealthPoint = BossNet.instance.BossPdvMax;
                }
                else
                {
                    regenTimer += Time.deltaTime;
                    if (regenTimer >= 1.0f)
                    {
                        regenTimer = 0;
                        BossNet.instance.HealthPoint += 5;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Boss"))
        {
            BossNet.instance.nearOfInteractibleObject = false;
            inverseMesh.SetActive(false);
            CanvasManagerNet.instance.GetActionTextBoss().SetActive(false);
        }
    }
}
