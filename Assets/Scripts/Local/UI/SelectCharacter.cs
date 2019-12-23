using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{

    [SerializeField]
    GameObject textChooseCharacterPeon,
                textChooseCharacterBoss,
                readyPeon,
                readyBoss,
                textReadyPeon,
                textReadyBoss,
                checkMarkPeon,
                checkMarkBoss,
                textTimer;

    bool chooseController = true,
                peonIsChoosing = true,
                peonIsReady = false,
                bossIsReady = false;

    Controller peonController,
               bossController;

    float secondToWait = 6.0f;

    void Start()
    {

    }

    void Update()
    {
        if (chooseController)
        {
            if (peonIsChoosing)
            {
                textChooseCharacterPeon.SetActive(true);
                textChooseCharacterBoss.SetActive(false);
            }
            else
            {
                textChooseCharacterPeon.SetActive(false);
                textChooseCharacterBoss.SetActive(true);
            }

            if (Input.GetButtonDown("K1_Space"))
            {
                if (peonIsChoosing)
                {
                    peonController = Controller.K1;
                    peonIsChoosing = false;
                }
                else if (peonController != Controller.K1)
                {
                    bossController = Controller.K1;
                    chooseController = false;
                }
            }
            if (Input.GetButtonDown("J1_A"))
            {
                if (peonIsChoosing)
                {
                    peonController = Controller.J1;
                    peonIsChoosing = false;
                }
                else if (peonController != Controller.J1)
                {
                    bossController = Controller.J1;
                    chooseController = false;
                }
            }
            if (Input.GetButtonDown("J2_A"))
            {
                if (peonIsChoosing)
                {
                    peonController = Controller.J2;
                    peonIsChoosing = false;
                }
                else if (peonController != Controller.J2)
                {
                    bossController = Controller.J2;
                    chooseController = false;
                }
            }

           
        }
        else
        {
            textChooseCharacterBoss.SetActive(false);
            readyPeon.SetActive(true);
            readyBoss.SetActive(true);

 			DataManager.instance.ControllerPeon = peonController;
            DataManager.instance.ControllerBoss = bossController;
            if (peonController == Controller.K1)
            {
                if (Input.GetButtonDown(peonController + "_Space"))
                    peonIsReady = !peonIsReady;
            }
            else
            {
                if (Input.GetButtonDown(peonController + "_A"))
                    peonIsReady = !peonIsReady;
            }

            if (bossController == Controller.K1)
            {
                if (Input.GetButtonDown(bossController + "_Space"))
                    bossIsReady = !bossIsReady;
            }
            else
            {
                if (Input.GetButtonDown(bossController + "_A"))
                    bossIsReady = !bossIsReady;
            }


            if (peonIsReady)
            {
                textReadyPeon.GetComponent<Text>().text = "Ready";
                checkMarkPeon.SetActive(true);
            }
            else
            {
                textReadyPeon.GetComponent<Text>().text = "Not Ready";
                checkMarkPeon.SetActive(false);
            }

            if (bossIsReady)
            {
                textReadyBoss.GetComponent<Text>().text = "Ready";
                checkMarkBoss.SetActive(true);
            }
            else
            {
                textReadyBoss.GetComponent<Text>().text = "Not Ready";
                checkMarkBoss.SetActive(false);
            }

            if (peonIsReady && bossIsReady)
            {
                if (!GetComponentInParent<ButtonFunction>().invert)
                {

                    textTimer.SetActive(true);
                    if (secondToWait >= 0)
                        textTimer.GetComponent<Text>().text = ((int)secondToWait / 1) + "";
                    else
                    {
                        GetComponentInParent<ButtonFunction>().onPlayTwoLocal();
                    }
                    secondToWait -= Time.deltaTime;
                }

            }
            else
            {
                textTimer.SetActive(false);
                secondToWait = 0.0f;
            }

        }
    }
}
