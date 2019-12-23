using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraController.camBoss.EnableRotation = false;
        CameraController.camPeon.EnableRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            Destroy(gameObject);
        }

        if (Input.GetButtonDown("J1_X") || Input.GetButtonDown("J2_X"))
        {
            SwitchToMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("J1_Start") || Input.GetButtonDown("J2_Start"))
        {
            Resume();
        }

        //if (WinLooseManager.instance != null)
        //{
        //    if (DataManager.instance.isUiActive || WinLooseManager.instance.gameIsAlreadyEnd)
        //    {
        //        Resume();
        //    }
        //}
        else if (WinLooseManagerNet.instance != null)
        {
            if (DataManager.instance.isUiActive || WinLooseManagerNet.instance.gameIsAlreadyEnd)
            {
                Resume();
            }
        }

        if (DataManager.instance.destroyTheOptionCanvas)
        {
            DataManager.instance.destroyTheOptionCanvas = false;
            Resume();
        }


    }

    public void Resume()
    {
        CameraController.camBoss.EnableRotation = true;
        CameraController.camPeon.EnableRotation = true;
        //Cursor.lockState = DataManager.instance.previousMode;
        if (WinLooseManager.instance)
        {
            Boss.instance.isBusy = false;
            Peon.instance.isBusy = false;

        }
        else if (WinLooseManagerNet.instance)
        {
            BossNet.instance.isBusy = false;
            PeonNet.instance.isBusy = false;
        }
        CameraController.camBoss.EnableRotation = true;
        CameraController.camPeon.EnableRotation = true;


        Time.timeScale = 1.0f;

        ////redémarrage des spawners
        //for (int i = 0; i < AllSpawner.instance.transform.childCount; i++)
        //{
        //    AllSpawner.instance.transform.GetChild(i).GetComponent<PopThings>().SetPlay();
        //}

        ////redémarrage des ennemis
        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        //{
        //    go.GetComponent<Enemy>().SetPlay();
        //}

        Destroy(gameObject);
    }

    public void SwitchToMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            FindObjectOfType<Prototype.NetworkLobby.LobbyManager>().GoBackButton();
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("SceneMenu");
    }
}
