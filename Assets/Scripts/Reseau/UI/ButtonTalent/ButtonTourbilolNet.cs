using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTourbilolNet : MonoBehaviour
{

    List<Toggle> toggleList;
    EventSystemProvider eventSystem;

    CustomToggleButton tempToggle;

    [SerializeField]
    GameObject nextRoundButtonGO;

    void Start()
    {
        toggleList = new List<Toggle>();

        foreach (Toggle toggle in GetComponentsInChildren<Toggle>())
        {
            toggleList.Add(toggle);
        }
        toggleList[1].enabled = false;
        toggleList[1].GetComponentInChildren<Image>().enabled = false;
        toggleList[2].enabled = false;
        toggleList[2].GetComponentInChildren<Image>().enabled = false;

        eventSystem = GetComponentInChildren<EventSystemProvider>();
    }

    void Update()
    {
        OnTalentGain();
    }

    public void OnTalentGain()
    {
        if (toggleList[0].isOn)
        {
            BossNet.instance.stats.whirlwind = 10;
            toggleList[1].enabled = true;
            toggleList[1].GetComponentInChildren<Image>().enabled = true;
        }
        if (toggleList[1].isOn)
        {
            BossNet.instance.stats.whirlwind = 15;
            toggleList[2].enabled = true;
            toggleList[2].GetComponentInChildren<Image>().enabled = true;
        }
        if (toggleList[2].isOn)
        {
            BossNet.instance.stats.whirlwind = 20;
        }

        if (!toggleList[0].isOn)
        {
            toggleList[1].enabled = false;
            toggleList[1].isOn = false;
            toggleList[1].GetComponentInChildren<Image>().enabled = false;

            toggleList[2].enabled = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponentInChildren<Image>().enabled = false;
        }

        if (!toggleList[1].isOn)
        {
            toggleList[2].enabled = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponentInChildren<Image>().enabled = false;
        }

        if (!toggleList[0].isOn)
            BossNet.instance.stats.whirlwind = 5;

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

        if (tempToggle != null && tempToggle.isOn && GetComponentInParent<Lifebar>() != null)
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
        if (tempToggle == null)
        {
            WinLooseManagerNet.instance.bossTalentPoints--;
        }

        tempToggle = eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>();
        eventSystem.eventSystem.SetSelectedGameObject(nextRoundButtonGO);
    }
}