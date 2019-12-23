using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTalent : MonoBehaviour
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
    [SerializeField] GameObject nextRoundButton;

    List<CustomToggleButton> toggleList;

    [HideInInspector]
    public EventSystemProvider eventSystem;

    Navigation navigationButton1;
    Navigation navigationButton2;
    Navigation navigationButton3;
    Navigation navigationNextRound;

    bool isScolded = false;

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
                toggleList[1].interactable = false;
                toggleList[2].interactable = false;
                toggleList[1].GetComponent<CustomToggleButton>().enabled = false;
                toggleList[2].GetComponent<CustomToggleButton>().enabled = false;
                toggleList[1].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                navigationButton1.selectOnUp = nextRoundButton.GetComponent<Toggle>();
                navigationButton1.selectOnDown = nextRoundButton.GetComponent<Toggle>();

                if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
                {
                    navigationNextRound.selectOnUp = toggleList[0];
                }
                break;
            case 1:
                toggleList[0].isOn = true;
                toggleList[2].interactable = false;
                toggleList[2].GetComponent<CustomToggleButton>().enabled = false;
                toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                break;
            case 2:
                toggleList[0].isOn = true;
                toggleList[1].isOn = true;
                toggleList[1].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(1, 1, 1, 1);
                break;
            case 3:
                toggleList[0].isOn = true;
                toggleList[1].isOn = true;
                toggleList[2].isOn = true;
                toggleList[1].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(1, 1, 1, 1);
                toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(1, 1, 1, 1);
                break;
            default:
                toggleList[1].interactable = false;
                toggleList[2].interactable = false;
                toggleList[1].GetComponent<CustomToggleButton>().enabled = false;
                toggleList[2].GetComponent<CustomToggleButton>().enabled = false;
                toggleList[1].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                break;
        }

        navigationButton1 = toggleList[0].navigation;
        navigationButton2 = toggleList[1].navigation;
        navigationButton3 = toggleList[2].navigation;
        navigationNextRound = nextRoundButton.GetComponent<Toggle>().navigation;

        if (toggleList[0].isOn)
        {
            navigationButton1.selectOnDown = toggleList[1];
            navigationButton1.selectOnUp = nextRoundButton.GetComponent<Toggle>();

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[1];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }
        else
        {
            navigationButton1.selectOnDown = nextRoundButton.GetComponent<Toggle>();
            toggleList[0].navigation = navigationButton1;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[0];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }

        if (toggleList[1].isOn)
        {
            navigationButton2.selectOnDown = toggleList[2];
            navigationButton2.selectOnUp = toggleList[0];
            
            toggleList[1].navigation = navigationButton2;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[2];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }
        else if(toggleList[0].isOn == true)
        {
            navigationButton2.selectOnDown = nextRoundButton.GetComponent<Toggle>();
            navigationButton2.selectOnUp = toggleList[0];
            
            toggleList[1].navigation = navigationButton2;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[1];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }

        if (toggleList[2].isOn)
        {
            navigationButton3.selectOnUp = toggleList[2];
            navigationButton3.selectOnDown = nextRoundButton.GetComponent<Toggle>();

            toggleList[2].navigation = navigationButton3;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[2];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }

        eventSystem = GetComponentInChildren<EventSystemProvider>();
    }

    public void OnTalentGain()
    {
        navigationButton1 = toggleList[0].navigation;
        navigationButton2 = toggleList[1].navigation;
        navigationButton3 = toggleList[2].navigation;

        if (toggleList[0].isOn)
        {
            toggleList[1].interactable = true;
            toggleList[1].GetComponent<CustomToggleButton>().enabled = true;
            toggleList[1].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(1, 1, 1, 1);

            navigationButton1.selectOnDown = toggleList[1];
            navigationButton1.selectOnUp = nextRoundButton.GetComponent<Toggle>();

            toggleList[0].navigation = navigationButton1;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[1];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }
        else if (!toggleList[0].isOn)
        {
            toggleList[1].interactable = false;
            toggleList[1].isOn = false;
            toggleList[1].GetComponent<CustomToggleButton>().enabled = false;
            toggleList[1].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);

            toggleList[2].interactable = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponent<CustomToggleButton>().enabled = false;
            toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);

            navigationButton1.selectOnDown = nextRoundButton.GetComponent<Toggle>();
            toggleList[0].navigation = navigationButton1;


            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[0];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }

        if (toggleList[1].isOn)
        {
            toggleList[2].interactable = true;
            toggleList[2].GetComponent<CustomToggleButton>().enabled = true;
            toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(1, 1, 1, 1);

            navigationButton2.selectOnDown = toggleList[2];
            navigationButton2.selectOnUp = toggleList[0];

            toggleList[1].navigation = navigationButton2;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[2];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }
        else
        {
            toggleList[2].interactable = false;
            toggleList[2].isOn = false;
            toggleList[2].GetComponent<CustomToggleButton>().enabled = false;
            toggleList[2].GetComponent<CustomToggleButton>().targetGraphic.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);

            navigationButton2.selectOnDown = nextRoundButton.GetComponent<Toggle>();
            navigationButton2.selectOnUp = toggleList[0];

            toggleList[1].navigation = navigationButton2;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[1];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }

        if (toggleList[2].isOn)
        {
            navigationButton3.selectOnUp = toggleList[2];
            navigationButton3.selectOnDown = nextRoundButton.GetComponent<Toggle>();

            toggleList[2].navigation = navigationButton3;

            if (talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
            {
                navigationNextRound.selectOnUp = toggleList[2];
                nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
            }
        }

        if (!toggleList[0].isOn && talentKey == TalentsList.bossTalentDash || talentKey == TalentsList.peonTalentInventory)
        {
            navigationNextRound.selectOnUp = toggleList[0];
            nextRoundButton.GetComponent<Toggle>().navigation = navigationNextRound;
        }
    }

    public void talentSelected()
    {
        if ((talentKey == TalentsList.peonTalentMovementSpeed || talentKey == TalentsList.peonTalentCraftingSpeed
         || talentKey == TalentsList.peonTalentGatheringSpeed || talentKey == TalentsList.peonTalentInventory
         || talentKey == TalentsList.peonTalentPickaxe) && DataManager.instance.peonCurrentTalentPoints <= 0)
        {
            if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>().isOn == true)
            {
                eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>().isOn = false;
                isScolded = true;
            }
        }
        else if ((talentKey == TalentsList.bossTalentMovementSpeed || talentKey == TalentsList.bossTalentDash
        || talentKey == TalentsList.bossTalentWhirlwind || talentKey == TalentsList.bossTalentAttack
        || talentKey == TalentsList.bossTalentHealth) && DataManager.instance.bossCurrentTalentPoints <= 0)
        {
            if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>().isOn == true)
            {
                eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>().isOn = false;
                isScolded = true;
            }
        }

        if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>().isOn)
        {
            if ((talentKey == TalentsList.peonTalentMovementSpeed || talentKey == TalentsList.peonTalentCraftingSpeed
            || talentKey == TalentsList.peonTalentGatheringSpeed || talentKey == TalentsList.peonTalentInventory
            || talentKey == TalentsList.peonTalentPickaxe) && DataManager.instance.peonCurrentTalentPoints > 0)
            {
                DataManager.instance.peonCurrentTalentPoints--;

                if (DataManager.instance.peonCurrentTalentPoints <= 0)
                {
                    eventSystem.eventSystem.SetSelectedGameObject(nextRoundButton);
                }
            }
            else if ((talentKey == TalentsList.bossTalentMovementSpeed || talentKey == TalentsList.bossTalentDash
            || talentKey == TalentsList.bossTalentWhirlwind || talentKey == TalentsList.bossTalentAttack
            || talentKey == TalentsList.bossTalentHealth) && DataManager.instance.bossCurrentTalentPoints > 0)
            {
                DataManager.instance.bossCurrentTalentPoints--;

                if (DataManager.instance.bossCurrentTalentPoints <= 0)
                {
                    eventSystem.eventSystem.SetSelectedGameObject(nextRoundButton);
                }
            }

        }
        else
        {
            if (!isScolded && DataManager.instance.peonCurrentTalentPoints >= 0 && DataManager.instance.peonCurrentTalentPoints <= DataManager.instance.peonMaxTalentPoints
           && (talentKey == TalentsList.peonTalentMovementSpeed || talentKey == TalentsList.peonTalentCraftingSpeed
          || talentKey == TalentsList.peonTalentGatheringSpeed || talentKey == TalentsList.peonTalentInventory
          || talentKey == TalentsList.peonTalentPickaxe))
            {
                if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[0]
                    && !toggleList[1].isOn)
                    DataManager.instance.peonCurrentTalentPoints += 1;
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[0]
                    && toggleList[1].isOn && !toggleList[2].isOn)
                    DataManager.instance.peonCurrentTalentPoints += 2;
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[0]
                    && toggleList[1].isOn && toggleList[2].isOn)
                    DataManager.instance.peonCurrentTalentPoints += 3;
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[1]
                    && !toggleList[2].isOn)
                    DataManager.instance.peonCurrentTalentPoints += 1;
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[1]
                    && toggleList[2].isOn)
                    DataManager.instance.peonCurrentTalentPoints += 2;
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[2])
                    DataManager.instance.peonCurrentTalentPoints += 1;
            }
            else if (!isScolded && DataManager.instance.bossCurrentTalentPoints >= 0
                && DataManager.instance.bossCurrentTalentPoints <= DataManager.instance.bossMaxTalentPoints
                && (talentKey == TalentsList.bossTalentMovementSpeed || talentKey == TalentsList.bossTalentDash
                || talentKey == TalentsList.bossTalentWhirlwind || talentKey == TalentsList.bossTalentAttack
                || talentKey == TalentsList.bossTalentHealth))
            {
                if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[0]
                    && !toggleList[1].isOn)
                {
                    DataManager.instance.bossCurrentTalentPoints += 1;
                }
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[0]
                    && toggleList[1].isOn && !toggleList[2].isOn)
                {
                    DataManager.instance.bossCurrentTalentPoints += 2;
                }
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[0]
                    && toggleList[1].isOn && toggleList[2].isOn)
                {
                    DataManager.instance.bossCurrentTalentPoints += 3;
                }
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[1]
                   && !toggleList[2].isOn)
                {
                    DataManager.instance.bossCurrentTalentPoints += 1;
                }
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[1]
                    && toggleList[2].isOn)
                {
                    DataManager.instance.bossCurrentTalentPoints += 2;
                }
                else if (eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>() == toggleList[2])
                {
                    DataManager.instance.bossCurrentTalentPoints += 1;
                }
            }
        }

        isScolded = false;
        OnTalentGain();
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
