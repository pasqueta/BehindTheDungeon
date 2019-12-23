using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WeaponRecipe", menuName = "Inventory/WeaponRecipe", order = 1)]
[Serializable]
public class WeaponRecipe : Recipe {

    

    public enum Weapon
    {
        WOODSWORD,
        WOODSCYTHE,
        WOODHAMMER,
        IRONSWORD,
        IRONSCYTHE,
        IRONHAMMER,
        DIAMONDSWORD,
        DIAMONDSCYTHE,
        DIAMONDHAMMER,
        MITHRILSWORD,
        MITHRILSCYTHE,
        MITHRILHAMMER,
        NONE
    }
    //SWORD,
    //AXE,
    //BOW,

    [SerializeField]
    public Weapon weapon;

    [SerializeField]
    public int weaponAttack;

    [SerializeField]
    public int durability;

    public int durabilityMax;

    public Sprite iconCarre;

    public Sprite[] iconResource;

    [SerializeField]
    GameObject prefabWeapon;

    public Weapon GetWeapon()
    {
        return weapon;
    }

    public int GetAttack()
    {
        return weaponAttack;
    }

    public int GetDurability()
    {
        return durability;
    }

	 public WeaponRecipe(WeaponRecipe _weaponRecipe)
    {
        weapon = _weaponRecipe.weapon;
        weaponAttack = _weaponRecipe.weaponAttack;
        durability = _weaponRecipe.durability;
        durabilityMax = _weaponRecipe.durabilityMax;
        iconCarre = _weaponRecipe.iconCarre;
        iconResource = _weaponRecipe.iconResource;
    }

    public GameObject GetPrefabWeapon()
    {
        return prefabWeapon;
    }
}
