using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsingText : MonoBehaviour
{

    [SerializeField]
    float minSize = 0.5f, maxSize = 1.0f;

    [SerializeField]
    float lerpTime = 1.0f;

    float currentDt;
    bool rise;

    void Start()
    {
        currentDt = 0.0f;
        rise = true;
    }

    void Update()
    {
        currentDt += Time.deltaTime;

        if (currentDt >= lerpTime)
        {
            currentDt = 0.0f;
            rise = !rise;
        }

        if (rise)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, currentDt / lerpTime);
        }
        else
        {
            transform.localScale = Vector3.one * Mathf.Lerp(maxSize, minSize, currentDt / lerpTime);
        }
    }
}
