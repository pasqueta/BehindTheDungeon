using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSelector : MonoBehaviour
{
    [SerializeField]
    GameObject hammer;

    public bool isSitBored = false;
    public bool isStealer = false;
    public bool isCheer01 = false;
    public bool isAnvil01 = false;

    Animator anim;

    [SerializeField]
    Gradient colors;

    [SerializeField]
    SkinnedMeshRenderer meshRenderer;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        
        anim.SetBool("SitBored", isSitBored);
        anim.SetBool("Stealer", isStealer);
        anim.SetBool("Cheer01", isCheer01);
        anim.SetBool("Anvil01", isAnvil01);

        if(isAnvil01)
        {
            hammer.gameObject.SetActive(true);
        }
        Color c = colors.Evaluate(Random.Range(0.0f, 1.0f));
        meshRenderer.material.SetVector("_Color", c);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //meshRenderer.material.color = color;
    }
}
