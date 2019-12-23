using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {

    bool peonIsNear = false;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Peon"))
            peonIsNear = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Peon"))
        {
            peonIsNear = false;

            if (CanvasManager.instance)
            {
                CanvasManager.instance.GetActionTextPeon().SetActive(false);
            }
            else if(CanvasManagerNet.instance)
            {
                CanvasManagerNet.instance.GetActionTextPeon().SetActive(false);
            }
        }
    }

    public bool GetPeonIsNear()
    {
        return peonIsNear;
    }
}
