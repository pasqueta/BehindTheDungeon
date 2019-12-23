using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCanvas : MonoBehaviour {

    [SerializeField]
    bool isBossCanvas = true;

    [SerializeField]
    GameObject canvasEn;

    [SerializeField]
    GameObject canvasFr;

	void Update ()
    {
        // BOSS LOCAL
        if (Boss.instance && isBossCanvas)
        {

            if ((Boss.instance.Controller == Controller.K1 && Input.GetKey(KeyCode.LeftControl)) || 
                ((Boss.instance.Controller == Controller.J1 || Boss.instance.Controller == Controller.J2) && (Input.GetAxisRaw(Boss.instance.Controller + "_RT") != 0 || Input.GetAxisRaw(Boss.instance.Controller + "_LT") != 0)))
            {
                if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
                    canvasFr.SetActive(true);
                else
                    canvasEn.SetActive(true);
            }
            else
            {
                canvasFr.SetActive(false);
                canvasEn.SetActive(false);
            }
        }

        // BOSS NET
        if (BossNet.instance && isBossCanvas)
        {
            if ((BossNet.instance.Controller == Controller.K1 && Input.GetKey(KeyCode.LeftControl)) ||
            ((BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2) && (Input.GetAxis(BossNet.instance.Controller + "_RT") != 0 || Input.GetAxis(BossNet.instance.Controller + "_LT") != 0)))
            {
                if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
                    canvasFr.SetActive(true);
                else
                    canvasEn.SetActive(true);
            }
            else
            {
                canvasFr.SetActive(false);
                canvasEn.SetActive(false);
            }
        }

        // PEON LOCAL
        if (Peon.instance && !isBossCanvas)
        {
            if ((Peon.instance.Controller == Controller.K1 && Input.GetKey(KeyCode.LeftControl)) ||
            (Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2) && (Input.GetAxis(Peon.instance.Controller + "_RT") > 0 || Input.GetAxis(Peon.instance.Controller + "_LT") > 0))
            {
                if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
                    canvasFr.SetActive(true);
                else
                    canvasEn.SetActive(true);
            }
            else
            {
                canvasFr.SetActive(false);
                canvasEn.SetActive(false);
            }
        }

        // PEON NET
        if (PeonNet.instance && !isBossCanvas)
        {
            if ((PeonNet.instance.Controller == Controller.K1 && Input.GetKey(KeyCode.LeftControl)) ||
            (PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2) && (Input.GetAxis(PeonNet.instance.Controller + "_RT") > 0 || Input.GetAxis(PeonNet.instance.Controller + "_LT") > 0))
            {
                if (DataManager.instance.language == DataManager.LANGUAGE.FRANCAIS)
                    canvasFr.SetActive(true);
                else
                    canvasEn.SetActive(true);
            }
            else
            {
                canvasFr.SetActive(false);
                canvasEn.SetActive(false);
            }
        }
    }
}
