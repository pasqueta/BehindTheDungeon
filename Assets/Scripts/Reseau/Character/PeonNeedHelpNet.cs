using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeonNeedHelpNet : MonoBehaviour
{
    [SerializeField]
    BridgeNet[] bridge = new BridgeNet[2];

    [SerializeField]
    AudioSource helpBridgeSound;

    bool justOneTime = false;


    public void HelpBridgeSound()
    {
        if (helpBridgeSound)
        {
            helpBridgeSound.Play();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (bridge[0].GetPeonPos() == 0 && !bridge[0].GetEnableJump())
        {
            //if (!helpBridgeSound.isPlaying && justOneTime == false)
            //{
            //    HelpBridgeSound();
            //}
            justOneTime = true;

            GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else if (bridge[1].GetPeonPos() == 0 && !bridge[1].GetEnableJump())
        {
            //if (!helpBridgeSound.isPlaying && justOneTime == false)
            //{
            //    HelpBridgeSound();
            //}
            justOneTime = true;

            GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }
}
