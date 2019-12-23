using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageModifier : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Dropdown>().value = (int)DataManager.instance.language;
        GetComponent<Dropdown>().onValueChanged.AddListener(delegate { OnLanguageChanged(); });
    }

    public void OnLanguageChanged()
    {
        DataManager.instance.language = (DataManager.LANGUAGE)GetComponent<Dropdown>().value;

        foreach (LanguageKey lk in FindObjectsOfType<LanguageKey>())
        {
            lk.ChangeText();
        }
        foreach (AudioKey ak in FindObjectsOfType<AudioKey>())
        {
            ak.ChangeSound();
        }
    }
}
