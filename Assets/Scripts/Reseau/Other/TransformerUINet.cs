using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TransformerUINet : MonoBehaviour
{
    [SerializeField]
    public TransformerNet transformer;

    [SerializeField]
    public Button trap;

    [SerializeField]
    public Button weapons;

    [SerializeField]
    Button Fabrique;

    [SerializeField]
    Image craftTableImage;

    [SerializeField]
    Sprite CraftTable1;

    [SerializeField]
    Sprite CraftTable2;

    [SerializeField]
    Text textTrapMenu;

    [SerializeField]
    Text textWeaponMenu;

    [SerializeField]
    AudioSource craftMithrilWeapon;

    [SerializeField]
    AudioSource craftWeapon;

    [SerializeField]
    AudioSource craftTrap;

    CameraController cameraController;

    [SerializeField]
    public RackNet chest;

    [SerializeField]
    AudioSource soundCraft;

    [SerializeField]
    AudioSource craftTrapPoulay;

    /*[SerializeField]
    Text textCoutRessource;*/

    [SerializeField]
    Text textAttack;

    [SerializeField]
    Text textDurability;

    [SerializeField]
    Image cadre;

    [SerializeField]
    public EventSystem eventSystem;

    [SerializeField]
    GameObject textFlottant;

    UtilityStruct.TRAPS traps;

    Vector3 trapOldPos;
    Vector3 weaponsOldPos;
    
    int currentMenu;
    bool timerMenuIsTrigger;
    float timer;

    int currentRecipe = 1;
    Sprite oldSprite;

    public bool boolEnteringMenu;

    bool init = false;

    float craftingSpeed;
    public bool isCrafting = false;

    IsAWeaponsMenuNet weaponsMenu;
    IsATrapMenuNet trapMenu;

    GameObject selectedGameObject;

    [HideInInspector]
    public int indexCraftSword;
    [HideInInspector]
    public int indexCraftScythe;
    [HideInInspector]
    public int indexCraftHammer;

    public int GetCurrentRecipe()
    {
        return currentRecipe;
    }

    public void SetCurrentRecipe(int value)
    {
        currentRecipe = value;
    }

    public int GetCurrentMenu()
    {
        return currentMenu;
    }

    public WeaponRecipe.Weapon lastWeaponCrafted = WeaponRecipe.Weapon.NONE;


    void Init()
    {
        init = true;
        cameraController = CameraController.camPeon;
        craftingSpeed = PeonNet.instance.stats.craftingSpeed;

        if (!cameraController)
        {
            init = false;
        }

        oldSprite = cadre.sprite;

        trapOldPos = trap.transform.localPosition;
        weaponsOldPos = weapons.transform.localPosition;

        currentMenu = 1;
        timer = 0.15f;
        timerMenuIsTrigger = false;

        if (cameraController)
        {
            menuSelect();
        }

        weaponsMenu = GetComponentInChildren<IsAWeaponsMenuNet>(true);
        weaponsMenu.gameObject.SetActive(false);

        trapMenu = GetComponentInChildren<IsATrapMenuNet>(true);
        trapMenu.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            selectedGameObject = eventSystem.currentSelectedGameObject;
        }
        else
        {
            eventSystem.SetSelectedGameObject(selectedGameObject);
        }

        if (!init)
        {
            Init();
        }
        else
        {
            if (currentMenu == 1)
            {
                textTrapMenu.color = Color.red;
                textWeaponMenu.color = Color.black;
                if (PeonNet.instance != null)
                {
                    if (PeonNet.instance.nbWeaponRecipe > currentRecipe - 1)
                    {
                        cadre.sprite = PeonNet.instance.GiveTrapRecipe(currentRecipe - 1).icon;
                        soundCraft.clip = PeonNet.instance.GiveTrapRecipe(currentRecipe - 1).soundCraft;
                    }
                    else
                    {
                        cadre.sprite = oldSprite;
                    }
                }
            }
            else if (currentMenu == 2)
            {
                textTrapMenu.color = Color.black;
                textWeaponMenu.color = Color.red;
                if (PeonNet.instance.nbWeaponRecipe > currentRecipe - 1)
                {
                    cadre.sprite = PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).icon;
                    soundCraft.clip = PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).soundCraft;
                }
                else
                {
                    cadre.sprite = oldSprite;
                }
            }

            if (PeonNet.instance != null)
            {
                if (PeonNet.instance.Controller == Controller.J1 || PeonNet.instance.Controller == Controller.J2)
                {
                    if (Input.GetButton(PeonNet.instance.Controller + "_RB") && currentMenu < 2 && !timerMenuIsTrigger)
                    {
                        eventSystem.SetSelectedGameObject(weapons.gameObject);
                        currentMenu++;
                        timer = 0.15f;
                        menuSelect(currentMenu);
                        timerMenuIsTrigger = true;
                    }
                    if (Input.GetButton(PeonNet.instance.Controller + "_LB") && currentMenu > 1 && !timerMenuIsTrigger)
                    {
                        eventSystem.SetSelectedGameObject(trap.gameObject);
                        currentMenu--;
                        timer = 0.15f;
                        menuSelect(currentMenu);
                        timerMenuIsTrigger = true;
                    }

                    if (timerMenuIsTrigger && timer > 0.0f)
                    {
                        timer -= Time.deltaTime;
                    }
                    else
                    {
                        timerMenuIsTrigger = false;
                    }

                    if (Input.GetButtonDown(PeonNet.instance.Controller + "_X") && transformer.isUIActive == true)
                    {
                        StartCoroutineFabrique();
                    }
                }
            }
        }
    }
    
    public void menuSelect(int menuState = 1)
    {
        currentMenu = menuState;
        currentRecipe = 1;

        onClick(1);

        if (menuState == 1)
        {
            craftTableImage.sprite = CraftTable1;

            weapons.GetComponent<RectTransform>().localPosition = weaponsOldPos;
            weapons.GetComponent<RectTransform>().sizeDelta = new Vector2(weapons.GetComponent<RectTransform>().sizeDelta.x, 28.5f);
            
            IsAWeaponsMenuNet weaponsMenu = GetComponentInChildren<IsAWeaponsMenuNet>(true);
            weaponsMenu.gameObject.SetActive(false);

            IsATrapMenuNet trapMenu = GetComponentInChildren<IsATrapMenuNet>(true);
            trapMenu.gameObject.SetActive(true);

            trap.GetComponent<RectTransform>().localPosition = new Vector3(trap.GetComponent<RectTransform>().localPosition.x, 181.0f, trap.GetComponent<RectTransform>().localPosition.z);
            trap.GetComponent<RectTransform>().sizeDelta = new Vector2(trap.GetComponent<RectTransform>().sizeDelta.x, 43);
        }
        else if (menuState == 2)
        {
            craftTableImage.sprite = CraftTable2;

            trap.GetComponent<RectTransform>().localPosition = trapOldPos;
            trap.GetComponent<RectTransform>().sizeDelta = new Vector2(trap.GetComponent<RectTransform>().sizeDelta.x, 28.5f);
            
            IsAWeaponsMenuNet weaponsMenu = GetComponentInChildren<IsAWeaponsMenuNet>(true);
            weaponsMenu.gameObject.SetActive(true);

            IsATrapMenuNet trapMenu = GetComponentInChildren<IsATrapMenuNet>(true);
            trapMenu.gameObject.SetActive(false);

            weapons.GetComponent<RectTransform>().localPosition = new Vector3(weapons.GetComponent<RectTransform>().localPosition.x, 181.0f, weapons.GetComponent<RectTransform>().localPosition.z);
            weapons.GetComponent<RectTransform>().sizeDelta = new Vector2(weapons.GetComponent<RectTransform>().sizeDelta.x, 43);
        }
    }
    

    public void onClick(int buttonPressed)
    {
        currentRecipe = buttonPressed;

        if (currentMenu == 1)
        {
            textAttack.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 57] + " : " + PeonNet.instance.GiveTrapRecipe(currentRecipe - 1).GetTrapMaxUses();
            textDurability.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 58] + " : " + PeonNet.instance.GiveTrapRecipe(currentRecipe - 1).GetTrapDamages();
        }
        else
        {
            textAttack.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 49] + " : " + PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetAttack();
            textDurability.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 50] + " : " + PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetDurability();
        }

        GetComponentInChildren<ResourcesCostNet>().SetResourcesImages();
    }

    void FloatingText()
    {
        GameObject temp = Instantiate(textFlottant, transform);
        temp.GetComponentInChildren<Image>().sprite = PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).iconResource[0];

        if (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetWoodRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetWoodRessources();
        }
        if (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetIronRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetIronRessources();
        }
        if (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetMithrilRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetMithrilRessources();
        }
        if (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetDiamondRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetDiamondRessources();
        }
    }

    public void CraftMithrilWeapon()
    {
        if (craftMithrilWeapon)
        {
            if (!AudioTypeBehaviour.voiceIsPlaying)
            {
                craftMithrilWeapon.Play();
                AudioTypeBehaviour.voiceIsPlaying = true;
                craftMithrilWeapon.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", craftMithrilWeapon.clip.length);
            }
        }
    }

    public void CraftWeapon()
    {
        if (craftWeapon)
        {
            if (!AudioTypeBehaviour.voiceIsPlaying)
            {
                craftWeapon.Play();
                AudioTypeBehaviour.voiceIsPlaying = true;
                craftWeapon.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", craftWeapon.clip.length);
            }
        }
    }

    public void CraftTrapPoulay()
    {
        if (craftTrapPoulay)
        {
            if (!AudioTypeBehaviour.voiceIsPlaying)
            {
                craftTrapPoulay.Play();
                AudioTypeBehaviour.voiceIsPlaying = true;
                craftTrapPoulay.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", craftTrapPoulay.clip.length);
            }
        }
    }

    public void CraftTrap()
    {
        if (craftTrap)
        {
            if (!AudioTypeBehaviour.voiceIsPlaying)
            {
                craftTrap.Play();
                AudioTypeBehaviour.voiceIsPlaying = true;
                craftTrap.GetComponent<AudioTypeBehaviour>().Invoke("ResetVoieIsPlaying", craftTrap.clip.length);
            }
        }
    }
    public void StartCoroutineFabrique()
    {
        StartCoroutine(CoroutineFabrique());
    }

    private IEnumerator CoroutineFabrique()
    {
        if (!isCrafting)
        {
            isCrafting = true;
            int tempCurrentRecipe = currentRecipe;
            // bouton craft en rouge
            ColorBlock colorFabrique = Fabrique.colors;
            colorFabrique.normalColor = new Color(1, 0, 0, 1);
            colorFabrique.highlightedColor = new Color(1, 0, 0, 1);
            Fabrique.colors = colorFabrique;

            // enlever les matos
            if (currentMenu == 1)
            {
                if (PeonNet.instance.GiveTrapRecipe(tempCurrentRecipe - 1).DoRecipe(ref transformer.acquiredMinerals))
                {
                    FloatingText();
                    soundCraft.Play();

                    yield return new WaitForSeconds(2.0f);

                    switch (PeonNet.instance.GiveTrapRecipe(tempCurrentRecipe - 1).GetTrap())
                    {
                        case TrapRecipe.Trap.CHICKEN:
                            CraftTrapPoulay();
                            TrapPadNet.instance.inventoryTrap.chicken++;
                            break;
                        case TrapRecipe.Trap.ICE:
                            CraftTrap();
                            TrapPadNet.instance.inventoryTrap.ice++;
                            break;
                        case TrapRecipe.Trap.MINE:
                            CraftTrap();
                            TrapPadNet.instance.inventoryTrap.mine++;
                            break;
                        case TrapRecipe.Trap.SPIKE:
                            CraftTrap();
                            TrapPadNet.instance.inventoryTrap.spike++;
                            break;
                        case TrapRecipe.Trap.ARROWSLIT:
                            CraftTrap();
                            TrapPadNet.instance.inventoryTrap.arrowslit++;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (currentMenu == 2)
            {
                if (PeonNet.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).DoRecipe(ref transformer.acquiredMinerals))
                {
                    FloatingText();

                    // Affichage de l'arme dans le ratelier
                    int index1 = chest.swordIndex;
                    int index2 = chest.scytheIndex;
                    int index3 = chest.hammerIndex;
                    switch (PeonNet.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon())
                    {
                        case WeaponRecipe.Weapon.WOODSWORD:
                            indexCraftSword = 1;
                            lastWeaponCrafted = WeaponRecipe.Weapon.WOODSWORD;
                            if (chest.swordIndex < 1)
                            {
                                chest.swordIndex = 1;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODSWORD;
                            }
                            break;
                        case WeaponRecipe.Weapon.WOODSCYTHE:
                            indexCraftScythe = 1;
                            lastWeaponCrafted = WeaponRecipe.Weapon.WOODSCYTHE;
                            if (chest.scytheIndex < 1)
                            {
                                chest.scytheIndex = 1;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODSCYTHE;
                            }
                            break;
                        case WeaponRecipe.Weapon.WOODHAMMER:
                            indexCraftHammer = 1;
                            lastWeaponCrafted = WeaponRecipe.Weapon.WOODHAMMER;
                            if (chest.hammerIndex < 1)
                            {
                                chest.hammerIndex = 1;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODHAMMER;
                            }
                            break;
                        case WeaponRecipe.Weapon.IRONSWORD:
                            indexCraftSword = 2;
                            lastWeaponCrafted = WeaponRecipe.Weapon.IRONSWORD;
                            if (chest.swordIndex < 2)
                            {
                                chest.swordIndex = 2;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONSWORD;
                            }
                            break;
                        case WeaponRecipe.Weapon.IRONSCYTHE:
                            indexCraftScythe = 2;
                            lastWeaponCrafted = WeaponRecipe.Weapon.IRONSCYTHE;
                            if (chest.scytheIndex < 2)
                            {
                                chest.scytheIndex = 2;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONSCYTHE;
                            }
                            break;
                        case WeaponRecipe.Weapon.IRONHAMMER:
                            indexCraftHammer = 2;
                            lastWeaponCrafted = WeaponRecipe.Weapon.IRONHAMMER;
                            if (chest.hammerIndex < 2)
                            {
                                chest.hammerIndex = 2;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONHAMMER;
                            }
                            break;
                        case WeaponRecipe.Weapon.DIAMONDSWORD:
                            indexCraftSword = 3;
                            lastWeaponCrafted = WeaponRecipe.Weapon.DIAMONDSWORD;
                            if (chest.swordIndex < 3)
                            {
                                chest.swordIndex = 3;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDSWORD;
                            }
                            break;
                        case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                            indexCraftScythe = 3;
                            lastWeaponCrafted = WeaponRecipe.Weapon.DIAMONDSCYTHE;
                            if (chest.scytheIndex < 3)
                            {
                                chest.scytheIndex = 3;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDSCYTHE;
                            }
                            break;
                        case WeaponRecipe.Weapon.DIAMONDHAMMER:
                            indexCraftHammer = 3;
                            lastWeaponCrafted = WeaponRecipe.Weapon.DIAMONDHAMMER;
                            if (chest.hammerIndex < 3)
                            {
                                chest.hammerIndex = 3;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDHAMMER;
                            }
                            break;
                        case WeaponRecipe.Weapon.MITHRILSWORD:
                            indexCraftSword = 4;
                            lastWeaponCrafted = WeaponRecipe.Weapon.MITHRILSWORD;
                            if (chest.swordIndex < 4)
                            {
                                chest.swordIndex = 4;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILSWORD;
                            }
                            break;
                        case WeaponRecipe.Weapon.MITHRILSCYTHE:
                            indexCraftScythe = 4;
                            lastWeaponCrafted = WeaponRecipe.Weapon.MITHRILSCYTHE;
                            if (chest.scytheIndex < 4)
                            {
                                chest.scytheIndex = 4;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILSCYTHE;
                            }
                            break;
                        case WeaponRecipe.Weapon.MITHRILHAMMER:
                            indexCraftHammer = 4;
                            lastWeaponCrafted = WeaponRecipe.Weapon.MITHRILHAMMER;
                            if (chest.hammerIndex < 4)
                            {
                                chest.hammerIndex = 4;
                                chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILHAMMER;
                            }
                            break;
                        default:
                            break;
                    }



                    if ((index1 + index2 + index3) != (chest.swordIndex + chest.scytheIndex + chest.hammerIndex))
                    {
                        chest.DisplayWeapons();
                    }

                    yield return new WaitForSeconds(2.0f);
                    soundCraft.Play();

                    //chest.weaponList.Add(PeonNet.instance.GiveWeaponRecipe(tempCurrentRecipe - 1));
                    
                    if (PeonNet.instance.isServer)
                    {
                        PeonNet.instance.RpcAddWeapon(currentRecipe - 1, chest.swordIndex, chest.scytheIndex, chest.hammerIndex, index1, index2, index3);
                    }
                    else
                    {
                        PeonNet.instance.CmdAddWeapon(currentRecipe - 1, chest.swordIndex, chest.scytheIndex, chest.hammerIndex, index1, index2, index3);
                    }

                    if (PeonNet.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILHAMMER
                        || PeonNet.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILSCYTHE
                        || PeonNet.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILSWORD)
                    {
                        CraftMithrilWeapon();
                    }
                    else
                    {
                        CraftWeapon();
                    }

                    PeonNet.instance.hasCraftSmth = true;
                    WeaponUpInChest.weaponCrafted = tempCurrentRecipe - 1;
                }
            }
            //afficher l'arme

            //appeler pol pour faire disparaitre l'arme

            colorFabrique.normalColor = new Color(1, 1, 1, 1);
            colorFabrique.highlightedColor = new Color(0, 1, 0, 1);
            Fabrique.colors = colorFabrique;
            isCrafting = false;
        }
    }

    //public void OnClickFabrique(bool fabrique)
    //{
    //    if(!PeonNet.instance.isLocalPlayer)
    //    {
    //        return;
    //    }

    //    if (currentMenu == 1)
    //    {
    //        if (PeonNet.instance.GiveTrapRecipe(currentRecipe - 1).DoRecipe(ref transformer.acquiredMinerals))
    //        {
    //            soundCraft.Play();

    //            switch (PeonNet.instance.GiveTrapRecipe(currentRecipe - 1).GetTrap())
    //            {
    //                case TrapRecipe.Trap.CHICKEN:
    //                    CraftTrapPoulay();
    //                    TrapPadNet.instance.inventoryTrap.chicken++;
    //                    break;
    //                case TrapRecipe.Trap.ICE:
    //                    CraftTrap();
    //                    TrapPadNet.instance.inventoryTrap.ice++;
    //                    break;
    //                case TrapRecipe.Trap.MINE:
    //                    CraftTrap();
    //                    TrapPadNet.instance.inventoryTrap.mine++;
    //                    break;
    //                case TrapRecipe.Trap.SPIKE:
    //                    CraftTrap();
    //                    TrapPadNet.instance.inventoryTrap.spike++;
    //                    break;
    //                case TrapRecipe.Trap.ARROWSLIT:
    //                    CraftTrap();
    //                    TrapPadNet.instance.inventoryTrap.arrowslit++;
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //    else if (currentMenu == 2)
    //    {
    //        if (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).DoRecipe(ref transformer.acquiredMinerals))
    //        {
    //            soundCraft.Play();

    //            //chest.weaponList.Add(PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1));

    //            // Affichage de l'arme dans le ratelier
    //            int index1 = chest.swordIndex;
    //            int index2 = chest.scytheIndex;
    //            int index3 = chest.scytheIndex;

    //            switch (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetWeapon())
    //            {
    //                case WeaponRecipe.Weapon.WOODSWORD:
    //                    if (chest.swordIndex < 1)
    //                    {
    //                        chest.swordIndex = 1;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.WOODSCYTHE:
    //                    if (chest.scytheIndex < 1)
    //                    {
    //                        chest.scytheIndex = 1;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.WOODHAMMER:
    //                    if (chest.hammerIndex < 1)
    //                    {
    //                        chest.hammerIndex = 1;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.IRONSWORD:
    //                    if (chest.swordIndex < 2)
    //                    {
    //                        chest.swordIndex = 2;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.IRONSCYTHE:
    //                    if (chest.scytheIndex < 2)
    //                    {
    //                        chest.scytheIndex = 2;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.IRONHAMMER:
    //                    if (chest.hammerIndex < 2)
    //                    {
    //                        chest.hammerIndex = 2;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.DIAMONDSWORD:
    //                    if (chest.swordIndex < 3)
    //                    {
    //                        chest.swordIndex = 3;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.DIAMONDSCYTHE:
    //                    if (chest.scytheIndex < 3)
    //                    {
    //                        chest.scytheIndex = 3;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.DIAMONDHAMMER:
    //                    if (chest.hammerIndex < 3)
    //                    {
    //                        chest.hammerIndex = 3;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.MITHRILSWORD:
    //                    if (chest.swordIndex < 4)
    //                    {
    //                        chest.swordIndex = 4;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.MITHRILSCYTHE:
    //                    if (chest.scytheIndex < 4)
    //                    {
    //                        chest.scytheIndex = 4;
    //                    }
    //                    break;
    //                case WeaponRecipe.Weapon.MITHRILHAMMER:
    //                    if (chest.hammerIndex < 4)
    //                    {
    //                        chest.hammerIndex = 4;
    //                    }
    //                    break;
    //                default:
    //                    break;
    //            }

    //            if ((index1 + index2 + index3) != (chest.swordIndex + chest.scytheIndex + chest.hammerIndex))
    //            {
    //                chest.DisplayWeapons();
    //            }

    //            if (PeonNet.instance.isServer)
    //            {
    //                PeonNet.instance.RpcAddWeapon(currentRecipe - 1, chest.swordIndex, chest.scytheIndex, chest.hammerIndex, index1, index2, index3);
    //            }
    //            else
    //            {
    //                PeonNet.instance.CmdAddWeapon(currentRecipe - 1, chest.swordIndex, chest.scytheIndex, chest.hammerIndex, index1, index2, index3);
    //            }

    //            if (PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILHAMMER
    //                || PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILSCYTHE
    //                || PeonNet.instance.GiveWeaponRecipe(currentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILSWORD)
    //            {
    //                CraftMithrilWeapon();
    //            }
    //            else
    //            {
    //                CraftWeapon();
    //            }

    //            PeonNet.instance.hasCraftSmth = true;
    //            WeaponUpInChest.weaponCrafted = currentRecipe - 1;

    //            FloatingText();
    //        }
    //    }
    //}
}