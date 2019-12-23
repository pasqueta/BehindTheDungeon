using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressiveDisparition : MonoBehaviour {
    public bool disparition = true;
    public float disparitionDuration = 1.0f;
    float currentTime = 0.0f;
    public bool StartDisparition { get; set; }


    [SerializeField]
    MeshRenderer meshRenderer;
    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

    // Use this for initialization
    void OnEnable ()
    {
        if (!disparition)
        {
            if (meshRenderer)
                meshRenderer.material.SetFloat("_Progression", 1);
            if(skinnedMeshRenderer)
                skinnedMeshRenderer.material.SetFloat("_Progression", 1);
        }
    }

    private void OnDisable()
    {
        currentTime = 0;
        StartDisparition = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (StartDisparition)
        {
            currentTime += Time.deltaTime;
            if (disparition)
            {
                if (meshRenderer)
                    meshRenderer.material.SetFloat("_Progression", currentTime / disparitionDuration);
                if (skinnedMeshRenderer)
                    skinnedMeshRenderer.material.SetFloat("_Progression", currentTime / disparitionDuration);
            }
            else
            {
                if (meshRenderer)
                    meshRenderer.material.SetFloat("_Progression",1 - (currentTime / disparitionDuration));
                if (skinnedMeshRenderer)
                    skinnedMeshRenderer.material.SetFloat("_Progression", 1 - (currentTime / disparitionDuration));
            }
        }
	}


}
