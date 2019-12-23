using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityStruct {
    
    public struct STATS
    {
        public int health;
        public int attack;
        public float attackSpeed;
        public float movementSpeed;
        public int whirlwind;
        public float dash;
    }


    public struct STATSPEON
    {
        public float movementSpeed;
        public int backPack;
        public int pickAxe;
        public float craftingSpeed;
        public int gatheringSpeed;
    }


    public struct MINERALS
    {
        public int wood;
        public int iron;
        public int diamond;
        public int mithril;
        public int bones;
        public int dust;
    }

    public struct WEAPONS
    {

        //wood
        public int woodSword;
        public int woodScythe;
        public int woodHammer;
        //iron
        public int ironSword;
        public int ironScythe;
        public int ironHammer;
        //Diamond
        public int diamondSword;
        public int diamondScythe;
        public int diamondHammer;
        //Mithril
        public int mithrilSword;
        public int mithrilScythe;
        public int mithrilHammer;
    }

    public struct TRAPS
    {
        public int chicken;
        public int ice;
        public int mine;
        public int spike;
        public int arrowslit;
    }
}
