using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TransformerUI : MonoBehaviour
{
    [SerializeField]
    public Transformer transformer;

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
    public Rack chest;

    [SerializeField]
    AudioSource soundCraft;

    [SerializeField]
    AudioSource craftTrapPoulay;

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

    IsAWeaponsMenu weaponsMenu;
    IsATrapMenu trapMenu;

    GameObject selectedGameObject;

    [HideInInspector]
    public int indexCraftSword;
    [HideInInspector]
    public int indexCraftScythe;
    [HideInInspector]
    public int indexCraftHammer;

    [HideInInspector]
    public WeaponRecipe.Weapon lastWeaponCrafted = WeaponRecipe.Weapon.NONE;


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



    void Init()
    {
        init = true;
        cameraController = CameraController.camPeon;
        craftingSpeed = Peon.instance.stats.craftingSpeed;
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

        weaponsMenu = GetComponentInChildren<IsAWeaponsMenu>(true);
        weaponsMenu.gameObject.SetActive(false);

        trapMenu = GetComponentInChildren<IsATrapMenu>(true);
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
                if (Peon.instance.nbWeaponRecipe > currentRecipe - 1)
                {
                    cadre.sprite = Peon.instance.GiveTrapRecipe(currentRecipe - 1).icon;
                    soundCraft.clip = Peon.instance.GiveTrapRecipe(currentRecipe - 1).soundCraft;
                }
                else
                {
                    cadre.sprite = oldSprite;
                }
            }
            else if (currentMenu == 2)
            {
                textTrapMenu.color = Color.black;
                textWeaponMenu.color = Color.red;
                if (Peon.instance.nbWeaponRecipe > currentRecipe - 1)
                {
                    cadre.sprite = Peon.instance.GiveWeaponRecipe(currentRecipe - 1).icon;
                    soundCraft.clip = Peon.instance.GiveWeaponRecipe(currentRecipe - 1).soundCraft;
                }
                else
                {
                    cadre.sprite = oldSprite;
                }
            }

            if (Peon.instance != null)
            {
                if (Peon.instance.Controller == Controller.J1 || Peon.instance.Controller == Controller.J2)
                {
                    if (Input.GetButton(Peon.instance.Controller + "_RB") && currentMenu < 2 && !timerMenuIsTrigger)
                    {
                        eventSystem.SetSelectedGameObject(weapons.gameObject);
                        currentMenu++;
                        timer = 0.15f;
                        menuSelect(currentMenu);
                        timerMenuIsTrigger = true;
                    }
                    if (Input.GetButton(Peon.instance.Controller + "_LB") && currentMenu > 1 && !timerMenuIsTrigger)
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

                    if (Input.GetButtonDown(Peon.instance.Controller + "_X") && transformer.isUIActive == true)
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


            IsAWeaponsMenu weaponsMenu = GetComponentInChildren<IsAWeaponsMenu>(true);
            weaponsMenu.gameObject.SetActive(false);

            IsATrapMenu trapMenu = GetComponentInChildren<IsATrapMenu>(true);
            trapMenu.gameObject.SetActive(true);

            trap.GetComponent<RectTransform>().localPosition = new Vector3(trap.GetComponent<RectTransform>().localPosition.x, 181.0f, trap.GetComponent<RectTransform>().localPosition.z);
            trap.GetComponent<RectTransform>().sizeDelta = new Vector2(trap.GetComponent<RectTransform>().sizeDelta.x, 43);

        }
        else if (menuState == 2)
        {
            craftTableImage.sprite = CraftTable2;

            trap.GetComponent<RectTransform>().localPosition = trapOldPos;
            trap.GetComponent<RectTransform>().sizeDelta = new Vector2(trap.GetComponent<RectTransform>().sizeDelta.x, 28.5f);


            IsAWeaponsMenu weaponsMenu = GetComponentInChildren<IsAWeaponsMenu>(true);
            weaponsMenu.gameObject.SetActive(true);

            IsATrapMenu trapMenu = GetComponentInChildren<IsATrapMenu>(true);
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
            textAttack.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 57] + " : " + Peon.instance.GiveTrapRecipe(currentRecipe - 1).GetTrapMaxUses();
            textDurability.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 58] + " : " + Peon.instance.GiveTrapRecipe(currentRecipe - 1).GetTrapDamages();
        }
        else
        {
            textAttack.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 49] + " : " + Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetAttack();
            textDurability.text = DataManager.instance.myTab[(int)DataManager.instance.language + 1, 50] + " : " + Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetDurability();
        }

        GetComponentInChildren<ResourcesCost>().SetResourcesImages();
    }

    void FloatingText()
    {
        GameObject temp = Instantiate(textFlottant, transform);
        temp.GetComponentInChildren<Image>().sprite = Peon.instance.GiveWeaponRecipe(currentRecipe - 1).iconResource[0];

        if (Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetWoodRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetWoodRessources();
        }
        if (Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetIronRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetIronRessources();
        }
        if (Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetMithrilRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetMithrilRessources();
        }
        if (Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetDiamondRessources() > 0)
        {
            temp.GetComponentInChildren<Text>().text = "-" + Peon.instance.GiveWeaponRecipe(currentRecipe - 1).GetDiamondRessources();
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
                if (Peon.instance.GiveTrapRecipe(tempCurrentRecipe - 1).DoRecipe(ref transformer.acquiredMinerals))
                {
                    FloatingText();
                    soundCraft.Play();

                    yield return new WaitForSeconds(craftingSpeed);

                    switch (Peon.instance.GiveTrapRecipe(tempCurrentRecipe - 1).GetTrap())
                    {
                        case TrapRecipe.Trap.CHICKEN:
                            CraftTrapPoulay();
                            TrapPad.instance.inventoryTrap.chicken++;
                            break;
                        case TrapRecipe.Trap.ICE:
                            CraftTrap();
                            TrapPad.instance.inventoryTrap.ice++;
                            break;
                        case TrapRecipe.Trap.MINE:
                            CraftTrap();
                            TrapPad.instance.inventoryTrap.mine++;
                            break;
                        case TrapRecipe.Trap.SPIKE:
                            CraftTrap();
                            TrapPad.instance.inventoryTrap.spike++;
                            break;
                        case TrapRecipe.Trap.ARROWSLIT:
                            CraftTrap();
                            TrapPad.instance.inventoryTrap.arrowslit++;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (currentMenu == 2)
            {
                if (Peon.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).DoRecipe(ref transformer.acquiredMinerals))
                {
                    FloatingText();

                    // Affichage de l'arme dans le ratelier
                    int index1 = chest.swordIndex;
                    int index2 = chest.scytheIndex;
                    int index3 = chest.hammerIndex;
                    switch (Peon.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon())
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

                    yield return new WaitForSeconds(craftingSpeed);
                    soundCraft.Play();

                    chest.weaponList.Add(Peon.instance.GiveWeaponRecipe(tempCurrentRecipe - 1));

                    if (Peon.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILHAMMER
                        || Peon.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILSCYTHE
                        || Peon.instance.GiveWeaponRecipe(tempCurrentRecipe - 1).GetWeapon() == WeaponRecipe.Weapon.MITHRILSWORD)
                    {
                        CraftMithrilWeapon();
                    }
                    else
                    {
                        CraftWeapon();
                    }

                    Peon.instance.hasCraftSmth = true;
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
}

