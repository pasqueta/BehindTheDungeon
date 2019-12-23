using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[CreateAssetMenu(fileName = "TrapRecipe", menuName = "Inventory/TrapRecipe", order = 1)]
[Serializable]
public class Recipe : ScriptableObject {

    public string displayName = "recipe_noname";
    public Sprite icon;

    public AudioClip soundCraft;
    public AudioClip soundHit;


    [SerializeField]
    int wood;

    [SerializeField]
    int iron;

    [SerializeField]
    int diamond;

    [SerializeField]
    int mithril;

    [SerializeField]
    int bones;

    [SerializeField]
    int dust;

    public bool DoRecipe(ref UtilityStruct.MINERALS minerals)
    {
        if (minerals.wood >= wood && minerals.iron >= iron && minerals.diamond >= diamond && minerals.mithril >= mithril && minerals.bones >= bones && minerals.dust >= dust)
        {
            minerals.wood -= wood;
            minerals.iron -= iron;
            minerals.diamond -= diamond;
            minerals.mithril -= mithril;
            minerals.bones -= bones;
            minerals.dust -= dust;

            return true;
        }
        return false;
    }

    public int GetWoodRessources()
    {
        return wood;
    }

    public int GetIronRessources()
    {
        return iron;
    }

    public int GetMithrilRessources()
    {
        return mithril;
    }

    public int GetDiamondRessources()
    {
        return diamond;
    }

    public int GetBonesRessources()
    {
        return bones;
    }

    public int GetDustRessources()
    {
        return dust;
    }

    public bool GetMultipleElement()
    {
        if ((wood > 1 && iron > 1)  || (wood > 1 && diamond > 1) || (wood > 1 && mithril > 1) || (wood > 1 && bones >1)
            || (iron > 1 && diamond > 1) || (iron > 1 && mithril > 1) || (iron >1 && bones > 1) || (diamond > 1 && mithril >1)
            || (diamond > 1 && bones > 1) || (mithril > 1 && bones > 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
