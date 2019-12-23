using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickController : MonoBehaviour
{

    EventSystem es;
    // Use this for initialization
    private void OnEnable()
    {
        if (gameObject.activeSelf)
        {
            if (GetComponent<Button>().IsInteractable())
            {
                Invoke("walla", 0.1f);
            }
        }
    }

    void walla()
    {
        es = FindObjectOfType<EventSystem>();
        es.SetSelectedGameObject(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
