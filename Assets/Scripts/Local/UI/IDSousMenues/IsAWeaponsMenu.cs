using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsAWeaponsMenu : MonoBehaviour
{

    Button[] buttons;

    private void Start()
    {
        buttons = new Button[Peon.instance.nbWeaponRecipe];

        int index = 0;
        ColorBlock colorBlock;

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            buttons[index] = button;
            index++;
        }

        for (int i = 0; i < Peon.instance.nbWeaponRecipe; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].image.sprite = Peon.instance.GiveWeaponRecipe(i).iconCarre;
                colorBlock = buttons[i].colors;
                colorBlock.normalColor = new Color(255, 255, 255, 255);
                buttons[i].colors = colorBlock;
            }
        }
    }
}
