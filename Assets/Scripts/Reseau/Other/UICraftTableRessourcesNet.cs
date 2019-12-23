using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICraftTableRessourcesNet : MonoBehaviour
{
    [SerializeField]
    Text woodText;

    [SerializeField]
    Text ironText;

    [SerializeField]
    Text diamondText;

    [SerializeField]
    Text mithrilText;

    [SerializeField]
    Text bonesText;

    [SerializeField]
    Text dustText;

    TransformerUINet transformerUI;
    // Use this for initialization
    void Start () {
        transformerUI = GetComponentInParent<TransformerUINet>();

        woodText.text = "x 0";
        ironText.text = "x 0";
        diamondText.text = "x 0";
        mithrilText.text = "x 0";
        bonesText.text = "x 0";
        dustText.text = "x 0";
    }

    // Update is called once per frame
    void Update ()
    {
        if (woodText != null && ironText != null && diamondText != null && mithrilText != null && bonesText != null && dustText != null)
        {
            woodText.text = "x " + transformerUI.transformer.acquiredMinerals.wood;
            ironText.text = "x " + transformerUI.transformer.acquiredMinerals.iron;
            diamondText.text = "x " + transformerUI.transformer.acquiredMinerals.diamond;
            mithrilText.text = "x " + transformerUI.transformer.acquiredMinerals.mithril;
            bonesText.text = "x " + transformerUI.transformer.acquiredMinerals.bones;
            dustText.text = "x " + transformerUI.transformer.acquiredMinerals.dust;
        }
    }
}
