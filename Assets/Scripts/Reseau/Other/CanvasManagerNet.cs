using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CanvasManagerNet : NetworkBehaviour
{
    public static CanvasManagerNet instance;

    [SerializeField] Canvas canvasBoss;
    [SerializeField] GameObject napUI;
    [SerializeField] GameObject actionTextBoss;

    [SerializeField] Canvas canvasPeon;
    [SerializeField] GameObject actionTextPeon;

    [SerializeField] Canvas canvasCommon;
    [SerializeField] Text textTimeBetweenWaves;
    [SerializeField] Text textRemainingWave;
    [SerializeField] Text textEndOfWave;


    void Start ()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        Invoke("InitPlayer", 2.0f);
    }

    void InitPlayer()
    {
        if (PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2)
        {
            actionTextPeon.GetComponentInChildren<LanguageKey>().SetKey(86);
        }
        if (BossNet.instance.Controller == Controller.J1 || BossNet.instance.Controller == Controller.J2)
        {
            actionTextBoss.GetComponentInChildren<LanguageKey>().SetKey(86);
        }
    }

    public Canvas GetCanvasBoss()
    {
        return canvasBoss;
    }

    public Canvas GetCanvasPeon()
    {
        return canvasPeon;
    }

    public Canvas GetCanvasCommon()
    {
        return canvasCommon;
    }

    public Text GetTextTimeBetweenWaves()
    {
        return textTimeBetweenWaves;
    }

    public Text GetTextRemainingWave()
    {
        return textRemainingWave;
    }

    public Text GetTextEndOfWave()
    {
        return textEndOfWave;
    }

    public GameObject GetActionTextPeon()
    {
        return actionTextPeon;
    }

    public GameObject GetActionTextBoss()
    {
        return actionTextBoss;
    }

    public GameObject GetNapUI()
    {
        return napUI;
    }
}
