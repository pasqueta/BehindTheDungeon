using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class FrozenMineNet : TrapNet
{
    /*[SerializeField]
    int maxUse = 1;*/

    [SerializeField]
    GameObject freezeZone;

    [SerializeField]
    AudioSource soundFreeze;

    [SyncVar]
    int currentUse = 0;

    new private void Start()
    {
        base.Start();
        damages = PeonNet.instance.GiveTrapRecipe(3).GetTrapDamages();
        maxUse = PeonNet.instance.GiveTrapRecipe(3).GetTrapMaxUses();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isServer)
            return;

        if (isActive && (collider.transform.CompareTag("Enemy")) && currentUse != maxUse)
        {
            //freezeZone.SetActive(true);
            RpcSetFreezeZoneActive(true);
            //soundFreeze.Play();
        }
    }

    [ClientRpc]
    void RpcSetFreezeZoneActive(bool state)
    {
        freezeZone.SetActive(state);
    }
}
