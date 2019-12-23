using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TalentDisplay : MonoBehaviour {

    [SerializeField]
    CustomToggleButton toggleTalent;

    [SerializeField]
    AudioSource whatDoYouNeedSound;

    [SerializeField]
    CustomEventSystem eventSystem;

    GameObject InversedUVGameObject;

    private bool boolEnteringTalent;




    public void DisplayTalentDelayed()
    {
        if (GetComponent<Canvas>().enabled != true)
        {
            GetComponent<Canvas>().enabled = true;
            if (WinLooseManager.instance)
            {
                if (WinLooseManager.instance.gameIsAlreadyEnd == true)
                {
                    Transform[] tfs = GetComponentsInChildren<Transform>(true);

                    WhatDoYouNeedSound();

                    foreach (Transform tf in tfs)
                    {
                        if (tf.CompareTag("UIToInvert"))
                        {
                            tf.gameObject.SetActive(!tf.gameObject.activeSelf);
                        }
                    }
                }
            }
            else if (WinLooseManagerNet.instance)
            {
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
            }

            eventSystem.SetSelectedGameObject(toggleTalent.gameObject);
        }
    }

    public void DisplayTalent()
    {
        Invoke("DisplayTalentDelayed", 0.1f);
    }

    public void WhatDoYouNeedSound()
    {
        if(whatDoYouNeedSound && AudioTypeBehaviour.voiceIsPlaying == false)
        {
            whatDoYouNeedSound.Play();
            AudioTypeBehaviour.voiceIsPlaying = true;
            whatDoYouNeedSound.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", whatDoYouNeedSound.clip.length);
        }
    }

    public void SaveTalents()
    {
        foreach (ButtonTalent item in GetComponentsInChildren<ButtonTalent>()) 
        {
            item.OnSave();
        }
    }

}
