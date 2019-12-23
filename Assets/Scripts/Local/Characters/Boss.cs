using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;


public class Boss : MonoBehaviour
{
    //------------VARIABLES------------//
    public static Boss instance;

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
    Transformer transformer;

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
    [HideInInspector]
    public int currentWeaponIndex = -1;

    // Life  
    int bossPdvMax = 500;
    int regenHp = 100;
    bool isDead = false;

    // Loot
    List<GameObject> itemsToLoot;       // list of all items to loot in the range of Boss
    GameObject currentLooting = null;   // item currently looted
    float lootTime = 2.0f;              // time to loot an item
    float dtLoot = 0.0f;                //current time of looting
    int nbMoneyBag = 0;                 // number of bag carried by Boss

    // BasicAttack
    bool canAttacking = true;
    bool isAttacking = false;
    List<Enemy> enemiesEverAttacked;

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
                return InventoryWeapons[currentWeaponIndex];
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
            if (!isDead)
            {
                if (value < 0)
                    stats.health = 0;
                else
                    stats.health = value;
            }
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
            return currentWeaponIndex != -1 ? InventoryWeapons[currentWeaponIndex].durability : 0;
        }
    }


    public int WeaponDurabilityMax
    {
        get
        {
            return currentWeaponIndex != -1 ? InventoryWeapons[currentWeaponIndex].durabilityMax : 0;
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
                return stats.attack + InventoryWeapons[currentWeaponIndex].GetAttack();
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

    public GameObject NoWeaponGameObject
    {
        get
        {
            return noWeaponGameObject;
        }

        set
        {
            noWeaponGameObject = value;
        }
    }

    public List<WeaponRecipe> InventoryWeapons
    {
        get
        {
            return inventoryWeapons;
        }

        set
        {
            inventoryWeapons = value;
        }
    }

    public List<GameObject> WeaponsGameObjects
    {
        get
        {
            return weaponsGameObjects;
        }

        set
        {
            weaponsGameObjects = value;
        }
    }

    #endregion

    //p// es-tu vraiment utile ?
    bool initSuccess = false;

    bool hasAlreadyAttacked;

    private void Start()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        Init();
        dashCooldown -= 2*stats.dash;
    }

    void Init()
    {
        InitBossStats();

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        itemsToLoot = new List<GameObject>();
        InventoryWeapons = new List<WeaponRecipe>();
        enemiesEverAttacked = new List<Enemy>();

        if (DataManager.instance)
        {
            initSuccess = true;
            controller = DataManager.instance.ControllerBoss;
            pivotCam.GetComponent<CameraController>().screenOption = DataManager.instance.ScreenModeBoss;
        }
        else
        {
            initSuccess = false;
        }

        pivotCam.GetComponent<CameraController>().Ctrl = controller;
        pivotCam.GetComponent<CameraController>().SetTarget(transform);
        pivotCam.GetComponent<CameraController>().SetCameraPreset();

        if (CanvasManager.instance)
        {
            napCanvas = CanvasManager.instance.GetNapUI();
        }
        else
        {
            initSuccess = false;
        }

        postProccessingBehaviour = pivotCam.GetComponent<CameraController>().GetCameraScene().GetComponent<PostProcessingBehaviour>();
    }


    void Update()
    {
        if (!initSuccess)
        {
            Init();
        }
        else if (WinLooseManager.instance && WinLooseManager.instance.gameIsAlreadyEnd)
        {
            OnEndGame();
        }
        else if (!isBusy)
        {
            if (!isDead)
            {
                UpdateStomp();
                UpdateDash();

                UpdateWhirlwind();

                if (!nearOfInteractibleObject)
                    UpdateBasicAttack();

                if (!isDashing && !isStomping)
                {
                    UpdateDeplacement();
                    UpdateRotation();
                    if (!isWhirlwind && !isAttacking)
                    {
                        UpdateWeapon();
                    }
                }
                UpdateLooting();
            }
        }

        UpdateLife();
        UpdateCooldown();
    }

    private void LateUpdate()
    {
        // Dans le LateUpdate pour éviter les saccadement.
        PivotCam.position = transform.position;
    }

    void InitBossStats()
    {
        bossPdvMax = 500 + 50 * PlayerPrefs.GetInt("bossTalentHealth");
        stats.health = bossPdvMax;
        stats.attack = 10 + 2 * PlayerPrefs.GetInt("bossTalentAttack");
        stats.attackSpeed = 1.0f;
        //equippedWeapon = WeaponRecipe.Weapon.NONE;
        stats.dash = PlayerPrefs.GetInt("bossTalentDash");
        stats.whirlwind = PlayerPrefs.GetInt("bossTalentWhirlwind");
        stats.movementSpeed = 10 + 2 * PlayerPrefs.GetInt("bossTalentMovementSpeed");
    }

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
        }//coming soon le second son avec les armes QUI TRANCHENT !
        //else
        //{
        //    if (true)
        //    {

        //    }
        //}
    }

    #endregion


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

    #region Updates

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
                    Instantiate(moneyBagModel, temp, Quaternion.identity, ChestBoss.instance.transform);
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
                anim.SetBool("stun", false);
                postProccessingBehaviour.profile = baseProfil;
            }
            else
            {
                Debug.Log(HealthPoint);
                stats.health += (int)(regenHp * Time.deltaTime);
                Peon.instance.playedSoundOfDeath = false;
            }
        }
    }

    void UpdateDeplacement()
    {
        float verticalVelocity = rb.velocity.y;
        Vector3 velocity = Vector3.zero;

        velocity = PivotCam.forward * Input.GetAxisRaw(controller + "_LYaxis");
        velocity += PivotCam.right * Input.GetAxisRaw(controller + "_LXaxis");
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
        if (canAttacking && !isWhirlwind && !isDashing && !isStomping &&
            ((Controller == Controller.K1 && Input.GetMouseButtonDown(0)) ||
            ((Controller == Controller.J1 || Controller == Controller.J2) && Input.GetButtonDown(Controller + "_A"))))
        {
            dtLoot = 0.0f;

            canAttacking = false;
            //isAttacking = true;
            if (currentWeaponIndex == -1)
            {
                anim.SetBool("TwoHands", false);
            }
            else if (InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODSWORD
                    || InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONSWORD
                    || InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDSWORD
                    || InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD)
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
    }

    void UpdateStomp()
    {
        if (!isStomping)
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
        }
    }

    void UpdateWhirlwind()
    {
        if (HaveWeapon && enableWhirlwind && !isWhirlwind)
        {
            if (!isAttacking && !isDashing && !isStomping &&
                (controller == Controller.K1 && Input.GetKeyDown(KeyCode.Alpha3)) ||
                ((controller == Controller.J1 || controller == Controller.J2) && Input.GetButtonDown(controller + "_Y")))
            {
                dtLoot = 0.0f;

                if (InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODHAMMER ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONHAMMER ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDHAMMER ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                {
                    nbTurnWhirlwind = 1;
                }
                else if (InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODSWORD ||
                            InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONSWORD ||
                            InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDSWORD ||
                            InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD)
                {
                    nbTurnWhirlwind = 3 + stats.whirlwind;
                }
                else
                {
                    nbTurnWhirlwind = 3;
                }

                isWhirlwind = true;
                WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = true;
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
    }

    void UpdateWeapon()
    {
        if (InventoryWeapons.Count > 0)
        {
            if (Controller == Controller.K1)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (currentWeaponIndex != -1)
                        WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
                    else
                        NoWeaponGameObject.SetActive(false);
                    currentWeaponIndex++;
                    if (currentWeaponIndex == InventoryWeapons.Count)
                        currentWeaponIndex = 0;
                    WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(true);

                    if (InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                        InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                        InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                    {
                        MithrilWeaponSound();
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown(controller + "_RB"))
                {
                    if (currentWeaponIndex != -1)
                        WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
                    else
                        NoWeaponGameObject.SetActive(false);
                    currentWeaponIndex++;
                    if (currentWeaponIndex == InventoryWeapons.Count)
                        currentWeaponIndex = 0;
                    WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(true);

                    if (InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                        InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                        InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
                    {
                        MithrilWeaponSound();
                    }

                }
                if (Input.GetButtonDown(controller + "_LB"))
                {
                    if (currentWeaponIndex != -1)
                        WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
                    else
                        NoWeaponGameObject.SetActive(false);
                    currentWeaponIndex--;
                    if (currentWeaponIndex < 0)
                        currentWeaponIndex = InventoryWeapons.Count - 1;
                    WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(true);

                    if (InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD ||
                        InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSCYTHE ||
                        InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER)
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
                        transformer.acquiredMinerals.bones += 5;
                        Instantiate(TextLootBones, CanvasManager.instance.GetCanvasBoss().transform);
                        break;
                    case LOOT_TYPE.GOLD:
                        NbMoneyBag++;
                        Instantiate(TextLootGold, CanvasManager.instance.GetCanvasBoss().transform);
                        break;
                }
                Destroy(currentLooting);
                RemoveItemInLootList(currentLooting);
            }
        }
    }

    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (isAttacking && collider.GetComponent<Enemy>().canBeAttacking)
            {
                HitSound();
                collider.GetComponent<Enemy>().HealthPoint -= Damage;
                collider.GetComponent<Enemy>().canBeAttacking = false;
                enemiesEverAttacked.Add(collider.GetComponent<Enemy>());

                if(hasAlreadyAttacked == false)
                {
                    SetDurability();
                    hasAlreadyAttacked = true;
                }

                if (HaveWeapon && (
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODSWORD ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONSWORD ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDSWORD ||                
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILSWORD))
                {
                    ResetAutoAttack();
                    anim.SetTrigger("stopAttack");
                }

                if (HaveWeapon && (
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.WOODHAMMER ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.IRONHAMMER ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.DIAMONDHAMMER ||
                    InventoryWeapons[currentWeaponIndex].weapon == WeaponRecipe.Weapon.MITHRILHAMMER))
                {
                    collider.GetComponent<Enemy>().SetBump(3.0f);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && isDashing)
        {
            collision.collider.GetComponent<Enemy>().SetKnockDown(2.0f);
        }
    }

    public void HideWeapon(bool _b)
    {
        if (currentWeaponIndex != -1)
            WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(!_b);
    }

    public CameraController GetCameraController()
    {
        return PivotCam.GetComponent<CameraController>();
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
            Peon.instance.playedSoundOfDeath = false;
        }
    }

    public void DestroyEquipedWeapon()
    {
        WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].SetActive(false);
        NoWeaponGameObject.SetActive(true);
        InventoryWeapons.RemoveAt(currentWeaponIndex);
        currentWeaponIndex = -1;

        if(InventoryWeapons.Count == 0)
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
        InventoryWeapons.Add(new WeaponRecipe(_weaponRecipe));
    }

    public void SetDurability()
    {
        if (currentWeaponIndex != -1)
        {
            InventoryWeapons[currentWeaponIndex].durability--;
            if (InventoryWeapons[currentWeaponIndex].durability <= 0)
                DestroyEquipedWeapon();
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
            WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = false;
    }


    #region UseInAnimator

    public void StompEnemies()
    {
        if (stompParticle)
        {
            stompParticle.gameObject.SetActive(true);
        }
        foreach (GameObject item in SpawnerManager.instance.GetAllEntities())
        {
            item.GetComponent<Enemy>().SetStun(enemyStunDuration);
        }

        Peon.instance.SetStun();
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
            if (HaveWeapon)
                WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = false;
            SetDurability();
        }
    }

    public void ResetAutoAttack()
    {
        isAttacking = false;
        canAttacking = true;
        foreach (Enemy item in enemiesEverAttacked)
        {
            item.canBeAttacking = true;
        }
        enemiesEverAttacked.Clear();
        hasAlreadyAttacked = false;
    }

    void WhrilwindTick()
    {
        if (currentWeaponIndex == -1)
            return;

        Collider[] hitColliders;
        switch (InventoryWeapons[currentWeaponIndex].weapon)
        {
            case WeaponRecipe.Weapon.WOODSWORD:
            case WeaponRecipe.Weapon.IRONSWORD:
            case WeaponRecipe.Weapon.DIAMONDSWORD:
            case WeaponRecipe.Weapon.MITHRILSWORD:
                hitColliders = Physics.OverlapSphere(transform.position, 5);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].GetComponent<Enemy>())
                    {
                        hitColliders[i].GetComponent<Enemy>().HealthPoint -= Damage;
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
                    if (hitColliders[i].GetComponent<Enemy>())
                    {
                        hitColliders[i].GetComponent<Enemy>().HealthPoint -= Damage;
                    }
                }
                break;

            case WeaponRecipe.Weapon.WOODHAMMER:
            case WeaponRecipe.Weapon.IRONHAMMER:
            case WeaponRecipe.Weapon.DIAMONDHAMMER:
            case WeaponRecipe.Weapon.MITHRILHAMMER:
                Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
                foreach (Collider hit in colliders)
                {
                    if (hit.GetComponent<Enemy>())
                    {
                        hit.GetComponent<Enemy>().SetBump(3.0f + stats.whirlwind);
                        hit.GetComponent<Enemy>().HealthPoint -= Damage;
                        Rigidbody rb = hit.GetComponent<Rigidbody>();
                        if (rb != null)
                            rb.AddExplosionForce(40, transform.position, 5, 3.0f);
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
        if (HaveWeapon)
            WeaponsGameObjects[(int)InventoryWeapons[currentWeaponIndex].weapon].GetComponent<MeleeWeaponTrail>().Emit = true;
    }

    #endregion

    #region LootAccessors
    public void AddItemInLootList(GameObject go)
    {
        if (!itemsToLoot.Contains(go))
        {
            itemsToLoot.Add(go);
            if (!currentLooting)
                currentLooting = itemsToLoot[0];
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
}
