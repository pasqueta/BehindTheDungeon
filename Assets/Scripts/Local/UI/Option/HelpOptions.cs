using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpOptions : MonoBehaviour {

    [SerializeField] GameObject menu;
    GameObject instance = null;

    bool tripatouillage = false;

    public GameObject Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public bool Tripatouillage
    {
        get
        {
            return tripatouillage;
        }

        set
        {
            tripatouillage = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Tripatouillage)
            Tripatouillage = false;

        if (Input.GetButtonDown("Menu_Cancel") && Instance)
        {
            Destroy(Instance);
            instance = null;
            Tripatouillage = true;
        }
    }

    public void OpenHelp()
    {
        Instance = Instantiate(menu, transform);
    }
}
