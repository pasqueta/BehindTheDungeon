using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPeon : MonoBehaviour {

    GameObject currentHelpMenu = null;

    [SerializeField] GameObject fr;
    [SerializeField] GameObject en;
    void Start () {
		
	}
	
	void Update () {
        if (Peon.instance)
        {
            if (Peon.instance.Controller == Controller.K1)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(fr, transform);
                    }
                    else if (DataManager.instance.language == DataManager.LANGUAGE.ENGLISH && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(en, transform);
                    }
                }
                else if (currentHelpMenu != null)
                {
                    Destroy(currentHelpMenu);
                }
            }
            else if (Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2)
            {
                if (Input.GetAxis(Peon.instance.Controller + "_RT") > 0 || Input.GetAxis(Peon.instance.Controller + "_LT") > 0)
                {
                    if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(fr, transform);
                    }
                    else if (DataManager.instance.language == DataManager.LANGUAGE.ENGLISH && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(en, transform);
                    }
                }
                else if (currentHelpMenu != null)
                {
                    Destroy(currentHelpMenu);
                }
            }
        }
        else if(PeonNet.instance)
        {
            if (PeonNet.instance.Controller == Controller.K1)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(fr, transform);
                    }
                    else if (DataManager.instance.language == DataManager.LANGUAGE.ENGLISH && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(en, transform);
                    }
                }
                else if (currentHelpMenu != null)
                {
                    Destroy(currentHelpMenu);
                }
            }
            else if (PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2)
            {
                if (Input.GetAxis(PeonNet.instance.Controller + "_RT") > 0 || Input.GetAxis(PeonNet.instance.Controller + "_LT") > 0)
                {
                    if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(fr, transform);
                    }
                    else if (DataManager.instance.language == DataManager.LANGUAGE.ENGLISH && currentHelpMenu == null)
                    {
                        currentHelpMenu = Instantiate(en, transform);
                    }
                }
                else if (currentHelpMenu != null)
                {
                    Destroy(currentHelpMenu);
                }
            }
        }
    }
}
