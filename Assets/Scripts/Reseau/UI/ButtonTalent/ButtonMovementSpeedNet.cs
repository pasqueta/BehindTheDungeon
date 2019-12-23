    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMovementSpeedNet : MonoBehaviour {

    List<CustomToggleButton> toggleList;
    EventSystemProvider eventSystem;

    CustomToggleButton tempToggle;

    [SerializeField]
    GameObject nextRoundButtonGO;

    void Start ()
    {
        toggleList = new List<CustomToggleButton>();        

        foreach (CustomToggleButton toggle in GetComponentsInChildren<CustomToggleButton>())
        {
            toggleList.Add(toggle);
        }

        toggleList[1].enabled = false;
        toggleList[1].GetComponentInChildren<Image>().enabled = false;
        toggleList[2].enabled = false;
        toggleList[2].GetComponentInChildren<Image>().enabled = false;

        eventSystem = GetComponentInChildren<EventSystemProvider>();
    }

    void Update ()
    {
        OnTalentGain();
	}

    public void OnTalentGain()
    {
        if (toggleList[0].isOn)
        {
            PeonNet.instance.stats.movementSpeed = 10.0f;
            BossNet.instance.stats.movementSpeed = 10.0f;
            toggleList[1].enabled = true;
            toggleList[1].GetComponentInChildren<Image>().enabled = true;
            PlayerPrefs.SetInt("peonTalentMovementSpeed", 10);
        }
        if (toggleList[1].isOn)
        {
            PeonNet.instance.stats.movementSpeed = 15.0f;
            BossNet.instance.stats.movementSpeed = 15.0f;
            toggleList[2].enabled = true;
            toggleList[2].GetComponentInChildren<Image>().enabled = true;
            PlayerPrefs.SetInt("peonTalentMovementSpeed", 15);
        }
        if (toggleList[2].isOn)
        {
            PeonNet.instance.stats.movementSpeed = 20.0f;
            BossNet.instance.stats.movementSpeed = 20.0f;
            PlayerPrefs.SetInt("peonTalentMovementSpeed", 20);
        }
        
        if(!toggleList[0].isOn)
        {          
            toggleList[1].enabled = false;
            toggleList[1].isOn = false;
            toggleList[1].GetComponentInChildren<Image>().enabled = false;

            toggleList[2].enabled = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponentInChildren<Image>().enabled = false;

            if(GetComponentInParent<Lifebar>() != null)
            {
                BossNet.instance.stats.movementSpeed = 5.0f;
                PlayerPrefs.SetInt("bossTalentMovementSpeed", 5);
            }
            else
            {
                PeonNet.instance.stats.movementSpeed = 5.0f;
                PlayerPrefs.SetInt("peonTalentMovementSpeed", 5);
            }

            
        }

        if(!toggleList[1].isOn)
        {
            toggleList[2].enabled = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponentInChildren<Image>().enabled = false;
                        
        }

        if(GetComponentInParent<Lifebar>() != null)
        {
            if (WinLooseManagerNet.instance.bossTalentPoints <= 0)
            {
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = false;
                }
            }
            else
            {
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = true;
                }
            }
        }
        else
        {
            if (WinLooseManagerNet.instance.peonTalentPoints <= 0)
            {
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = false;
                }
            }
            else
            {
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = true;
                }
            }
        }        

        
        if(tempToggle != null && tempToggle.isOn  && GetComponentInParent<Lifebar>() == null)
        {
            if (PeonNet.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.Escape))
            {
                WinLooseManagerNet.instance.peonTalentPoints++;

                tempToggle.isOn = false;
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = true;
                }

                nextRoundButtonGO.GetComponent<Toggle>().isOn = true;
                WinLooseManagerNet.instance.peonClickOK = false;

                eventSystem.eventSystem.SetSelectedGameObject(toggleList[0].gameObject);
            }

            if ((PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2) 
                && Input.GetButtonDown(Peon.instance.Controller + "_B"))
            {
                WinLooseManagerNet.instance.peonTalentPoints++;

                tempToggle.isOn = false;
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = true;
                }

                nextRoundButtonGO.GetComponent<Toggle>().isOn = true;
                WinLooseManagerNet.instance.peonClickOK = false;

                eventSystem.eventSystem.SetSelectedGameObject(toggleList[0].gameObject);
            }
        }
        else if(tempToggle != null && tempToggle.isOn  && GetComponentInParent<Lifebar>() != null)
        {
            if (BossNet.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.Escape))
            {
                WinLooseManagerNet.instance.bossTalentPoints++;

                tempToggle.isOn = false;
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = true;
                }

                nextRoundButtonGO.GetComponent<Toggle>().isOn = true;
                WinLooseManagerNet.instance.bossClickOK = false;
                eventSystem.eventSystem.SetSelectedGameObject(toggleList[0].gameObject);
            }

            if ((BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2)
                && Input.GetButtonDown(Boss.instance.Controller + "_B"))
            {
                WinLooseManagerNet.instance.bossTalentPoints++;

                tempToggle.isOn = false;
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = true;
                }

                nextRoundButtonGO.GetComponent<Toggle>().isOn = true;
                WinLooseManagerNet.instance.bossClickOK = false;

                eventSystem.eventSystem.SetSelectedGameObject(toggleList[0].gameObject);
            }
        }
    }

    public void talentSelected()
    {
        if(tempToggle == null)
        {
            if (GetComponentInParent<Lifebar>() != null)
            {
                WinLooseManagerNet.instance.bossTalentPoints--;
            }
            else
            {
                WinLooseManagerNet.instance.peonTalentPoints--;
            }
        }
        tempToggle = eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>();

        eventSystem.eventSystem.SetSelectedGameObject(nextRoundButtonGO);
    }
}
