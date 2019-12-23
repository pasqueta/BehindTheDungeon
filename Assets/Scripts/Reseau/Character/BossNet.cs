using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;


public class BossNet : NetworkBehaviour
{
    //------------VARIABLES------------//
    public static BossNet instance;

    [SerializeField]
    Transform pivotCam;
    Animator anim;
    Rigidbody rb;
    PostProcessingBehaviour postProccessingBehaviour;

    Controller controller;

    [SerializeField]
    List<GameObject> weaponsGameObjects;
    [SerializeField]
    GameObject noWeaponGameObject;


    [SerializeField]
    AudioSource audioHitSound;
    [SerializeField]
    AudioClip[] clipsHitSound;


    [SerializeField]
    AudioSource walkSound;

    [SerializeField]
    AudioSource noMoreWeaponSound;

    [SerializeField]
    AudioSource nappingSound;

    [SerializeField]
    AudioSource sautSound;

    [SerializeField]
    AudioSource mithrilWeaponSound;

    [SerializeField]
    TransformerNet transformer; //p// à voir si il y a pas un autre moyen

    [SerializeField]
    public AudioSource soundEnemyKill;


    public UtilityStruct.STATS stats;

    // [SerializeField]
    GameObject napCanvas;
    [SerializeField]
    PostProcessingProfile baseProfil;
    [SerializeField]
    PostProcessingProfile deadProfil;
    [SerializeField]
    GameObject moneyBagModel;
    [SerializeField]
    GameObject TextLootBones;
    [SerializeField]
    GameObject TextLootGold;

    // Weapons
    List<WeaponRecipe> inventoryWeapons;
    [HideInInspector, SyncVar]
	public int currentWeaponIndex = -1;
    // Life  
    int bossPdvMax = 500;
    int regenHp = 100;
    [SyncVar]
    bool isDead = false;

    // Loot
    List<GameObject> itemsToLoot;       // list of all items to loot in the range of Boss
    GameObject currentLooting = null;   // item currently looted
    float lootTime = 2.0f;              // time to loot an item
    float dtLoot = 0.0f;                   //current time of looting
    int nbMoneyBag = 0;                 // number of bag carried by Boss

    // BasicAttack
    bool canAttacking = true;
    bool isAttacking = false;
    List<EnemyNet> enemiesEverAttacked;
    bool hasAlreadyAttacked;

    // Stomp
    float stompCooldown = 10.0f;
    float dtStompCooldown = 0.0f;
    bool isStomping = false;
    bool enableStomp = true;
    float enemyStunDuration = 1.5f;

    // Dash
    float dashCooldown = 10.0f;
    float dtDashCooldown = 0.0f;
    float dashForce = 50.0f;
    bool isDashing = false;
    bool enableDash = true;

    // Whirlwind
    float whirlwindCooldown = 10.0f;
    float dtWhirlwindCooldown = 0.0f;
    int currentTurnWhirlwind = 0;
    int nbTurnWhirlwind = 3;
    bool isWhirlwind = false;
    bool enableWhirlwind = true;

    //rotation
    Vector3 lastVelocity;

    [HideInInspector]
    public bool isBusy = false;

    [HideInInspector]
    public bool nearOfInteractibleObject = false;

    [SerializeField]
    StompParticle stompParticle;

    #region Getters&Setters


    public bool HaveWeapon
    {
        get
        {
            return currentWeaponIndex != -1 ? true : false;
        }
    }

    public float LootTime
    {
        get
        {
            return lootTime;
        }

        set
        {
            lootTime = value;
        }
    }


    public float DtLoot
    {
        get
        {
            return dtLoot;
        }
    }

    public WeaponRecipe CurrentWeapon
    {
        get
        {
            if (currentWeaponIndex != -1)
            {
                return inventoryWeapons[currentWeaponIndex];
            }
            return null;
        }
    }

    public int HealthPoint
    {
        get
        {
            return stats.health;
        }

        set
        {
            stats.health = value;
        }
    }

    public int BossPdvMax
    {
        get
        {
            return bossPdvMax;
        }
    }


    public int WeaponDurability
    {
        get
        {
            return currentWeaponIndex != -1 ? inventoryWeapons[currentWeaponIndex].durability : 0;
        }
    }


    public int WeaponDurabilityMax
    {
        get
        {
            return currentWeaponIndex != -1 ? inventoryWeapons[currentWeaponIndex].durabilityMax : 0;
        }
    }

    //p// set utile ?
    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    public Transform PivotCam
    {
        get
        {
            return pivotCam;
        }

        set
        {
            pivotCam = value;
        }
    }

    public Controller Controller
    {
        get
        {
            return controller;
        }

        set
        {
            controller = value;
        }
    }

    public int NbMoneyBag
    {
        get
        {
            return nbMoneyBag;
        }

        set
        {
            nbMoneyBag = value;
        }
    }

    public bool IsAttacking { get { return isAttacking; } }

    public bool IsWhirlwind { get { return isWhirlwind; } }

    public int Damage
    {
        get
        {
            if (currentWeaponIndex == -1)
                return stats.attack;
            else
                return stats.attack + inventoryWeapons[currentWeaponIndex].GetAttack();
        }
    }

    public float StompCooldown
    {
        get
        {
            return stompCooldown;
        }

        set
        {
            stompCooldown = value;
        }
    }

    public float DtStompCooldown
    {
        get
        {
            return dtStompCooldown;
        }

        set
        {
            dtStompCooldown = value;
        }
    }

    public float DashCooldown
    {
        get
        {
            return dashCooldown;
        }

        set
        {
            dashCooldown = value;
        }
    }

    public float DtDashCooldown
    {
        get
        {
            return dtDashCooldown;
        }

        set
        {
            dtDashCooldown = value;
        }
    }

    public float WhirlwindCooldown
    {
        get
        {
            return whirlwindCooldown;
        }

        set
        {
            whirlwindCooldown = value;
        }
    }

    public float DtWhirlwindCooldown
    {
        get
        {
            return dtWhirlwindCooldown;
        }

        set
        {
            dtWhirlwindCooldown = value;
        }
    }

    #endregion

    //p// es-tu vraiment utile ?
    bool initSuccess = false;

    RackNet chestWeapon;

    //------------INITIALISATION------------//
    void Start()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        Init();
    }

    void Init()
    {
        InitBossStats();

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        pivotCam = CameraController.camBoss.transform;

        itemsToLoot = new List<GameObject>();
        inventoryWeapons = new List<WeaponRecipe>();
        enemiesEverAttacked = new List<EnemyNet>();

        if (DataManager.instance)
        {
            initSuccess = true;
            controller = DataManager.instance.ControllerBoss;
        }

        if (isLocalPlayer)
        {
            pivotCam.gameObject.SetActive(true);
            pivotCam.GetComponent<CameraController>().screenOption = CameraController.ScreenMode.fullscreen;
            DataManager.instance.ScreenModeBoss = CameraController.ScreenMode.fullscreen;
        }
        else
        {
            pivotCam.gameObject.SetActive(false);
            pivotCam.GetComponent<CameraController>().screenOption = CameraController.ScreenMode.none;
            DataManager.instance.ScreenModeBoss = CameraController.ScreenMode.none;
        }

        pivotCam.GetComponent<CameraController>().Ctrl = controller;
        pivotCam.GetComponent<CameraController>().SetTarget(transform);
        pivotCam.GetComponent<CameraController>().SetCameraPreset();

        if (CanvasManagerNet.instance)
        {
            napCanvas = CanvasManagerNet.instance.GetNapUI();
            napCanvas.SetActive(false);
        }
        else
        {
            initSuccess = false;
        }

        postProccessingBehaviour = pivotCam.GetComponent<CameraController>().GetCameraScene().GetComponent<PostProcessingBehaviour>();
        
        //disable canvas if not the local player
        if (!isLocalPlayer)
        {
            CanvasManagerNet.instance.GetCanvasBoss().enabled = false;
        }
    }

    //------------GLOBAL UPDATE------------//
    void Update()
    {
        if (!isLocalPlayer)
        return;

        if (!initSuccess)
        {
            Init();
        }
        else if (WinLooseManagerNet.instance && WinLooseManagerNet.instance.gameIsAlreadyEnd)
        {
            OnEndGame();
        }

        else if (!isBusy)
        {


            if (!IsDead)
            {
                UpdateStomp();
                UpdateDash();

                if (HaveWeapon)
                {
                    UpdateWhirlwind();
                }

                if (!nearOfInteractibleObject)
                {
                    UpdateBasicAttack();
                }

                if (!isDashing && !isStomping)
                {
                    UpdateDeplacement();
                    UpdateRotation();
                    if (!isWhirlwind && !isAttacking)
                    {
                        UpdateWeapon();
                        UpdateLooting();
                    }
                }
            }
        }
        UpdateLife();
        UpdateCooldown();
    }

    private void LateUpdate()
    {
        pivotCam.position = transform.position;
    }


    void UpdateCooldown()
    {
        if (!enableDash && !isDashing)
        {
            DtDashCooldown += Time.deltaTime;
            if (DtDashCooldown >= DashCooldown)
            {
                isDashing = false;
                enableDash = true;
                DtDashCooldown = 0.0f;
            }
        }

        if (!enableStomp && !isStomping)
        {
            DtStompCooldown += Time.deltaTime;
            if (DtStompCooldown >= StompCooldown)
            {
                enableStomp = true;
                DtStompCooldown = 0.0f;
            }
        }

        if (!enableWhirlwind && !isWhirlwind)
        {
            DtWhirlwindCooldown += Time.deltaTime;

            if (DtWhirlwindCooldown >= WhirlwindCooldown)
            {
                DtWhirlwindCooldown = 0.0f;
                enableWhirlwind = true;
            }
        }
    }


    void UpdateLife()
    {
        if (!isDead)
        {
            if (stats.health <= 0)
            {
                NappingSound();

                isDead = true;
                if(!isServer)
                {
                    CmdSetIsDeadNapBoss(true);
                }

                ResetAutoAttack();
                ResetStomp();
                isWhirlwind = false;
                nbTurnWhirlwind = 0;
                isDashing = false;

                anim.SetBool("stun", true);
                anim.SetBool("stomp", false);
                anim.SetBool("charge", false);
                anim.SetBool("Tourbillon", false);

                napCanvas.SetActive(true);
                postProccessingBehaviour.profile = deadProfil;
                dtLoot = 0.0f;
                rb.velocity = Vector3.zero;

                for (int i = 0; i < NbMoneyBag; i++)
                {
                    Vector3 temp = Random.insideUnitSphere * 4;
                    temp.y = 0;
                    temp += transform.position;
                    Instantiate(moneyBagModel, temp, Quaternion.identity, ChestBossNet.instance.transform);

                    if (isServer)
                    {
                        RpcSpawnObject(moneyBagModel, temp, Quaternion.identity);
                    }
                    else
                    {
                        CmdSpawnObject(moneyBagModel, temp, Quaternion.identity);
                    }
                }

                NbMoneyBag = 0;
            }
        }
        else
        {
            if (HealthPoint >= BossPdvMax)
            {
                HealthPoint = BossPdvMax;
                napCanvas.SetActive(false);
                isDead = false;
                if (!isServer)
                {
                    CmdSetIsDeadNapBoss(false);
                }
                anim.SetBool("stun", false);
                postProccessingBehaviour.profile = baseProfil;
            }
            else
            {
                HealthPoint += (int)(regenHp * Time.deltaTime);
                PeonNet.instance.playedSoundOfDeath = false;           //p// c'est quoi ?
            }
        }
    }

    void UpdateDeplacement()
    {
        float verticalVelocity = rb.velocity.y;
        Vector3 velocity = Vector3.zero;

        velocity = pivotCam.forward * Input.GetAxisRaw(controller + "_LYaxis");
        velocity += pivotCam.right * Input.GetAxisRaw(controller + "_LXaxis");

        velocity.Normalize();

        velocity *= stats.movementSpeed;

        velocity.y = verticalVelocity;
        rb.velocity = velocity;
        
        anim.SetFloat("velocity", velocity.magnitude);
    }

    void UpdateRotation()
    {
        if (!isWhirlwind)
        {
            if (Input.GetAxisRaw(controller + "_LXaxis") != 0.0f || Input.GetAxisRaw(controller + "_LYaxis") != 0.0f)
            {
                lastVelocity = rb.velocity;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lastVelocity), 0.3f);
            }
        }
    }

    void UpdateBasicAttack()
    {
        if (canAttacking &&
            ((Controller == Controller.K1 && Input.GetMouseButtonDown(0)) ||
            ((Controller == Controller.J1 || Controller == Controller.J2) && Input.GetButtonDown(Controller + "_A"))))
        {
            dtLoot = 0.0f;

            if (!isServer)
            {
                CmdAttack();
            }

            canAttacking = false;

            if (inventoryWeapons.Count <= 0 || currentWeaponIndex == -1)
            {
                anim.SetBool("TwoHands", false);
            }
            else if (inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODSWORD
                || inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONSWORD
                || inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDSWORD
                || inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD)
            {
                anim.SetBool("TwoHands", false);
            }
            else
            {
                anim.SetBool("TwoHands", true);
            }
            anim.SetTrigger("attack");
        }
    }

    void UpdateDash()
    {
        if (!isDashing && enableDash)
        {
            if (!isAttacking && !isWhirlwind && !isStomping && 
                (controller == Controller.K1 && Input.GetKeyDown(KeyCode.Alpha2)) ||
                ((controller == Controller.J1 || controller == Controller.J2) && Input.GetButtonDown(controller + "_X")))
            {
                dtLoot = 0.0f;
                anim.SetBool("charge", true);
                rb.velocity += transform.forward * dashForce;
                isDashing = true;
                enableDash = false;
            }
        }
        else if (isDashing)
        {
            if (rb.velocity.x <= 1.0f && rb.velocity.z <= 1.0f)
            {
                anim.SetBool("charge", false);
                isDashing = false;
            }
        }
        // else if (!enableDash)
        // {
        //     DtDashCooldown += Time.deltaTime;
        //     if (DtDashCooldown >= DashCooldown)
        //     {
        //         isDashing = false;
        //         enableDash = true;
        //         DtDashCooldown = 0.0f;
        //     }
        // }
    }

    void UpdateStomp()
    {
        if (enableStomp && !isDashing)
        {
            if (!isAttacking && !isDashing && !isWhirlwind &&
                    (controller == Controller.K1 && Input.GetKeyDown(KeyCode.Alpha1)) ||
                   ((controller == Controller.J1 || controller == Controller.J2) && Input.GetButtonDown(controller + "_B")))
            {
                dtLoot = 0.0f;
                rb.velocity = Vector3.zero;
                isStomping = true;
                anim.SetTrigger("stomp");
                enableStomp = false;
            }
        }
        // else if (!enableStomp)
        // {
        //     DtStompCooldown += Time.deltaTime;
        //     if (DtStompCooldown >= StompCooldown)
        //     {
        //         enableStomp = true;
        //         DtStompCooldown = 0.0f;
        //     }
        // }
    }

    void UpdateWhirlwind()
    {
        if (enableWhirlwind && !isWhirlwind)
        {
            if (!isAttacking && !isDashing && !isStomping && 
                (controller == Controller.K1 && Input.GetKeyDown(KeyCode.Alpha3)) ||
               ((controller == Controller.J1 || controller == Controller.J2) && Input.GetButtonDown(controller + "_Y")))
            {
                dtLoot = 0.0f;

                if (inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODHAMMER ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONHAMMER ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDHAMMER ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                {
                    nbTurnWhirlwind = 1;
                }
                else if (inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODSWORD ||
                         inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONSWORD ||
                         inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDSWORD ||
                         inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD)
                {
                    nbTurnWhirlwind = 3 + stats.whirlwind;
                }
                else
                {
                    nbTurnWhirlwind = 3;
                }


                isWhirlwind = true;
                weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = true;
                anim.SetBool("Tourbillon", true);
                enableWhirlwind = false;
            }
        }
        else if (isWhirlwind)
        {
            if (currentTurnWhirlwind >= nbTurnWhirlwind)
            {
                currentTurnWhirlwind = 0;
                isWhirlwind = false;
                anim.SetBool("Tourbillon", false);
            }
        }
        // else if(!enableWhirlwind)
        // {
		// 	DtWhirlwindCooldown += Time.deltaTime;
        // 
        //     if (DtWhirlwindCooldown >= WhirlwindCooldown)
        //     {
        //         DtWhirlwindCooldown = 0.0f;
        //         enableWhirlwind = true;
        //     }
        // }
    }

    void UpdateWeapon()
    {
        if (inventoryWeapons.Count > 0)
        {
            if (Controller == Controller.K1)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (currentWeaponIndex != -1)
                    {
                        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
                    }
                    else
                    {
                        noWeaponGameObject.SetActive(false);
                    }
                    currentWeaponIndex++;

                    if (inventoryWeapons.Count > 0)
                    {
                        if (currentWeaponIndex == inventoryWeapons.Count)
                        {
                            currentWeaponIndex = 0;
                        }
                    }

                    if (currentWeaponIndex != -1)
                    {
                        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);

                        if (inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                            inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                            inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                        {
                            MithrilWeaponSound();
                        }
                    }

                    if (isServer)
                    {
                        RpcEnableWeapon();
                    }
                    else
                    {
                        CmdEnableWeapon();
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown(controller + "_RB"))
                {
                    if (currentWeaponIndex != -1)
                        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
                    else
                        noWeaponGameObject.SetActive(false);
                    currentWeaponIndex++;
                    if (inventoryWeapons.Count > 0)
                    {
                        if (currentWeaponIndex == inventoryWeapons.Count)
                            currentWeaponIndex = 0;
                    }
                    weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);

                    if(isServer)
                    {
                        RpcEnableWeapon();
                    }
                    else
                    {
                        CmdEnableWeapon();
                    }

                    if (inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                        inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                        inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                    {
                        MithrilWeaponSound();
                    }

                }
                if (Input.GetButtonDown(controller + "_LB"))
                {
                    if (currentWeaponIndex != -1)
                        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
                    else
                        noWeaponGameObject.SetActive(false);
                    currentWeaponIndex--;
                    if (currentWeaponIndex < 0)
                        currentWeaponIndex = inventoryWeapons.Count - 1;
                    weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);

                    if (isServer)
                    {
                        RpcSwitchWeaponDown();
                    }
                    else
                    {
                        CmdSwitchWeaponDown();
                    }

                    if (inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                        inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                        inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                    {
                        MithrilWeaponSound();
                    }
                }
            }
        }
        else
        {
            currentWeaponIndex = -1;
        }
    }

    void UpdateLooting()
    {
        if (currentLooting)
        {
            dtLoot += Time.deltaTime;
            if (dtLoot >= lootTime)
            {
                switch (currentLooting.GetComponent<Lootable>().LootType)
                {
                    case LOOT_TYPE.BONES:
                        if(isServer)
                        {
                            RpcGiveBonesToPeon();
                        }
                        else
                        {
                            CmdGiveBonesToPeon();
                        }
                        Instantiate(TextLootBones, CanvasManagerNet.instance.GetCanvasBoss().transform);
                        break;
                    case LOOT_TYPE.GOLD:
                        NbMoneyBag++;

                        if (isServer)
                        {
                            //RpcLootBag();
                        }
                        else
                        {
                            CmdLootBag();
                        }

                        Instantiate(TextLootGold, CanvasManagerNet.instance.GetCanvasBoss().transform);
                        break;
                }
                if(isServer)
                {
                    NetworkServer.Destroy(currentLooting);
                }
                else
                {
                    CmdDestroy(currentLooting);
                }
                RemoveItemInLootList(currentLooting);
            }
        }
    }
    
    [Command]
    void CmdGiveBonesToPeon()
    {
        FindObjectOfType<TransformerNet>().acquiredMinerals.bones += 5;
    }

    [ClientRpc]
    void RpcGiveBonesToPeon()
    {
        FindObjectOfType<TransformerNet>().acquiredMinerals.bones += 5;
    }

    [Command]
    void CmdLootBag()
    {
        NbMoneyBag++;
    }

    [ClientRpc]
    void RpcLootBag()
    {
        NbMoneyBag++;
    }

    //---------------SOUNDS---------------//

    #region Son

    public void WalkSound()
    {
        if (walkSound)
        {
            walkSound.Play();
        }
    }

    public void NappingSound()
    {
        if (nappingSound)
        {
            nappingSound.Play();
        }
    }

    public void MithrilWeaponSound()
    {
        if (mithrilWeaponSound)
        {
            mithrilWeaponSound.Play();
        }
    }

    public void KillSound()
    {
        int temp = Random.Range(1, 25);

        if (temp == 2)
        {
            if (soundEnemyKill)
            {
                soundEnemyKill.Play();
            }
        }
    }

    public void HitSound()
    {
        if (!HaveWeapon || CurrentWeapon.weapon == WeaponRecipe.Weapon.WOODHAMMER || CurrentWeapon.weapon == WeaponRecipe.Weapon.IRONHAMMER
            || CurrentWeapon.weapon == WeaponRecipe.Weapon.DIAMONDHAMMER || CurrentWeapon.weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
        {
            if (clipsHitSound.Length > 0 && !audioHitSound.isPlaying)
            {
                audioHitSound.PlayOneShot(clipsHitSound[Random.Range(0, clipsHitSound.Length)]);
            }
        }
    }

    #endregion

    public CameraController GetCameraController()
    {
        return pivotCam.GetComponent<CameraController>();
    }
    
    void InitBossStats()
    {
        bossPdvMax = 500 + 50 * PlayerPrefs.GetInt("bossTalentHealth");
        stats.health = bossPdvMax;
        stats.attack = 10 + 2 * PlayerPrefs.GetInt("bossTalentAttack");
        stats.attackSpeed = 1.0f;
        stats.dash = PlayerPrefs.GetInt("bossTalentDash");
        stats.whirlwind = PlayerPrefs.GetInt("bossTalentWhirlwind");
        stats.movementSpeed = 10 + 2 * PlayerPrefs.GetInt("bossTalentMovementSpeed");
    }

    void StatsFromWeapons()
    {
        
    }

    void Naping()
    {
        if (HealthPoint >= BossPdvMax)
        {
            HealthPoint = BossPdvMax;
            napCanvas.SetActive(false);
            IsDead = false;
            postProccessingBehaviour.profile = baseProfil;
        }
        else
        {
            HealthPoint += (int)(75 * Time.deltaTime);
            PeonNet.instance.playedSoundOfDeath = false;
        }
    }

    void EndGame()
    {
        if (ChestBossNet.instance)
        {
            if (ChestBossNet.instance.NbMoneyBag <= 0 && NbMoneyBag <= 0)
            {
                if (SpawnerManagerNet.instance.EntityHaveSteal())
                {
                    return;
                }

                //p// Tout les sacs sont en enfants du coffre
                if (ChestBossNet.instance.transform.childCount > 0)
                {
                    return;
                }
                
                if (WinLooseManagerNet.instance)
                {
                    WinLooseManagerNet.instance.EndGame(false, DataManager.instance.myTab[(int)DataManager.instance.language + 1, 82]);
                }
            }
        }
    }

    void OnEndGame()
    {
        ResetAutoAttack();
        ResetStomp();
        isWhirlwind = false;
        anim.SetBool("Tourbillon", false);
        anim.SetBool("charge", false);
        nbTurnWhirlwind = 0;
        isDashing = false;

        //anim.Play("Idle_Boss");
    }


    public void DestroyEquipedWeapon()
    {
        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        noWeaponGameObject.SetActive(true);
        inventoryWeapons.RemoveAt(currentWeaponIndex);
        currentWeaponIndex = -1;

        if (inventoryWeapons.Count == 0)
        {
            int temp = Random.Range(1, 5);
            if (temp == 4)
            {
                if (noMoreWeaponSound)
                {
                    noMoreWeaponSound.Play();
                }
            }
        }
    }

    public void AddWeapon(WeaponRecipe _weaponRecipe)
    {
        inventoryWeapons.Add(new WeaponRecipe(_weaponRecipe));
    }

    public void HideWeapon(bool _b)
    {
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(!_b);
        }
    }

    public void SetDurability()
    {
        if (currentWeaponIndex != -1)
        {
            inventoryWeapons[currentWeaponIndex].durability--;
            if (inventoryWeapons[currentWeaponIndex].durability <= 0)
            {
                if (isServer)
                {
                    RpcDestroyEquipedWeapon(currentWeaponIndex);
                }
                else
                {
                    CmdDestroyEquipedWeapon(currentWeaponIndex);
                }
                DestroyEquipedWeapon();

                if(inventoryWeapons.Count == 0)
                {
                    currentWeaponIndex = -1;
                }
            }
        }
    }

    [ClientRpc]
    void RpcDestroyEquipedWeapon(int currentWeapon)
    {
        weaponsGameObjects[(int)inventoryWeapons[currentWeapon].weapon].SetActive(false);
        noWeaponGameObject.SetActive(true);
        inventoryWeapons.RemoveAt(currentWeapon);
        currentWeaponIndex = -1;
    }
    [Command]
    void CmdDestroyEquipedWeapon(int currentWeapon)
    {
        weaponsGameObjects[(int)inventoryWeapons[currentWeapon].weapon].SetActive(false);
        noWeaponGameObject.SetActive(true);
        inventoryWeapons.RemoveAt(currentWeapon);
        currentWeaponIndex = -1;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (isAttacking && collider.GetComponent<EnemyNet>().canBeAttacking)
            {
                HitSound();
                collider.GetComponent<EnemyNet>().HealthPoint -= Damage;
                collider.GetComponent<EnemyNet>().canBeAttacking = false;
                enemiesEverAttacked.Add(collider.GetComponent<EnemyNet>());

                if (!isServer)
                {
                    if (currentWeaponIndex == -1)
                    {
                        CmdDamageEnemy(collider.gameObject, 0);
                    }
                    else
                    {
                        CmdDamageEnemy(collider.gameObject, inventoryWeapons[currentWeaponIndex].durability);
                    }
                }

                if (hasAlreadyAttacked == false)
                {
                    SetDurability();
                    hasAlreadyAttacked = true;
                }

                if (HaveWeapon && (
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODSWORD ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONSWORD ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDSWORD ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD))
                {
                    ResetAutoAttack();
                    anim.SetTrigger("stopAttack");
                    if (isServer)
                    {
                        RpcSwordHit();
                    }
                    else
                    {
                        CmdSwordHit();
                    }
                }

                if (HaveWeapon && (
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODHAMMER ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONHAMMER ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDHAMMER ||
                    inventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER))
                {
                    collider.GetComponent<EnemyNet>().SetBump(3.0f);

                    if (isServer)
                    {
                        RpcHammerHit(collider.gameObject);
                    }
                    else
                    {
                        CmdHammerHit(collider.gameObject);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && isDashing)
        {
            collision.collider.GetComponent<EnemyNet>().SetKnockDown(2.0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;
    }

    void StopEmitWeapon()
    {
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = false;
        }
    }

    #region UseInAnimator

    public void StompEnemies()
    {
        /*foreach (GameObject item in SpawnerManagerNet.instance.GetAllEntities())
        {
            item.GetComponent<EnemyNet>().SetStun(enemyStunDuration);
        }*/

        //if (stompParticle)
        //{
        //    stompParticle.gameObject.SetActive(true);
        //}

        //PeonNet.instance.SetStun();

        if (isServer)
        {
            RpcFreezeEnemyByJumping();
        }
        else
        {
            CmdFreezeEnemyByJumping();
        }
    }

    public void ResetStomp()
    {
        isStomping = false;
    }

    public void EndOfWhirlwindTurn()
    {
        currentTurnWhirlwind++;
        if (currentTurnWhirlwind >= nbTurnWhirlwind)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = false;
        }
    }

    public void ResetAutoAttack()
    {
        isAttacking = false;
        canAttacking = true;

        foreach (EnemyNet item in enemiesEverAttacked)
        {
            item.canBeAttacking = true;
        }

        enemiesEverAttacked.Clear();
        hasAlreadyAttacked = false;

        //if (currentWeaponIndex != -1)
        //{
        //    weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = false;
        //}
    }

    void WhrilwindTick()
    {
        if (currentWeaponIndex == -1)
            return;

        Collider[] hitColliders;
        switch (inventoryWeapons[currentWeaponIndex].weapon)
        {
            case WeaponRecipe.Weapon.WOODSWORD:
            case WeaponRecipe.Weapon.IRONSWORD:
            case WeaponRecipe.Weapon.DIAMONDSWORD:
            case WeaponRecipe.Weapon.MITHRILSWORD:
                hitColliders = Physics.OverlapSphere(transform.position, 5);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].GetComponent<EnemyNet>())
                    {
                        hitColliders[i].GetComponent<EnemyNet>().HealthPoint -= Damage;
                    }
                }
                break;

            case WeaponRecipe.Weapon.WOODSCYTHE:
            case WeaponRecipe.Weapon.IRONSCYTHE:
            case WeaponRecipe.Weapon.DIAMONDSCYTHE:
            case WeaponRecipe.Weapon.MITHRILSCYTHE:
                hitColliders = Physics.OverlapSphere(transform.position, 5 + stats.whirlwind);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].GetComponent<EnemyNet>())
                    {
                        hitColliders[i].GetComponent<EnemyNet>().HealthPoint -= Damage;
                    }
                }
                break;

            case WeaponRecipe.Weapon.WOODHAMMER:
            case WeaponRecipe.Weapon.IRONHAMMER:
            case WeaponRecipe.Weapon.DIAMONDHAMMER:
            case WeaponRecipe.Weapon.MITHRILHAMMER:
                hitColliders = Physics.OverlapSphere(transform.position, 5);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].GetComponent<EnemyNet>())
                    {
                        hitColliders[i].GetComponent<EnemyNet>().SetBump(2.0f);
                        hitColliders[i].GetComponent<EnemyNet>().HealthPoint -= Damage;
                        Rigidbody rb = hitColliders[i].GetComponent<Rigidbody>();
                        if (rb != null)
                            rb.AddExplosionForce(20 * stats.whirlwind, transform.position, 5, 3.0f);
                    }
                }
                break;
            default:
                break;
        }
    }

    void StartAttackDamage()
    {
        isAttacking = true;
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = true;
        }
    }

    #endregion

    #region LootAccessors
    public void AddItemInLootList(GameObject go)
    {
        if (!itemsToLoot.Contains(go))
        {
            itemsToLoot.Add(go);
            if (!currentLooting)
            {
                currentLooting = itemsToLoot[0];
            }
        }
    }

    public void RemoveItemInLootList(GameObject go)
    {
        if (itemsToLoot.Contains(go))
        {
            if (itemsToLoot.IndexOf(go) == 0)
            {
                itemsToLoot.Remove(go);

                dtLoot = 0.0f;

                if (itemsToLoot.Count > 0)
                {
                    currentLooting = itemsToLoot[0];
                }
                else
                {
                    currentLooting = null;
                }
            }
            else
            {
                itemsToLoot.Remove(go);
            }
        }
    }
    #endregion

    [ClientRpc]
    public void RpcDealDamage(int bossOrChest,int dmg)
    {
        if(bossOrChest == 0)
        {
            BossNet.instance.HealthPoint -= dmg;
        }
        else if(bossOrChest == 1)
        {
            ChestBossNet.instance.Pdv -= dmg;
        }
    }

    [Command]
    void CmdSetIsDeadNapBoss(bool state)
    {
        isDead = state;
    }

    [Command]
    void CmdSwordHit()
    {
        ResetAutoAttack();
        anim.SetTrigger("stopAttack");
    }

    [ClientRpc]
    void RpcSwordHit()
    {
        ResetAutoAttack();
        anim.SetTrigger("stopAttack");
    }

    [Command]
    void CmdHammerHit(GameObject go)
    {
        go.GetComponent<EnemyNet>().SetBump(3.0f);
    }

    [ClientRpc]
    void RpcHammerHit(GameObject go)
    {
        go.GetComponent<EnemyNet>().SetBump(3.0f);
    }

    [Command]
    void CmdDamageEnemy(GameObject obj, int durability)
    {
        obj.GetComponent<EnemyNet>().HealthPoint -= Damage;
        obj.GetComponent<EnemyNet>().canBeAttacking = false;
        
        //if (currentWeaponIndex != -1)
        //{
        //    inventoryWeapons[currentWeaponIndex].durability = durability;

        //    inventoryWeapons[currentWeaponIndex].durability--;
        //    if (inventoryWeapons[currentWeaponIndex].durability <= 0)
        //    {
        //        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        //        noWeaponGameObject.SetActive(true);
        //        inventoryWeapons.RemoveAt(currentWeaponIndex);
        //        currentWeaponIndex = -1;
        //    }
        //}
    }

    [Command]
    void CmdWinLose()
    {
        WinLooseManagerNet.instance.EndGame(false, "The Ruler have been defeated");
    }

    [ClientRpc]
    void RpcSpawnObject(GameObject obj, Vector3 pos, Quaternion quat)
    {
        GameObject go = Instantiate(obj, pos, quat);
        NetworkServer.Spawn(go);
    }
    [Command]
    void CmdSpawnObject(GameObject obj, Vector3 pos, Quaternion quat)
    {
        GameObject go = Instantiate(obj, pos, quat);
        NetworkServer.Spawn(go);
    }
    
    [ClientRpc]
    void RpcEnableWeapon()
    {
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        }
        else
        {
            noWeaponGameObject.SetActive(false);
        }

        if (inventoryWeapons.Count > 0)
        {
            if (currentWeaponIndex == inventoryWeapons.Count)
            {
                currentWeaponIndex = 0;
            }
        }
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);
        }

    }
    [Command]
    void CmdEnableWeapon()
    {
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        }
        else
        {
            noWeaponGameObject.SetActive(false);
        }
        currentWeaponIndex++;

        if (inventoryWeapons.Count > 0)
        {
            if (currentWeaponIndex == inventoryWeapons.Count)
            {
                currentWeaponIndex = 0;
            }
        }
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);
        }
    }

    [Command]
    void CmdSwitchWeaponDown()
    {
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        }
        else
        {
            noWeaponGameObject.SetActive(false);
        }
        currentWeaponIndex--;

        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = inventoryWeapons.Count - 1;
        }
        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);
    }
    [ClientRpc]
    void RpcSwitchWeaponDown()
    {
        if (currentWeaponIndex != -1)
        {
            weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        }
        else
        {
            noWeaponGameObject.SetActive(false);
        }
        currentWeaponIndex--;

        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = inventoryWeapons.Count - 1;
        }
        weaponsGameObjects[(int)inventoryWeapons[currentWeaponIndex].weapon].SetActive(true);
    }
    
    [Command]
    void CmdFreezeEnemyByJumping()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (stompParticle)
        {
            stompParticle.gameObject.SetActive(true);
        }

        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<EnemyNet>().SetStun(enemyStunDuration);
        }
        PeonNet.instance.SetStun();
    }
    [ClientRpc]
    void RpcFreezeEnemyByJumping()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (stompParticle)
        {
            stompParticle.gameObject.SetActive(true);
        }

        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<EnemyNet>().SetStun(enemyStunDuration);
        }
        PeonNet.instance.SetStun();
    }

    [Command]
    void CmdAttack()
    {
        canAttacking = false;
        anim.SetTrigger("attack");
    }

    //------------------EXTERNE-----------------//
    [Command]
    public void CmdGetWeapon()
    {
        RackNet rack = FindObjectOfType<RackNet>();

        foreach (WeaponRecipe weapon in rack.weaponList)
        {
            BossNet.instance.AddWeapon(weapon);
        }

        rack.swordIndex = 0;
        rack.scytheIndex = 0;
        rack.hammerIndex = 0;
        foreach (Transform tf in GetComponentsInChildren<Transform>())
        {
            if (tf.tag == "Sword" || tf.tag == "Scythe" || tf.tag == "Hammer")
            {
                tf.gameObject.SetActive(false);

            }
        }
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(false);
        PeonNet.instance.hasCraftSmth = false;
        rack.weaponList.Clear();
    }
    [ClientRpc]
    public void RpcGetWeapon()
    {
        RackNet rack = FindObjectOfType<RackNet>();

        foreach (WeaponRecipe weapon in rack.weaponList)
        {
            BossNet.instance.AddWeapon(weapon);
        }

        rack.swordIndex = 0;
        rack.scytheIndex = 0;
        rack.hammerIndex = 0;
        foreach (Transform tf in GetComponentsInChildren<Transform>())
        {
            if (tf.tag == "Sword" || tf.tag == "Scythe" || tf.tag == "Hammer")
            {
                tf.gameObject.SetActive(false);

            }
        }
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(false);
        PeonNet.instance.hasCraftSmth = false;
        rack.weaponList.Clear();
    }

    [ClientRpc]
    public void RpcSetTextBetweenToWaves(bool state, string str)
    {
        CanvasManagerNet.instance.GetTextTimeBetweenWaves().text = str;
        CanvasManagerNet.instance.GetTextTimeBetweenWaves().enabled = state;
    }

    [ClientRpc]
    public void RpcSetTextRemainingWave(bool state, string str)
    {
        CanvasManagerNet.instance.GetTextRemainingWave().text = str;
        CanvasManagerNet.instance.GetTextRemainingWave().enabled = state;
    }

    [Command]
    public void CmdDestroy(GameObject go)
    {
        NetworkServer.Destroy(go);
    }

    [Command]
    public void CmdMoveBridge(GameObject bridge, float pivotz, float lastRot, bool enableJump, float rotFactor, float jumpMarge)
    {
        // ROTATION PONT
        if (lastRot > 310.0f && pivotz < 50.0f)
        {
            bridge.transform.Rotate(Vector3.up, (pivotz - (lastRot - 360.0f)) / rotFactor);
        }
        else if (lastRot < 50.0f && pivotz > 310.0f)
        {
            bridge.transform.Rotate(Vector3.up, ((360.0f - pivotz) - lastRot) / rotFactor);
        }
        else
        {
            bridge.transform.Rotate(Vector3.up, (pivotz - lastRot) / rotFactor);
        }

        // CAN JUMP OR NOT
        if ((bridge.transform.rotation.eulerAngles.y >= (90 - jumpMarge) && bridge.transform.rotation.eulerAngles.y <= (90 + jumpMarge)) || (bridge.transform.rotation.eulerAngles.y >= (270 - jumpMarge) && bridge.transform.rotation.eulerAngles.y <= (270 + jumpMarge)))
        {
            bridge.GetComponent<BridgeNet>().enableJump = true;
        }
        else
        {
            bridge.GetComponent<BridgeNet>().enableJump = false;
        }

        BossNet.instance.GetComponent<Animator>().speed = lastRot != pivotz ? 1 : 0;
        
        lastRot = pivotz;
    }

    [ClientRpc]
    public void RpcMoveBridge(GameObject bridge, float pivotz, float lastRot, bool enableJump, float rotFactor, float jumpMarge)
    {
        // ROTATION PONT
        if (lastRot > 310.0f && pivotz < 50.0f)
        {
            bridge.transform.Rotate(Vector3.up, (pivotz - (lastRot - 360.0f)) / rotFactor);
        }
        else if (lastRot < 50.0f && pivotz > 310.0f)
        {
            bridge.transform.Rotate(Vector3.up, ((360.0f - pivotz) - lastRot) / rotFactor);
        }
        else
        {
            bridge.transform.Rotate(Vector3.up, (pivotz - lastRot) / rotFactor);
        }

        // CAN JUMP OR NOT
        if ((bridge.transform.rotation.eulerAngles.y >= (90 - jumpMarge) && bridge.transform.rotation.eulerAngles.y <= (90 + jumpMarge)) || (bridge.transform.rotation.eulerAngles.y >= (270 - jumpMarge) && bridge.transform.rotation.eulerAngles.y <= (270 + jumpMarge)))
        {
            enableJump = true;
        }
        else
        {
            enableJump = false;
        }

        BossNet.instance.GetComponent<Animator>().speed = lastRot != pivotz ? 1 : 0;
        
        lastRot = pivotz;
    }
}