using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LOOT_TYPE
{
    BONES,
    GOLD,
}

public class Lootable : MonoBehaviour {

    [SerializeField]
    LOOT_TYPE lootType;

    public LOOT_TYPE LootType
    {
        get
        {
            return lootType;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Boss>())
        {
            Boss.instance.AddItemInLootList(gameObject);
        }
        else if (collider.GetComponent<BossNet>())
        {
            BossNet.instance.AddItemInLootList(gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<Boss>())
        {
            Boss.instance.RemoveItemInLootList(gameObject);
        }
        else if (collider.GetComponent<BossNet>())
        {
            BossNet.instance.RemoveItemInLootList(gameObject);
        }
    }
}
