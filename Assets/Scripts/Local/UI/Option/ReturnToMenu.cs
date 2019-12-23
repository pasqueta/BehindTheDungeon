using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Il faut mettre ce script dans le script button function

public class ReturnToMenu : MonoBehaviour
{
    public void onReturnOption()
    {
        FindObjectOfType<Prototype.NetworkLobby.LobbyManager>().GoBackButton();
        //SceneManager.LoadScene(0);
    }

    public void onReturnMenu()
    {
       
    }
}
