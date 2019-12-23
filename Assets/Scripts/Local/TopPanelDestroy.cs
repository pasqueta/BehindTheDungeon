using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopPanelDestroy : MonoBehaviour
{
    // Use this for initialization
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            GetComponentInParent<Prototype.NetworkLobby.LobbyTopPanel>().gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GetComponentInParent<Prototype.NetworkLobby.LobbyTopPanel>().enabled == false)
        {
            if (!(SceneManager.GetActiveScene().buildIndex == 3))
            {
                GetComponentInParent<Prototype.NetworkLobby.LobbyTopPanel>().gameObject.SetActive(true);
            }
        }
    }
}
