using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTalentNet : MonoBehaviour
{
    public enum TalentsList
    {
        peonTalentMovementSpeed,
        peonTalentGatheringSpeed,
        peonTalentPickaxe,
        peonTalentCraftingSpeed,
        peonTalentInventory,
        bossTalentMovementSpeed,
        bossTalentAttack,
        bossTalentHealth,
        bossTalentDash,
        bossTalentWhirlwind
    }
    [SerializeField] TalentsList talentKey;

    List<CustomToggleButton> toggleList;

    CustomToggleButton tempToggle;

    [SerializeField]
    GameObject nextRoundButtonGO;

    EventSystemProvider eventSystem;

    //bool isOkey = false;

    void Start()
    {
        toggleList = new List<CustomToggleButton>();

        foreach (CustomToggleButton toggle in GetComponentsInChildren<CustomToggleButton>())
        {
            toggleList.Add(toggle);
        }

        switch (PlayerPrefs.GetInt(talentKey.ToString()))
        {
            case 0:
                toggleList[1].enabled = false;
                toggleList[1].GetComponentInChildren<Image>().enabled = false;
                toggleList[2].enabled = false;
                toggleList[2].GetComponentInChildren<Image>().enabled = false;
                break;
            case 1:
                toggleList[0].isOn = true;
                toggleList[2].enabled = false;
                toggleList[2].GetComponentInChildren<Image>().enabled = false;
                break;
            case 2:
                toggleList[0].isOn = true;
                toggleList[1].isOn = true;
                break;
            case 3:
                toggleList[0].isOn = true;
                toggleList[1].isOn = true;
                toggleList[2].isOn = true;
                break;
            default:
                toggleList[1].enabled = false;
                toggleList[1].GetComponentInChildren<Image>().enabled = false;
                toggleList[2].enabled = false;
                toggleList[2].GetComponentInChildren<Image>().enabled = false;
                break;
        }

        eventSystem = GetComponentInChildren<EventSystemProvider>();

        
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].onValueChanged.AddListener(delegate {
                talentSelected();
            });
        }
    }

    void Update()
    {
        OnTalentGain();
    }

    public void OnTalentGain()
    {
        if (toggleList[0].isOn)
        {
            toggleList[1].enabled = true;
            toggleList[1].GetComponentInChildren<Image>().enabled = true;
            toggleList[1].GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            toggleList[1].enabled = false;
            toggleList[1].isOn = false;
            toggleList[1].GetComponentInChildren<Image>().enabled = false;
            toggleList[1].GetComponentInChildren<Text>().enabled = false;

            toggleList[2].enabled = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponentInChildren<Image>().enabled = false;
            toggleList[2].GetComponentInChildren<Text>().enabled = false;
        }

        if (toggleList[1].isOn)
        {
            toggleList[2].enabled = true;
            toggleList[2].GetComponentInChildren<Image>().enabled = true;
            toggleList[2].GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            toggleList[2].enabled = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponentInChildren<Image>().enabled = false;
            toggleList[2].GetComponentInChildren<Text>().enabled = false;
        }

        if (talentKey == TalentsList.peonTalentMovementSpeed || talentKey == TalentsList.peonTalentCraftingSpeed 
            || talentKey == TalentsList.peonTalentGatheringSpeed || talentKey == TalentsList.peonTalentInventory 
            || talentKey == TalentsList.peonTalentPickaxe)
        {
            if(WinLooseManagerNet.instance.peonTalentPoints <= 0)
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

        if (tempToggle != null && tempToggle.isOn && (talentKey == TalentsList.peonTalentMovementSpeed || talentKey == TalentsList.peonTalentCraftingSpeed
            || talentKey == TalentsList.peonTalentGatheringSpeed || talentKey == TalentsList.peonTalentInventory
            || talentKey == TalentsList.peonTalentPickaxe))
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
        else if (tempToggle != null && tempToggle.isOn && (talentKey == TalentsList.bossTalentAttack
            || talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.bossTalentHealth
            || talentKey == TalentsList.bossTalentMovementSpeed || talentKey == TalentsList.bossTalentWhirlwind))
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
                && Input.GetButtonDown(BossNet.instance.Controller + "_B"))
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
            if (talentKey == TalentsList.peonTalentMovementSpeed || talentKey == TalentsList.peonTalentCraftingSpeed
            || talentKey == TalentsList.peonTalentGatheringSpeed || talentKey == TalentsList.peonTalentInventory
            || talentKey == TalentsList.peonTalentPickaxe)
            {
                WinLooseManagerNet.instance.peonTalentPoints--;
            }
            else
            {
                WinLooseManagerNet.instance.bossTalentPoints--;
            }
        }

        tempToggle = eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>();

        eventSystem.eventSystem.SetSelectedGameObject(nextRoundButtonGO);

    }

    public void OnSave()
    {
        if (toggleList[2].isOn)
            PlayerPrefs.SetInt(talentKey.ToString(), 3);
        else if (toggleList[1].isOn)
            PlayerPrefs.SetInt(talentKey.ToString(), 2);
        else if (toggleList[0].isOn)
            PlayerPrefs.SetInt(talentKey.ToString(), 1);
    }
}
