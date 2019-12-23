using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour {

    GameObject currentHelpMenu = null;

    [SerializeField] GameObject fr;
    [SerializeField] GameObject en;
    void Start () {
		
	}
	
	void Update () {
        if (Boss.instance)
        {
            if (Boss.instance.Controller == Controller.K1)
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
            else if (Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2)
            {
                if (Input.GetAxis(Boss.instance.Controller + "_RT") > 0 || Input.GetAxis(Boss.instance.Controller + "_LT") > 0)
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
        else if(BossNet.instance)
        {
            if (BossNet.instance.Controller == Controller.K1)
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
            else if (BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2)
            {
                if (Input.GetAxis(BossNet.instance.Controller + "_RT") > 0 || Input.GetAxis(BossNet.instance.Controller + "_LT") > 0)
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
