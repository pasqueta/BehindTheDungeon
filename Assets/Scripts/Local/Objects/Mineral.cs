using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Mineral : MonoBehaviour
{
    public enum MaterialType
    {
        WOOD,
        IRON,
        DIAMOND,
        MITHRIL,
    }

    [SerializeField]
    int nbMinerals = 50;
    private int nbMineralsMax;

    bool init = false;

    [SerializeField]
    public MaterialType material;

    [SerializeField]
    Image lifeBar;

    [SerializeField]
    Text lifeCount;

    [SerializeField]
    public GameObject[] models = new GameObject[4];

    [SerializeField]
    Sprite[] iconMineral = new Sprite[4];
    
    AudioSource mining;
    
    AudioSource miningDiamond;
    
    AudioSource miningMithril;

    [SerializeField]
    GameObject textFlottant;

    [SerializeField]
    GameObject lootFX;

    GameObject InversedUV;

    Animator anim;

    private void Start()
    {
        GetAudioSource();
    }

    private void Update()
    {
        if (!init)
        {
            InitMineral();
        }
    }

    private void InitMineral()
    {
        nbMineralsMax = nbMinerals;

        InversedUV = GetComponentInChildren<MeshInverseUV>().gameObject;

        lootFX.GetComponent<ParticleSystem>().Stop();

        Instantiate(models[(int)material], transform.position, Quaternion.identity, transform);

        UpdateUI();

        init = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Peon")
        {
            InversedUV.GetComponent<MeshRenderer>().enabled = true;
            CanvasManager.instance.GetActionTextPeon().SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Peon")
        {
            InversedUV.GetComponent<MeshRenderer>().enabled = false;
            CanvasManager.instance.GetActionTextPeon().SetActive(false);
        }
    }

    void FloatingText(int gathering)
    {
        GameObject temp = Instantiate(textFlottant, GetComponentInChildren<Canvas>().gameObject.transform);
        //temp.transform.position = lifeBar.transform.position;
        //temp.GetComponentInChildren<Image>().transform.position = new Vector3(0, 0, 0);
        //temp.GetComponentInChildren<Text>().transform.position = new Vector3(0, 0, 0);
        temp.GetComponentInChildren<Image>().sprite = iconMineral[(int)material];
        temp.GetComponentInChildren<Text>().text = "-" + gathering;
    }

    void SetAniamtionLooting()
    {
        Peon.instance.GetComponent<Animator>().SetTrigger("Loot");
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject == Peon.instance.gameObject && !WinLooseManager.instance.gameIsAlreadyEnd)
        {            
            if (!Peon.instance.isMining)
            {
                if (Peon.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.E) || (Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2) && Input.GetButtonDown(Peon.instance.Controller + "_A"))
                {

                    //Peon.instance.GetComponent<Rigidbody>().isKinematic = true;

                    int peonMinerals = other.transform.GetComponent<Peon>().totalMinerals;

                    if (peonMinerals < Peon.instance.stats.backPack)
                    {

                        //p//
                        //for (int i = 0; i < Peon.instance.stats.gatheringSpeed; i++)
                        //{
                        //    Invoke("SetAniamtionLooting", 3.5f*i);
                        //}
                        
                        Peon.instance.GetComponent<Animator>().SetTrigger("Loot");
                        lootFX.GetComponent<ParticleSystem>().Play();
                        Peon.instance.isMining = true;

                        Peon.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

                        //Debug.Log(Peon.instance.GetComponent<Rigidbody>().velocity);

                        Peon.instance.transform.LookAt(transform);

                        if (nbMinerals - Peon.instance.stats.pickAxe > 0)
                        {
                            nbMinerals -= peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals;

                            switch (material)
                            {
                                case MaterialType.WOOD:
                                    other.transform.GetComponent<Peon>().minerals.wood += peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals;
                                    if (!AudioTypeBehaviour.voiceIsPlaying)
                                    {
                                        mining.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        mining.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", mining.clip.length);
                                    }
                                    break;
                                case MaterialType.IRON:
                                    other.transform.GetComponent<Peon>().minerals.iron += peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals;
                                    if (!AudioTypeBehaviour.voiceIsPlaying)
                                    {
                                        mining.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        mining.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", mining.clip.length);
                                    }
                                    break;
                                case MaterialType.DIAMOND:
                                    other.transform.GetComponent<Peon>().minerals.diamond += peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals;
                                    if (!AudioTypeBehaviour.voiceIsPlaying)
                                    {
                                        miningDiamond.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        miningDiamond.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", miningDiamond.clip.length);
                                    }
                                    break;
                                case MaterialType.MITHRIL:
                                    other.transform.GetComponent<Peon>().minerals.mithril += peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals;
                                    if (!AudioTypeBehaviour.voiceIsPlaying)
                                    {
                                        miningMithril.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        miningMithril.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", miningMithril.clip.length);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            FloatingText(peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals);
                            peonMinerals += peonMinerals + Peon.instance.stats.pickAxe < Peon.instance.stats.backPack ? (Peon.instance.stats.pickAxe) : Peon.instance.stats.backPack - peonMinerals;


                        }
                        else
                        {
                            switch (material)
                            {
                                case MaterialType.WOOD:
                                    other.transform.GetComponent<Peon>().minerals.wood += peonMinerals + nbMinerals < Peon.instance.stats.backPack ? nbMinerals : Peon.instance.stats.backPack - peonMinerals;
                                    break;
                                case MaterialType.IRON:
                                    other.transform.GetComponent<Peon>().minerals.iron += peonMinerals + nbMinerals < Peon.instance.stats.backPack ? nbMinerals : Peon.instance.stats.backPack - peonMinerals;
                                    break;
                                case MaterialType.DIAMOND:
                                    other.transform.GetComponent<Peon>().minerals.diamond += peonMinerals + nbMinerals < Peon.instance.stats.backPack ? nbMinerals : Peon.instance.stats.backPack - peonMinerals;
                                    break;
                                case MaterialType.MITHRIL:
                                    other.transform.GetComponent<Peon>().minerals.mithril += peonMinerals + nbMinerals < Peon.instance.stats.backPack ? nbMinerals : Peon.instance.stats.backPack - peonMinerals;
                                    break;
                                default:
                                    break;
                            }

                            FloatingText(peonMinerals + nbMinerals < Peon.instance.stats.backPack ? nbMinerals : Peon.instance.stats.backPack - peonMinerals);
                            peonMinerals += peonMinerals + nbMinerals < Peon.instance.stats.backPack ? nbMinerals : Peon.instance.stats.backPack - peonMinerals;

                            nbMinerals = 0;

                        }
                        other.transform.GetComponent<Peon>().totalMinerals = peonMinerals;
                        if (nbMinerals == 0)
                        {
                            CanvasManager.instance.GetActionTextPeon().SetActive(false);
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        
                        for (int i = 0; i < 4; i++)
                        {
                            CanvasManager.instance.GetCanvasPeon().transform.GetChild(i).GetComponent<Ressources>().full = true;
                        }
                    }
                }
            }

            if (Peon.instance.totalMinerals >= Peon.instance.stats.backPack)
            {
                CanvasManager.instance.GetActionTextPeon().SetActive(false);
            }

            UpdateUI();
        }
    }

    //void TrapOnSound()
    //{
    //    if (trapOnSound && AudioTypeBehaviour.voiceIsPlaying == false)
    //    {
    //        trapOnSound.Play();
    //        AudioTypeBehaviour.voiceIsPlaying = true;
    //        trapOnSound.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", trapOnSound.clip.length);
    //    }
    //}

    private void UpdateUI()
    {
        if (lifeBar)
        {
            lifeBar.fillAmount = (float)nbMinerals / nbMineralsMax;
        }
        if (lifeCount)
        {
            lifeCount.text = "";
            //lifeCount.text = nbMinerals + "/" + nbMineralsMax;
        }
    }

    private void GetAudioSource()
    {
        AudioSource [] miningSounds = GetComponentsInChildren<AudioSource>();
        if (miningSounds.Length >0)
        {
            mining = miningSounds[0];
            if (miningSounds.Length > 1)
            {
                miningDiamond = miningSounds[1];
                if (miningSounds.Length > 2)
                {
                    miningMithril = miningSounds[2];
                }
            }
        }

    }
}
