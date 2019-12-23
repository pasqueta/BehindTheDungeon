using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMove : MonoBehaviour
{
    [SerializeField]
    Transform toMove;

    
    bool updown = true; // true = up enfin en haut
    bool isMoving = false;
    float time = 20;
    Vector3 toGo;
    Vector3 depart;
    private void Update()
    {
        if (time < 2)
        {
            time += Time.deltaTime;
           toMove.localPosition = Vector3.Lerp(depart, toGo, time / 2.0f);
        }
        else if(isMoving)
        {
            isMoving = false;
        }
    }
    public void OnMoveTheCredits()
    {
        if (!isMoving)
        {
            time = 0;
            updown = !updown;
            isMoving = true;

            if (updown)
            {
                GetComponentInChildren<Text>().text = "↓";
                toGo.y = 0;
                depart.y = 500;
            }
            else
            {
                GetComponentInChildren<Text>().text = "↑";
                toGo.y = 500;
                depart.y = 0;
            }
        }
        
    }
}
