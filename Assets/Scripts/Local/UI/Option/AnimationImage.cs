using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationImage : MonoBehaviour
{
    public bool animated = true;   

    // Update is called once per frame
    void Update()
    {
        if (animated)
        {
            GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void RevertAnimation()
    {
        GetComponent<Animator>().SetFloat("Speed", -GetComponent<Animator>().GetFloat("Speed"));
    }
}
