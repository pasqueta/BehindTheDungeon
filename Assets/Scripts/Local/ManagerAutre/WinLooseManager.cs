using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WinLooseManager : MonoBehaviour
{

    public static WinLooseManager instance;

    [SerializeField]
    Text stateText;
    [SerializeField]
    Text conditionText;

    [SerializeField]
    AudioSource victoryFanfare;

    [SerializeField]
    AudioSource youLoose;

    [SerializeField]
    Canvas peonCanvas;

    [SerializeField]
    Canvas bossCanvas;

    [SerializeField]
    EventSystem eventSystemToEnable;

    [SerializeField]
    GameObject mainTheme;

    [HideInInspector]
    public bool gameIsAlreadyEnd = false;

    //skill Tree end Round
    [HideInInspector]
    public bool peonClickOK = false;
    [HideInInspector]
    public bool bossClickOK = false;

    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        
        //mainTheme = GameObject.FindGameObjectWithTag("MainTheme");
    }

    private void Update()
    {
        if (gameIsAlreadyEnd && (Input.GetButtonDown("K1_Space") || Input.GetButtonDown("J1_A") || Input.GetButtonDown("J2_A")))
        {
            peonCanvas.GetComponent<TalentDisplay>().DisplayTalent();
            bossCanvas.GetComponent<TalentDisplay>().DisplayTalent();

            OnEndGame();
        }
    }

    public void EndGame(bool isVictory, string condition)
    {
       
        Peon.instance.GetCameraController().EnableRotation = false;
        Boss.instance.GetCameraController().EnableRotation = false;
        Peon.instance.isBusy = true;
        Peon.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Boss.instance.isBusy = true;
        Boss.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Boss.instance.GetComponent<Animator>().SetFloat("velocity", 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;        
        if (!gameIsAlreadyEnd)
        {

            if(DataManager.instance.peonMaxTalentPoints < 15 && DataManager.instance.bossMaxTalentPoints < 15)
            {
                DataManager.instance.peonMaxTalentPoints++;
                DataManager.instance.bossMaxTalentPoints++;

                DataManager.instance.peonCurrentTalentPoints++;
                DataManager.instance.bossCurrentTalentPoints++;
            }

            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                c.enabled = false;
            }
            GetComponent<Canvas>().enabled = true;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                if (child.GetComponent<Button>())
                {
                    eventSystemToEnable.gameObject.SetActive(true);
                    eventSystemToEnable.SetSelectedGameObject(child.gameObject);
                }
            }
            if (isVictory)
            {
                stateText.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 78];
                stateText.color = Color.blue;
                conditionText.color = Color.blue;

                if (victoryFanfare)
                {
                    victoryFanfare.Play();
                }
            }
            else
            {
                stateText.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 79]; ;
                stateText.color = Color.red;
                conditionText.color = Color.red;

                if (youLoose)
                {
                    mainTheme.GetComponent<AudioSource>().Stop();
                    youLoose.Play();
                }

            }

            conditionText.text = condition;
            gameIsAlreadyEnd = true;
        }
    }

    public void OnButtonClick()
    {
        if (bossClickOK && peonClickOK)
        {
            foreach (ButtonTalent item in CanvasManager.instance.GetCanvasPeon().GetComponentsInChildren<ButtonTalent>())
            {
                item.OnSave();
            }

            eventSystemToEnable.gameObject.SetActive(false);

            foreach (ButtonTalent item in CanvasManager.instance.GetCanvasBoss().GetComponentsInChildren<ButtonTalent>())
            {
                item.OnSave();
            }

            if (DataManager.instance.currentManche < DataManager.instance.nbManches)
            {
                DataManager.instance.NextManche();
            }
            else
            {
                DataManager.instance.currentManche = 1;
                DataManager.instance.BackToMenu();
            }
        }
    }

    public void OnEndGame()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void OnPeonClickOK()
    {
        peonClickOK = true;
    }

    public void OnBossClickOK()
    {
        bossClickOK = true;
    }
}
