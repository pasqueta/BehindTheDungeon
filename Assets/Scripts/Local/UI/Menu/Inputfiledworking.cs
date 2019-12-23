using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inputfiledworking : MonoBehaviour
{

    InputField input;
    TouchScreenKeyboard tsk = null;
    [SerializeField]
    EventSystem es;
    [SerializeField]
    Button Button;

    private void OnEnable()
    {
        es = FindObjectOfType<EventSystem>();
    }

    // Use this for initialization
    void Start()
    {
        input = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (es != null && es.currentSelectedGameObject == gameObject)
        {
            if (Input.GetButtonDown("Menu_Submit"))
            {
                if (tsk != null)
                {
                    tsk.active = true;
                }
                else
                {
                    tsk = TouchScreenKeyboard.Open(input.text);
                }               
               
            }
            
            if (Input.GetButtonDown("Menu_Cancel"))
            {
                if (tsk != null)
                {
                    tsk.active = false;
                }
                es.SetSelectedGameObject(Button.gameObject);
            }
        }

    }

}
