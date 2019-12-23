using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { FindObjectOfType<Prototype.NetworkLobby.LobbyManager>().GoBackButton(); });
	}

    //Pour yann
    //private void Update()
    //{
    //    Navigation nav =  GetComponent<Button>().navigation;
    //    nav.selectOnDown = GetComponent<Button>();
    //    GetComponent<Button>().navigation = nav;
    //}
}
