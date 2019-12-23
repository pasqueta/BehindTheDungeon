using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemInit : MonoBehaviour
{
    List<CustomInputStandalone> inputStandaloneList = new List<CustomInputStandalone>();
	// Use this for initialization
	void Start ()
    {
        foreach (CustomInputStandalone eventSystem in GetComponentsInChildren<CustomInputStandalone>())
        {
            inputStandaloneList.Add(eventSystem);
        }

        for (int i = 0; i < inputStandaloneList.Count; i++)
        {
            if (!inputStandaloneList[i].forceModuleActive)
                inputStandaloneList[i].forceModuleActive = true;
        }

        if (Peon.instance != null)
        {
            if (Peon.instance.Controller == Controller.K1)
            {
                inputStandaloneList[0].horizontalAxis = Peon.instance.Controller + "_LXaxis";
                inputStandaloneList[0].verticalAxis = Peon.instance.Controller + "_LYaxis";
                inputStandaloneList[0].submitButton = Peon.instance.Controller + "_Space";
                inputStandaloneList[0].cancelButton = Peon.instance.Controller + "_E";
            } 
            else if (Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2)
            {
                inputStandaloneList[0].horizontalAxis = Peon.instance.Controller + "_LXaxis";
                inputStandaloneList[0].verticalAxis = Peon.instance.Controller + "_LYaxis";
                inputStandaloneList[0].submitButton = Peon.instance.Controller + "_A";
                inputStandaloneList[0].cancelButton = Peon.instance.Controller + "_B";
            }
        }

        if (Boss.instance != null)
        {
            if (Boss.instance.Controller == Controller.K1)
            {
                inputStandaloneList[1].horizontalAxis = Boss.instance.Controller + "_LXaxis";
                inputStandaloneList[1].verticalAxis = Boss.instance.Controller + "_LYaxis";
                inputStandaloneList[1].submitButton = Boss.instance.Controller + "_Space";
                inputStandaloneList[1].cancelButton = Boss.instance.Controller + "_E";
            }
            else if (Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2)
            {
                inputStandaloneList[1].horizontalAxis = Boss.instance.Controller + "_LXaxis";
                inputStandaloneList[1].verticalAxis = Boss.instance.Controller + "_LYaxis";
                inputStandaloneList[1].submitButton = Boss.instance.Controller + "_A";
                inputStandaloneList[1].cancelButton = Boss.instance.Controller + "_B";
            }
        }

        if (PeonNet.instance != null)
        {
            if (DataManager.instance.ControllerPeon == Controller.K1)
            {
                inputStandaloneList[0].horizontalAxis = DataManager.instance.ControllerPeon + "_LXaxis";
                inputStandaloneList[0].verticalAxis = DataManager.instance.ControllerPeon + "_LYaxis";
                inputStandaloneList[0].submitButton = DataManager.instance.ControllerPeon + "_Space";
                inputStandaloneList[0].cancelButton = DataManager.instance.ControllerPeon + "_E";
            }
            else if (DataManager.instance.ControllerPeon == Controller.J1 || DataManager.instance.ControllerPeon == Controller.J2)
            {
                inputStandaloneList[0].horizontalAxis = DataManager.instance.ControllerPeon + "_LXaxis";
                inputStandaloneList[0].verticalAxis = DataManager.instance.ControllerPeon + "_LYaxis";
                inputStandaloneList[0].submitButton = DataManager.instance.ControllerPeon + "_A";
                inputStandaloneList[0].cancelButton = DataManager.instance.ControllerPeon + "_B";
            }
        }

        if (BossNet.instance != null)
        {
            if (DataManager.instance.ControllerBoss == Controller.K1)
            {
                inputStandaloneList[1].horizontalAxis = DataManager.instance.ControllerBoss + "_LXaxis";
                inputStandaloneList[1].verticalAxis = DataManager.instance.ControllerBoss + "_LYaxis";
                inputStandaloneList[1].submitButton = DataManager.instance.ControllerBoss + "_Space";
                inputStandaloneList[1].cancelButton = DataManager.instance.ControllerBoss + "_E";
            }
            else if (DataManager.instance.ControllerBoss == Controller.J1 || DataManager.instance.ControllerBoss == Controller.J2)
            {
                inputStandaloneList[1].horizontalAxis = DataManager.instance.ControllerBoss + "_LXaxis";
                inputStandaloneList[1].verticalAxis = DataManager.instance.ControllerBoss + "_LYaxis";
                inputStandaloneList[1].submitButton = DataManager.instance.ControllerBoss + "_A";
                inputStandaloneList[1].cancelButton = DataManager.instance.ControllerBoss + "_B";
            }
        }
        inputStandaloneList[1].gameObject.SetActive(false);
    }
}
