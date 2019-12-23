using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TalentDisplayNet : MonoBehaviour {

    [SerializeField]
    CustomToggleButton toggleTalent;

    [SerializeField]
    CustomEventSystem eventSystem;

    //Canvas canvas;

    GameObject InversedUVGameObject;

    private bool boolEnteringTalent;

    //private bool isUITalentActive = false;

    /*private void Start()
    {
        canvas = GetComponent<Canvas>();
    }*/

    //c'est quoi ca ?
    //private void Update()
    //{
    //    if (GetComponent<Lifebar>())
    //    {
    //        if (Input.GetButtonDown(Boss.instance.Controller + "_A"))
    //        {
    //            isUITalentActive = true;

    //            if (boolEnteringTalent)
    //                eventSystem.SetSelectedGameObject(toggleTalent.gameObject);

    //            boolEnteringTalent = false;
    //            if (isUITalentActive)
    //            {
    //                canvas.GetComponent<Canvas>().enabled = true;
    //                InversedUVGameObject.GetComponent<MeshRenderer>().enabled = false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        isUITalentActive = true;

    //        if (boolEnteringTalent)
    //        {
    //            eventSystem.SetSelectedGameObject(toggleTalent.gameObject);
    //            toggleTalent.enabled = true;
    //        }
    //        boolEnteringTalent = false;
    //        if (isUITalentActive)
    //        {
    //            canvas.GetComponent<Canvas>().enabled = true;
    //        }
    //    }
    //}

    public void DisplayTalent()
    {
        GetComponent<Canvas>().enabled = true;
        if (WinLooseManagerNet.instance.gameIsAlreadyEnd == true)
        {
            Transform[] tfs = GetComponentsInChildren<Transform>(true);

            foreach (Transform tf in tfs)
            {
                if (tf.CompareTag("UIToInvert"))
                {
                    tf.gameObject.SetActive(!tf.gameObject.activeSelf);
                }
            }

        }

        eventSystem.SetSelectedGameObject(toggleTalent.gameObject);
    }

    public void SaveTalents()
    {
        foreach (ButtonTalent item in GetComponentsInChildren<ButtonTalent>()) 
        {
            item.OnSave();
        }
    }

}
