using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WinLooseManagerNet : NetworkBehaviour
{
    public static WinLooseManagerNet instance;

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

    [SyncVar, HideInInspector]
    public bool gameIsAlreadyEnd = false;

    //skill Tree end Round
    [HideInInspector]
    public bool peonClickOK = false;
    [HideInInspector]
    public bool bossClickOK = false;
    [HideInInspector]
    public int peonTalentPoints = 0;
    [HideInInspector]
    public int bossTalentPoints = 0;

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
            //peonCanvas.GetComponent<TalentDisplayNet>().DisplayTalent();
            //bossCanvas.GetComponent<TalentDisplayNet>().DisplayTalent();

            //OnEndGame();
        }
    }

    public void EndGame(bool isVictory, string condition)
    {
        PeonNet.instance.GetCameraController().EnableRotation = false;
        BossNet.instance.GetCameraController().EnableRotation = false;
        PeonNet.instance.isBusy = true;
        PeonNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        BossNet.instance.isBusy = true;
        BossNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        BossNet.instance.GetComponent<Animator>().SetFloat("velocity", 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!gameIsAlreadyEnd)
        {
            if (DataManager.instance.peonMaxTalentPoints < 15 && DataManager.instance.bossMaxTalentPoints < 15)
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
                stateText.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 78]; ;
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
                    mainTheme.GetComponentInChildren<AudioSource>().Stop();
                    youLoose.Play();
                }
            }

            conditionText.text = condition;
            gameIsAlreadyEnd = true;

            RpcEndGame(isVictory, condition);
        }
    }

    [ClientRpc]
    void RpcEndGame(bool isVictory, string condition)
    {
        PeonNet.instance.GetCameraController().EnableRotation = false;
        BossNet.instance.GetCameraController().EnableRotation = false;
        PeonNet.instance.isBusy = true;
        PeonNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        BossNet.instance.isBusy = true;
        BossNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        BossNet.instance.GetComponent<Animator>().SetFloat("velocity", 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!gameIsAlreadyEnd)
        {
            if (DataManager.instance.peonMaxTalentPoints < 15 && DataManager.instance.bossMaxTalentPoints < 15)
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
                stateText.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 78]; ;
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
                    mainTheme.GetComponentInChildren<AudioSource>().Stop();
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
            foreach (ButtonTalentNet item in CanvasManagerNet.instance.GetCanvasPeon().GetComponentsInChildren<ButtonTalentNet>())
            {
                item.OnSave();
            }

            eventSystemToEnable.gameObject.SetActive(false);

            foreach (ButtonTalentNet item in CanvasManagerNet.instance.GetCanvasBoss().GetComponentsInChildren<ButtonTalentNet>())
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
            Debug.Log("Finish");
            DataManager.instance.currentManche = 1;
            DataManager.instance.BackToMenu();
        }
    }

    public void OnEndGame()
    {
        FindObjectOfType<Prototype.NetworkLobby.LobbyManager>().GoBackButton();

        //GetComponent<Canvas>().enabled = false;
        //DataManager.instance.currentManche = 1;
        //DataManager.instance.BackToMenu();
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