using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ressources : MonoBehaviour {

    enum RESSOURCES
    {
        WOOD,
        IRON,
        DIAMOND,
        MITHRIL
    }

    [SerializeField]
    Text text;

    [SerializeField]
    RESSOURCES ressource;

    [SerializeField]
    AudioSource cantCarryMoreSound;

    float currentDt = 0.0f;
    public bool full = false;

    void Start()
    {
    }

    public void CantCarryMoreSound()
    {
        if(cantCarryMoreSound)
        {
            cantCarryMoreSound.Play();
        }

    }

    void Update()
    {
        if(Peon.instance != null)
        {
            switch (ressource)
            {
                case RESSOURCES.WOOD:
                    text.text = Peon.instance.minerals.wood.ToString();
                    break;
                case RESSOURCES.IRON:
                    text.text = Peon.instance.minerals.iron.ToString();
                    break;
                case RESSOURCES.DIAMOND:
                    text.text = Peon.instance.minerals.diamond.ToString();
                    break;
                case RESSOURCES.MITHRIL:
                    text.text = Peon.instance.minerals.mithril.ToString();
                    break;
                default:
                    break;
            }

            //sac plein
            if (Peon.instance.minerals.wood + Peon.instance.minerals.iron + Peon.instance.minerals.diamond + Peon.instance.minerals.mithril >= Peon.instance.stats.backPack)
            {
                GetComponent<Image>().color = Color.red;
            }
            else
            {
                GetComponent<Image>().color = Color.white;
            }

            //récolte alors que le sac est plein 
            if (full)
            {
                currentDt += Time.deltaTime;

                if (currentDt <= 0.5f)
                {
                    CantCarryMoreSound();
                    transform.localScale += transform.localScale * (0.3f * Time.deltaTime);
                }
                else if (currentDt <= 1.0f)
                {
                    transform.localScale -= transform.localScale * 0.3f * Time.deltaTime;
                }
                else
                {
                    full = false;
                    currentDt = 0;
                }
            }
        }
        else if (PeonNet.instance != null)
        {
            switch (ressource)
            {
                case RESSOURCES.WOOD:
                    text.text = PeonNet.instance.minerals.wood.ToString();
                    break;
                case RESSOURCES.IRON:
                    text.text = PeonNet.instance.minerals.iron.ToString();
                    break;
                case RESSOURCES.DIAMOND:
                    text.text = PeonNet.instance.minerals.diamond.ToString();
                    break;
                case RESSOURCES.MITHRIL:
                    text.text = PeonNet.instance.minerals.mithril.ToString();
                    break;
                default:
                    break;
            }

            //sac plein
            if (PeonNet.instance.minerals.wood + PeonNet.instance.minerals.iron + PeonNet.instance.minerals.diamond + PeonNet.instance.minerals.mithril >= PeonNet.instance.stats.backPack)
            {
                GetComponent<Image>().color = Color.red;
            }
            else
            {
                GetComponent<Image>().color = Color.white;
            }

            //récolte alors que le sac est plein 
            if (full)
            {
                currentDt += Time.deltaTime;

                if (currentDt <= 0.5f)
                {
                    transform.localScale += transform.localScale * (0.3f * Time.deltaTime);
                }
                else if (currentDt <= 1.0f)
                {
                    transform.localScale -= transform.localScale * 0.3f * Time.deltaTime;
                }
                else
                {
                    full = false;
                    currentDt = 0;
                }
            }
        }
    }
}
