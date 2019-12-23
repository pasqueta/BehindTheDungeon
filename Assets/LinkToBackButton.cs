using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkToBackButton : MonoBehaviour {

    // Use this for initialization
    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (!GetComponent<Button>().navigation.selectOnUp)
        {
            Navigation nav = GetComponent<Button>().navigation;
            // Debug.Log(GetComponentsInParent<Prototype.NetworkLobby.LobbyPlayerList>()[0].backButton);
            if (GetComponentInParent<Prototype.NetworkLobby.LobbyPlayerList>())
            {
                nav.selectOnUp = GetComponentsInParent<Prototype.NetworkLobby.LobbyPlayerList>()[0].backButton;
            }

            GetComponent<Button>().navigation = nav;
        }
    }
}
