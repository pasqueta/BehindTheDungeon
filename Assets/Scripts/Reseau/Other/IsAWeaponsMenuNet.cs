using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsAWeaponsMenuNet : MonoBehaviour
{
    Button[] buttons;

    private void Start()
    {
        Invoke("Init", 2.0f);
    }

    void Init()
    {
        if (PeonNet.instance == null)
        {
            return;
        }

        buttons = new Button[PeonNet.instance.nbWeaponRecipe];

        int index = 0;
        ColorBlock colorBlock;

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            buttons[index] = button;
            index++;
        }

        for (int i = 0; i < PeonNet.instance.nbWeaponRecipe; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].image.sprite = PeonNet.instance.GiveWeaponRecipe(i).iconCarre;
                colorBlock = buttons[i].colors;
                colorBlock.normalColor = new Color(255, 255, 255, 255);
                buttons[i].colors = colorBlock;
            }
        }
    }
}
