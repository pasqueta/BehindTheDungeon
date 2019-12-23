using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class CustomToggleButton : Toggle
{

    public EventSystem eventSystem;
     
    
    protected override void Awake()
    {
        base.Awake();
        eventSystem = GetComponent<EventSystemProvider>().eventSystem;

        Image[] initImages = new Image[2];

        int i = 0;
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            initImages[i] = image;
            i++;
        }

        targetGraphic = initImages[0];
        graphic = initImages[1];

        if (Boss.instance)
        {
            if (GetComponentInParent<Lifebar>() != null)
            {
                initImages[1].gameObject.AddComponent<ToggleBoss>();
            }
            else
            {
                initImages[1].gameObject.AddComponent<TogglePeon>();
            }
        }
        else if(BossNet.instance)
        {
            if (GetComponentInParent<Lifebar>() != null)
            {
                initImages[1].gameObject.AddComponent<ToggleBoss>();
            }
            else
            {
                initImages[1].gameObject.AddComponent<TogglePeon>();
            }
        }
    }
    
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        if (IsInteractable() && navigation.mode != Navigation.Mode.None)
            eventSystem.SetSelectedGameObject(gameObject, eventData);

        base.OnPointerDown(eventData);
    }

    public override void Select()
    {
        if (eventSystem.alreadySelecting)
            return;

        eventSystem.SetSelectedGameObject(gameObject);
    }
}
