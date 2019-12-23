using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IsATrapMenuNet : NetworkBehaviour
{
    Button[] buttons;

    bool initSuccess = false;

    private void Start()
    {
        Invoke("Init", 2.0f);
    }

    void Init()
    {
        int index = 0;
        ColorBlock colorBlock;

        if (PeonNet.instance)
        {
            buttons = new Button[PeonNet.instance.nbTrapRecipe];

            foreach (Button button in GetComponentsInChildren<Button>())
            {
                buttons[index] = button;
                index++;
            }

            for (int i = 0; i < PeonNet.instance.nbTrapRecipe; i++)
            {
                buttons[i].image.sprite = PeonNet.instance.GiveTrapRecipe(i).icon;
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
