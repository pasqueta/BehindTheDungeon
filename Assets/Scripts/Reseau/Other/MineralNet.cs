using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MineralNet : NetworkBehaviour
{
    public enum MaterialType
    {
        WOOD,
        IRON,
        DIAMOND,
        MITHRIL,
    }

    [SerializeField, SyncVar]
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
            CanvasManagerNet.instance.GetActionTextPeon().SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Peon")
        {
            InversedUV.GetComponent<MeshRenderer>().enabled = false;
            CanvasManagerNet.instance.GetActionTextPeon().SetActive(false);
        }
    }

    void FloatingText(int gathering)
    {
        GameObject temp = Instantiate(textFlottant, GetComponentInChildren<Canvas>().gameObject.transform);
        temp.GetComponentInChildren<Image>().sprite = iconMineral[(int)material];
        temp.GetComponentInChildren<Text>().text = "-" + gathering;
    }

    void SetAniamtionLooting()
    {
        PeonNet.instance.GetComponent<Animator>().SetTrigger("Loot");
    }

    private void OnTriggerStay(Collider other)
    {
        if (PeonNet.instance && !PeonNet.instance.isLocalPlayer)
            return;


        if (other.transform.tag == "Peon" && !WinLooseManagerNet.instance.gameIsAlreadyEnd)
        {
            if (PeonNet.instance.Controller == Controller.K1 && Input.GetKeyDown(KeyCode.E) || (PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2) && Input.GetButtonDown(PeonNet.instance.Controller + "_A"))
            {
                if (!PeonNet.instance.isMining)
                {
                    int peonMinerals = other.transform.GetComponent<PeonNet>().totalMinerals;

                    if (peonMinerals < PeonNet.instance.stats.backPack)
                    {
                        PeonNet.instance.GetComponent<Animator>().SetTrigger("Loot");
                        lootFX.GetComponent<ParticleSystem>().Play();
                        PeonNet.instance.isMining = true;

                        PeonNet.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

                        if (nbMinerals - PeonNet.instance.stats.pickAxe > 0)
                        {
                            nbMinerals -= peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals;

                            switch (material)
                            {
                                case MaterialType.WOOD:
                                    other.transform.GetComponent<PeonNet>().minerals.wood += peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals;
                                    {
                                        mining.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        mining.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", mining.clip.length);
                                    }
                                    break;
                                case MaterialType.IRON:
                                    other.transform.GetComponent<PeonNet>().minerals.iron += peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals;
                                    {
                                        mining.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        mining.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", mining.clip.length);
                                    }
                                    break;
                                case MaterialType.DIAMOND:
                                    other.transform.GetComponent<PeonNet>().minerals.diamond += peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals;
                                    if (!AudioTypeBehaviour.voiceIsPlaying)
                                    {
                                        miningDiamond.Play();
                                        AudioTypeBehaviour.voiceIsPlaying = true;
                                        miningDiamond.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", miningDiamond.clip.length);
                                    }
                                    break;
                                case MaterialType.MITHRIL:
                                    other.transform.GetComponent<PeonNet>().minerals.mithril += peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals;
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

                            FloatingText(peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals);
                            peonMinerals += peonMinerals + PeonNet.instance.stats.pickAxe < PeonNet.instance.stats.backPack ? (PeonNet.instance.stats.pickAxe) : PeonNet.instance.stats.backPack - peonMinerals;
                        }
                        else
                        {
                            switch (material)
                            {
                                case MaterialType.WOOD:
                                    other.transform.GetComponent<PeonNet>().minerals.wood += peonMinerals + nbMinerals < PeonNet.instance.stats.backPack ? nbMinerals : PeonNet.instance.stats.backPack - peonMinerals;
                                    break;
                                case MaterialType.IRON:
                                    other.transform.GetComponent<PeonNet>().minerals.iron += peonMinerals + nbMinerals < PeonNet.instance.stats.backPack ? nbMinerals : PeonNet.instance.stats.backPack - peonMinerals;
                                    break;
                                case MaterialType.DIAMOND:
                                    other.transform.GetComponent<PeonNet>().minerals.diamond += peonMinerals + nbMinerals < PeonNet.instance.stats.backPack ? nbMinerals : PeonNet.instance.stats.backPack - peonMinerals;
                                    break;
                                case MaterialType.MITHRIL:
                                    other.transform.GetComponent<PeonNet>().minerals.mithril += peonMinerals + nbMinerals < PeonNet.instance.stats.backPack ? nbMinerals : PeonNet.instance.stats.backPack - peonMinerals;
                                    break;
                                default:
                                    break;
                            }

                            FloatingText(peonMinerals + nbMinerals < PeonNet.instance.stats.backPack ? nbMinerals : PeonNet.instance.stats.backPack - peonMinerals);
                            peonMinerals += peonMinerals + nbMinerals < PeonNet.instance.stats.backPack ? nbMinerals : PeonNet.instance.stats.backPack - peonMinerals;

                            nbMinerals = 0;
                        }

                        other.transform.GetComponent<PeonNet>().totalMinerals = peonMinerals;

                        if (nbMinerals <= 0)
                        {
                            if (isServer)
                            {
                                RpcDestroy();
                            }
                            else
                            {
                                CmdDestroy();
                                NetworkServer.Destroy(gameObject);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            CanvasManagerNet.instance.GetCanvasPeon().transform.GetChild(i).GetComponent<Ressources>().full = true;
                        }
                    }
                }
            }
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (lifeBar)
        {
            lifeBar.fillAmount = (float)nbMinerals / nbMineralsMax;
        }
        if (lifeCount)
        {
            lifeCount.text = nbMinerals + "/" + nbMineralsMax;
        }
    }

    private void GetAudioSource()
    {
        AudioSource[] miningSounds = GetComponentsInChildren<AudioSource>();
        if (miningSounds.Length > 0)
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

    [ClientRpc]
    void RpcDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }
    [Command]
    void CmdDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }
}
