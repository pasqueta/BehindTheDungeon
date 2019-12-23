using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaulDebugOptions : MonoBehaviour {
    
	void Start ()
    {

	}
	
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Boss.instance)
                Boss.instance.HealthPoint -= Boss.instance.BossPdvMax/2;
            else if(BossNet.instance)
                BossNet.instance.HealthPoint -= BossNet.instance.BossPdvMax / 2;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (SpawnerManager.instance)
            {
                SpawnerManager.instance.KillAllEnemies();
            }
            else if(SpawnerManagerNet.instance)
            {
                SpawnerManagerNet.instance.KillAllEnemies();
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (Peon.instance)
            {
                Peon.instance.minerals.wood += 20;
                Peon.instance.minerals.iron += 20;
                Peon.instance.minerals.diamond += 20;
                Peon.instance.minerals.mithril += 20;
            }
            else if (PeonNet.instance)
            {
                PeonNet.instance.minerals.wood += 20;
                PeonNet.instance.minerals.iron += 20;
                PeonNet.instance.minerals.diamond += 20;
                PeonNet.instance.minerals.mithril += 20;
            }
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (TrapPad.instance)
            {
                TrapPad.instance.inventoryTrap.arrowslit += 10;
                TrapPad.instance.inventoryTrap.chicken += 10;
                TrapPad.instance.inventoryTrap.ice += 10;
                TrapPad.instance.inventoryTrap.mine += 10;
                TrapPad.instance.inventoryTrap.spike += 10;
            }
            else if (TrapPadNet.instance)
            {
                TrapPadNet.instance.inventoryTrap.arrowslit += 10;
                TrapPadNet.instance.inventoryTrap.chicken += 10;
                TrapPadNet.instance.inventoryTrap.ice += 10;
                TrapPadNet.instance.inventoryTrap.mine += 10;
                TrapPadNet.instance.inventoryTrap.spike += 10;
            }
        }


        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (Boss.instance)
            {
                Boss.instance.stats.whirlwind = 3;
            }
            else if (BossNet.instance)
            {
                BossNet.instance.stats.whirlwind = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            string[] names = null;
            if (Boss.instance)
            {
                names = System.Enum.GetNames(typeof(ButtonTalent.TalentsList));
            }
            else if (BossNet.instance)
            {
                names = System.Enum.GetNames(typeof(ButtonTalentNet.TalentsList));
            }

            for (int i = 0; i < names.Length; i++)
            {
                PlayerPrefs.SetInt(names[i], 0);
            }
        }
    }
}
