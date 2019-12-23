using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FrozenMine : Trap {

    [SerializeField]
    GameObject freezeZone;

    [SerializeField]
    AudioSource soundFreeze;

    int currentUse = 0;

    new private void Start()
    {
        base.Start();
        damages = Peon.instance.GiveTrapRecipe(3).GetTrapDamages();
        maxUse = Peon.instance.GiveTrapRecipe(3).GetTrapMaxUses();
    }
    // Update is called once per frame
    void Update ()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isActive && (collider.transform.CompareTag("Enemy")) && currentUse != maxUse)
        {
            freezeZone.SetActive(true);
            soundFreeze.Play();
        }
    }
}
