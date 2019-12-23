using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TrapRecipe", menuName = "Inventory/TrapRecipe", order = 1)]
[Serializable]
public class TrapRecipe : Recipe {

   
    public enum Trap
    {
        CHICKEN,
        ICE,
        MINE,
        SPIKE,
        ARROWSLIT,
    }

    [SerializeField]
    Trap trap;

    public int trapDamages;
    public int trapMaxUses;

    public Sprite[] iconResource;

    public int GetTrapDamages()
    {
        return trapDamages;
    }

    public int GetTrapMaxUses()
    {
        return trapMaxUses;
    }


    public Trap GetTrap()
    {
        return trap;
    }
}
