using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.Audio;

public enum Controller
{
    K1,
    J1,
    J2
}

public class PeonNet : NetworkBehaviour
{
    public static PeonNet instance;

    [SyncVar]
    public bool hasCraftSmth = false;

    [SerializeField]
    Transform pivotCam;

    Controller controller;

    [SerializeField]
    WeaponRecipe[] weaponRecipe;

    [SerializeField]
    AudioSource stunSound;

    [SerializeField]
    TrapRecipe[] trapRecipe;

    [SerializeField]
    AudioSource walkSound;

    [SerializeField]
    AudioSource tauntSound;

    Rigidbody rb;
    public UtilityStruct.STATSPEON stats;

    public int totalMinerals = 0;
    public UtilityStruct.MINERALS minerals;

    public int nbWeaponRecipe;
    public int nbTrapRecipe;

    public bool playedSoundOfDeath = false;

    public float currentStamina = 5.0f;
    public float maxStamina = 5.0f;
    int boostFactor = 5;

    Animator anim;

    public bool isBusy = false;
    public bool isMining = false;

    //rotation
    Vector3 lastVelocity;

    //stun
    bool isStun = false;
    float stunTimer = 0.0f;

    RackNet chest;

    bool initSuccess = false;

    void initStructs()
    {
        //minerals
        minerals.wood = 0;
        minerals.iron = 0;
        minerals.diamond = 0;
        minerals.mithril = 0;

        //stats perso
        stats.movementSpeed = 10 + 2 * PlayerPrefs.GetInt("peonTalentMovementSpeed");
        stats.pickAxe = 5 + 3 * PlayerPrefs.GetInt("peontalentPickAxe");
        stats.craftingSpeed = 5 + 1 * PlayerPrefs.GetInt("peonTalentCraftingSpeed");
        stats.gatheringSpeed = 5 + 1 * PlayerPrefs.GetInt("peonTalentGatheringSpeed");
        stats.backPack = 20 + 10 * PlayerPrefs.GetInt("peonTalentInventory");
    }

    public Controller Controller
    {
        get
        {
            return controller;
        }
    }

    public Transform PivotCam
    {
        get
        {
            return pivotCam;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        pivotCam = CameraController.camPeon.transform;

        if (DataManager.instance)
        {
            initSuccess = true;
            controller = DataManager.instance.ControllerPeon;
        }

        if (isLocalPlayer)
        {
            pivotCam.gameObject.SetActive(true);
            pivotCam.GetComponent<CameraController>().screenOption = CameraController.ScreenMode.fullscreen;
            DataManager.instance.ScreenModePeon = CameraController.ScreenMode.fullscreen;
        }
        else
        {
            pivotCam.gameObject.SetActive(false);
            pivotCam.GetComponent<CameraController>().screenOption = CameraController.ScreenMode.none;
            DataManager.instance.ScreenModePeon = CameraController.ScreenMode.none;
        }

        pivotCam.GetComponent<CameraController>().Ctrl = controller;
        pivotCam.GetComponent<CameraController>().SetTarget(transform);

        pivotCam.GetComponent<CameraController>().SetCameraPreset();

        nbWeaponRecipe = weaponRecipe.Length;
        nbTrapRecipe = trapRecipe.Length;

        initStructs();

        if (!isLocalPlayer)
        {
            CanvasManagerNet.instance.GetCanvasPeon().enabled = false;
        }

        Invoke("InitChest", 3.0f);
    }

    void InitChest()
    {
        chest = FindObjectOfType<RackNet>();
    }

    private void LateUpdate()
    {
        pivotCam.gameObject.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if not the local player, don't do anything
        if (!isLocalPlayer)
        {
            return;
        }

        if (!initSuccess)
        {
            Init();
        }
        else
        {
            anim.SetFloat("velocity", rb.velocity.magnitude);

            if (!isBusy && !isStun && !isMining)
            {
                UpdateDeplacement();
                UpdateRotation();
                UpdateJump();
            }
            else if (isStun)
            {
                UpdateIfisStun();
            }
        }
        UpdateCooldown();
    }

    void UpdateCooldown()
    {
        if (!(Controller == Controller.K1 && Input.GetKey(KeyCode.LeftShift)) ||
            !((Controller == Controller.J1 || Controller == Controller.J2) && Input.GetButton(Controller + "_LStick")))
        {
            currentStamina += Time.deltaTime / 2;
            if (currentStamina >= maxStamina)
                currentStamina = maxStamina;
        }
    }

    void UpdateIfisStun()
    {
        if (stunTimer >= 1.0f)
        {
            stunTimer = 0;
            isStun = false;
        }
        else
        {
            stunTimer += Time.deltaTime;
        }
    }

    public void TauntSound()
    {
        if (tauntSound && playedSoundOfDeath == false)
        {
            tauntSound.Play();
            playedSoundOfDeath = true;
        }
    }

    void UpdateDeplacement()
    {
        Vector3 velocity = Vector3.zero;

        velocity = PivotCam.forward * Input.GetAxisRaw(Controller + "_LYaxis");
        velocity += PivotCam.right * Input.GetAxisRaw(Controller + "_LXaxis");

        velocity.Normalize();

        if (((Controller == Controller.K1 && Input.GetKey(KeyCode.LeftShift)) ||
            (Controller == Controller.J1 || Controller == Controller.J2) && Input.GetButton(Controller + "_LStick"))
            && currentStamina > 0.0f)
        {
            currentStamina -= Time.deltaTime;
            velocity *= (stats.movementSpeed + boostFactor);
            anim.SetBool("sprint", true);
        }
        else
        {
            // currentStamina += Time.deltaTime/2;
            // if (currentStamina >= maxStamina)
            //     currentStamina = maxStamina;
            velocity *= stats.movementSpeed;
            anim.SetBool("sprint", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_Peon") || anim.GetCurrentAnimatorStateInfo(0).IsName("Peon_Sprint"))
        {
            anim.speed = stats.movementSpeed / 10;
        }

        velocity.y = rb.velocity.y;
        rb.velocity = velocity;


        PivotCam.position = transform.position;

        // A smoothé
        pivotCam.position = Vector3.Lerp(pivotCam.position, transform.position, 0.1f);
    }

    public void WalkSound()
    {
        if (walkSound)
        {
            walkSound.Play();
        }
    }

    void UpdateRotation()
    {
        Quaternion rot;

        if (Input.GetAxisRaw(controller + "_LXaxis") != 0.0f || Input.GetAxisRaw(controller + "_LYaxis") != 0.0f)
        {
            rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lastVelocity), 0.3f);
            lastVelocity = rb.velocity;
            transform.rotation = rot;
        }
    }

    void UpdateJump()
    {
        if (IsGrounded())
        {
            if (Controller == Controller.K1 && Input.GetButtonDown(Controller + "_Space"))
            {
                Vector3 velocity = rb.velocity;
                velocity.y += 4;
                rb.velocity = velocity;
            }
            if ((Controller == Controller.J1 || Controller == Controller.J2) && Input.GetButtonDown(Controller + "_A"))
            {
                Vector3 velocity = rb.velocity;
                velocity.y = 8.0f;
                rb.velocity = velocity;
            }
        }
    }

    bool IsGrounded()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position - transform.up * 0.75f, 0.3f))
        {
            if (c.CompareTag("PeonFloor") || c.CompareTag("BossFloor"))
            {
                return true;
            }
        }
        return false;
    }

    public void SetStun()
    {
        isStun = true;
        StunSound();
    }

    public void StunSound()
    {
        if (stunSound)
        {
            stunSound.Play();
        }
    }

    public WeaponRecipe GiveWeaponRecipe(int currentRecipe)
    {
        return weaponRecipe[currentRecipe];
    }

    public TrapRecipe GiveTrapRecipe(int currentRecipe)
    {
        return trapRecipe[currentRecipe];
    }

    public CameraController GetCameraController()
    {
        return pivotCam.GetComponent<CameraController>();
    }

    public void ResetMining()
    {
        isMining = false;
    }

    [ClientRpc]
    public void RpcPlaceTrapNet(GameObject go)
    {
        go.GetComponent<TrapNet>().PlaceTrap();
    }

    [Command]//use to init a trap from the client to the server
    public void CmdSpawnTrapNet(NetworkHash128 hash, Vector3 position, Quaternion rotation)
    {
        GameObject go = Instantiate(ClientScene.prefabs[hash], position, rotation);
        TrapPadNet.instance.AddTrapInList(go);
        if (go.GetComponent<Renderer>())
        {
            go.GetComponent<Renderer>().material.color = Color.white;
        }
        go.GetComponent<TrapNet>().PlaceTrap();
        NetworkServer.Spawn(go);
    }

    [Command]
    public void CmdSpawnAChosenObject(NetworkHash128 hash, Vector3 position, Quaternion rotation)
    {
        GameObject go = Instantiate(ClientScene.prefabs[hash], position, rotation);
        NetworkServer.Spawn(go);
    }

    [Command]
    public void CmdAddWeapon(int currentRecipe, int swordIndex, int scytheIndex, int hammerIndex, int index1, int index2, int index3)
    {
        chest.weaponList.Add(instance.GiveWeaponRecipe(currentRecipe));

        switch (PeonNet.instance.GiveWeaponRecipe(currentRecipe).GetWeapon())
        {
            case WeaponRecipe.Weapon.WOODSWORD:
                if (chest.swordIndex < 1)
                {
                    chest.swordIndex = 1;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODSWORD;
                }
                break;
            case WeaponRecipe.Weapon.WOODSCYTHE:
                if (chest.scytheIndex < 1)
                {
                    chest.scytheIndex = 1;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.WOODHAMMER:
                if (chest.hammerIndex < 1)
                {
                    chest.hammerIndex = 1;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODHAMMER;
                }
                break;
            case WeaponRecipe.Weapon.IRONSWORD:
                if (chest.swordIndex < 2)
                {
                    chest.swordIndex = 2;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONSWORD;
                }
                break;
            case WeaponRecipe.Weapon.IRONSCYTHE:
                if (chest.scytheIndex < 2)
                {
                    chest.scytheIndex = 2;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.IRONHAMMER:
                if (chest.hammerIndex < 2)
                {
                    chest.hammerIndex = 2;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONHAMMER;
                }
                break;
            case WeaponRecipe.Weapon.DIAMONDSWORD:
                if (chest.swordIndex < 3)
                {
                    chest.swordIndex = 3;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDSWORD;
                }
                break;
            case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                if (chest.scytheIndex < 3)
                {
                    chest.scytheIndex = 3;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.DIAMONDHAMMER:
                if (chest.hammerIndex < 3)
                {
                    chest.hammerIndex = 3;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDHAMMER;
                }
                break;
            case WeaponRecipe.Weapon.MITHRILSWORD:
                if (chest.swordIndex < 4)
                {
                    chest.swordIndex = 4;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILSWORD;
                }
                break;
            case WeaponRecipe.Weapon.MITHRILSCYTHE:
                if (chest.scytheIndex < 4)
                {
                    chest.scytheIndex = 4;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.MITHRILHAMMER:
                if (chest.hammerIndex < 4)
                {
                    chest.hammerIndex = 4;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILHAMMER;
                }
                break;
            default:
                break;
        }

        chest.swordIndex = swordIndex;
        chest.scytheIndex = scytheIndex;
        chest.hammerIndex = hammerIndex;

        if ((index1 + index2 + index3) != (chest.swordIndex + chest.scytheIndex + chest.hammerIndex))
        {
            chest.DisplayWeapons();
        }

        instance.hasCraftSmth = true;
        WeaponUpInChest.weaponCrafted = currentRecipe;
    }

    [ClientRpc]
    public void RpcAddWeapon(int currentRecipe, int swordIndex, int scytheIndex, int hammerIndex, int index1, int index2, int index3)
    {
        chest.weaponList.Add(instance.GiveWeaponRecipe(currentRecipe));

        switch (PeonNet.instance.GiveWeaponRecipe(currentRecipe).GetWeapon())
        {
            case WeaponRecipe.Weapon.WOODSWORD:
                if (chest.swordIndex < 1)
                {
                    chest.swordIndex = 1;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODSWORD;
                }
                break;
            case WeaponRecipe.Weapon.WOODSCYTHE:
                if (chest.scytheIndex < 1)
                {
                    chest.scytheIndex = 1;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.WOODHAMMER:
                if (chest.hammerIndex < 1)
                {
                    chest.hammerIndex = 1;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.WOODHAMMER;
                }
                break;
            case WeaponRecipe.Weapon.IRONSWORD:
                if (chest.swordIndex < 2)
                {
                    chest.swordIndex = 2;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONSWORD;
                }
                break;
            case WeaponRecipe.Weapon.IRONSCYTHE:
                if (chest.scytheIndex < 2)
                {
                    chest.scytheIndex = 2;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.IRONHAMMER:
                if (chest.hammerIndex < 2)
                {
                    chest.hammerIndex = 2;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.IRONHAMMER;
                }
                break;
            case WeaponRecipe.Weapon.DIAMONDSWORD:
                if (chest.swordIndex < 3)
                {
                    chest.swordIndex = 3;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDSWORD;
                }
                break;
            case WeaponRecipe.Weapon.DIAMONDSCYTHE:
                if (chest.scytheIndex < 3)
                {
                    chest.scytheIndex = 3;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.DIAMONDHAMMER:
                if (chest.hammerIndex < 3)
                {
                    chest.hammerIndex = 3;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.DIAMONDHAMMER;
                }
                break;
            case WeaponRecipe.Weapon.MITHRILSWORD:
                if (chest.swordIndex < 4)
                {
                    chest.swordIndex = 4;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILSWORD;
                }
                break;
            case WeaponRecipe.Weapon.MITHRILSCYTHE:
                if (chest.scytheIndex < 4)
                {
                    chest.scytheIndex = 4;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILSCYTHE;
                }
                break;
            case WeaponRecipe.Weapon.MITHRILHAMMER:
                if (chest.hammerIndex < 4)
                {
                    chest.hammerIndex = 4;
                    chest.newWeaponDisplay = WeaponRecipe.Weapon.MITHRILHAMMER;
                }
                break;
            default:
                break;
        }

        chest.swordIndex = swordIndex;
        chest.scytheIndex = scytheIndex;
        chest.hammerIndex = hammerIndex;

        if ((index1 + index2 + index3) != (chest.swordIndex + chest.scytheIndex + chest.hammerIndex))
        {
            chest.DisplayWeapons();
        }

        instance.hasCraftSmth = true;
        WeaponUpInChest.weaponCrafted = currentRecipe;
    }
}
