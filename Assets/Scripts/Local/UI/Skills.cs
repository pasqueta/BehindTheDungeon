using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour {

    Text textTourbi, textCharge, textJump;
    Image imgTourbi, imgCharge, imgJump;
    Text keyTourbi, keyCharge, keyJump;

	void Start () {
        Transform jump = transform.GetChild(0);
        Transform charge = transform.GetChild(1);
        Transform tourbi = transform.GetChild(2);

        imgTourbi = tourbi.GetChild(0).GetComponent<Image>();
        textTourbi = tourbi.GetChild(1).GetComponent<Text>();

        imgCharge = charge.GetChild(0).GetComponent<Image>();
        textCharge = charge.GetChild(1).GetComponent<Text>();

        imgJump = jump.GetChild(0).GetComponent<Image>();
        textJump = jump.GetChild(1).GetComponent<Text>();

        keyTourbi = transform.GetChild(3).GetChild(5).GetComponent<Text>();
        keyCharge = transform.GetChild(3).GetChild(4).GetComponent<Text>();
        keyJump = transform.GetChild(3).GetChild(3).GetComponent<Text>();
    }

    void Update () {
        if (Boss.instance != null)
        {
            if (Boss.instance.Controller == Controller.K1)
            {
                keyTourbi.text = "3";
                keyCharge.text = "2";
                keyJump.text = "1";
            }
            else if (Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2)
            {
                keyTourbi.text = "Y";
                keyCharge.text = "X";
                keyJump.text = "B";
            }

            //charge
            if (Boss.instance.DtDashCooldown > 0.01f)
            {
                imgCharge.gameObject.SetActive(true);
                textCharge.gameObject.SetActive(true);

                imgCharge.fillAmount = (Boss.instance.DashCooldown - Boss.instance.DtDashCooldown) / Boss.instance.DashCooldown;
                textCharge.text = string.Format("{0:N1}", (Boss.instance.DashCooldown - Boss.instance.DtDashCooldown));
            }
            else
            {
                imgCharge.gameObject.SetActive(false);
                textCharge.gameObject.SetActive(false);
            }

            //tourbi
            if (Boss.instance.DtWhirlwindCooldown > 0.01f)
            {
                imgTourbi.gameObject.SetActive(true);
                textTourbi.gameObject.SetActive(true);

                imgTourbi.fillAmount = (Boss.instance.WhirlwindCooldown - Boss.instance.DtWhirlwindCooldown) / Boss.instance.WhirlwindCooldown;
                textTourbi.text = string.Format("{0:N1}", (Boss.instance.WhirlwindCooldown - Boss.instance.DtWhirlwindCooldown));
            }
            else if (Boss.instance.currentWeaponIndex >= 0)
            {
                imgTourbi.gameObject.SetActive(false);
                textTourbi.gameObject.SetActive(false);
            }
            else
            {
                textTourbi.gameObject.SetActive(false);
            }

            //stomp
            if (Boss.instance.DtStompCooldown > 0.01f)
            {
                imgJump.gameObject.SetActive(true);
                textJump.gameObject.SetActive(true);

                imgJump.fillAmount = (Boss.instance.StompCooldown - Boss.instance.DtStompCooldown) / Boss.instance.StompCooldown;
                textJump.text = string.Format("{0:N1}", (Boss.instance.StompCooldown - Boss.instance.DtStompCooldown));
            }
            else
            {
                imgJump.gameObject.SetActive(false);
                textJump.gameObject.SetActive(false);
            }

        }

        if (BossNet.instance != null)
        {
            if (BossNet.instance.Controller == Controller.K1)
            {
                keyTourbi.text = "3";
                keyCharge.text = "2";
                keyJump.text = "1";
            }
            else if (BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2)
            {
                keyTourbi.text = "Y";
                keyCharge.text = "X";
                keyJump.text = "B";
            }

            //charge
            if (BossNet.instance.DtDashCooldown > 0.01f)
            {
                imgCharge.gameObject.SetActive(true);
                textCharge.gameObject.SetActive(true);

                imgCharge.fillAmount = (BossNet.instance.DashCooldown - BossNet.instance.DtDashCooldown) / BossNet.instance.DashCooldown;
                textCharge.text = string.Format("{0:N1}", (BossNet.instance.DashCooldown - BossNet.instance.DtDashCooldown));
            }
            else
            {
                imgCharge.gameObject.SetActive(false);
                textCharge.gameObject.SetActive(false);
            }

            //tourbi
            if (BossNet.instance.DtWhirlwindCooldown > 0.01f)
            {
                imgTourbi.gameObject.SetActive(true);
                textTourbi.gameObject.SetActive(true);

                imgTourbi.fillAmount = (BossNet.instance.WhirlwindCooldown - BossNet.instance.DtWhirlwindCooldown) / BossNet.instance.WhirlwindCooldown;
                textTourbi.text = string.Format("{0:N1}", (BossNet.instance.WhirlwindCooldown - BossNet.instance.DtWhirlwindCooldown));
            }
            else if (BossNet.instance.currentWeaponIndex >= 0)
            {
                imgTourbi.gameObject.SetActive(false);
                textTourbi.gameObject.SetActive(false);
            }
            else
            {
                textTourbi.gameObject.SetActive(false);
            }

            //stomp
            if (BossNet.instance.DtStompCooldown > 0.01f)
            {
                imgJump.gameObject.SetActive(true);
                textJump.gameObject.SetActive(true);

                imgJump.fillAmount = (BossNet.instance.StompCooldown - BossNet.instance.DtStompCooldown) / BossNet.instance.StompCooldown;
                textJump.text = string.Format("{0:N1}", (BossNet.instance.StompCooldown - BossNet.instance.DtStompCooldown));
            }
            else
            {
                imgJump.gameObject.SetActive(false);
                textJump.gameObject.SetActive(false);
            }

        }
    }
}
