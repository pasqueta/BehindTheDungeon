using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapColor : MonoBehaviour {

    [SerializeField]
    SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    Gradient colorR;

    [SerializeField]
    Gradient colorG;

    [SerializeField]
    Gradient colorB;

    // Use this for initialization
    void Start ()
    {
        meshRenderer.material.SetVector("_ColorR", colorR.Evaluate(Random.Range(0.0f, 1.0f)));
        meshRenderer.material.SetVector("_ColorG", colorG.Evaluate(Random.Range(0.0f, 1.0f)));
        meshRenderer.material.SetVector("_ColorB", colorB.Evaluate(Random.Range(0.0f, 1.0f)));
    }
}
