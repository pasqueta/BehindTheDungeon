using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsATrapMenu : MonoBehaviour {

    Button[] buttons;

    bool initSuccess = false;
    private void Start()
    {      
        Init();
    }

    private void Init()
    {
        int index = 0;
        ColorBlock colorBlock;

        if (Peon.instance)
        {
            buttons = new Button[Peon.instance.nbTrapRecipe];

            foreach (Button button in GetComponentsInChildren<Button>())
            {
                buttons[index] = button;
                index++;
            }
            
            for (int i = 0; i < Peon.instance.nbTrapRecipe; i++)
            {
                buttons[i].image.sprite = Peon.instance.GiveTrapRecipe(i).icon;
                colorBlock = buttons[i].colors;
                colorBlock.normalColor = new Color(255, 255, 255, 255);
                buttons[i].colors = colorBlock;
            }

            initSuccess = true;
        }
    }

    private void Update()
    {
        if (!initSuccess)
        {
            Init();
        }
    }
}
