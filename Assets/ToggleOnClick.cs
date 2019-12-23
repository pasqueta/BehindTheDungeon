using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleOnClick : MonoBehaviour, ISubmitHandler, IPointerClickHandler
{
    ButtonTalent buttonTalent;
    private void Start()
    {
        buttonTalent = GetComponentInParent<ButtonTalent>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<CustomToggleButton>().interactable)
        {
            buttonTalent.talentSelected();
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (buttonTalent)
            if (buttonTalent.eventSystem.eventSystem.currentSelectedGameObject.GetComponent<CustomToggleButton>().interactable)
            {
                buttonTalent.talentSelected();
            }
    }
}
