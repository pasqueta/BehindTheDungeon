using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.Audio;

public class Peon : MonoBehaviour
{
    public static Peon instance;

    [HideInInspector]
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

    [HideInInspector]
    public int totalMinerals = 0;
    //public int totalWeapons = 0;
    public UtilityStruct.MINERALS minerals;
    //public UtilityStruct.WEAPONS weapons;

    [HideInInspector]
    public int nbWeaponRecipe;
    [HideInInspector]
    public int nbTrapRecipe;

    [HideInInspector]
    public bool playedSoundOfDeath = false;


    public float currentStamina = 5.0f;
    public float maxStamina = 5.0f;
    int boostFactor = 5;

    Animator anim;

    //public bool notBusy = true;
    [HideInInspector]
    public bool isBusy = false;
    [HideInInspector]
    public bool isMining = false;

    //rotation
    Vector3 lastVelocity;

    //stun
    bool isStun = false;
    float stunTimer = 0.0f;

    void initStructs()
    {
        //minerals
        minerals.wood = 0;
        minerals.iron = 0;
        minerals.diamond = 0;
        minerals.mithril = 0;
        totalMinerals = 0;
        //stats perso
        stats.movementSpeed = 10 + 2 * PlayerPrefs.GetInt("peonTalentMovementSpeed");
        stats.pickAxe = 5 + 3 * PlayerPrefs.GetInt("peonTalentPickaxe");
        stats.craftingSpeed = 2 - 0.5f * PlayerPrefs.GetInt("peonTalentCraftingSpeed");
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

    bool initSuccess = false;
    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        
        if (DataManager.instance)
        {
            initSuccess = true;
            controller = DataManager.instance.ControllerPeon;
            pivotCam.GetComponent<CameraController>().screenOption = DataManager.instance.ScreenModePeon;
        }

        pivotCam.GetComponent<CameraController>().Ctrl = controller;
        pivotCam.GetComponent<CameraController>().SetTarget(transform);

        pivotCam.GetComponent<CameraController>().SetCameraPreset();

        nbWeaponRecipe = weaponRecipe.Length;
        nbTrapRecipe = trapRecipe.Length;
        // pivotCam = GameObject.Find("pivotCamera").transform;  
        initStructs();
    }

    private void LateUpdate()
    {
        pivotCam.gameObject.transform.position = new Vector3(transform.position.x, 11.41f,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
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
            (Controller == Controller.J1 || Controller == Controller.J2) && Input.GetButton(Controller+"_LStick"))
            && currentStamina > 0.0f)
        {
            currentStamina -= Time.deltaTime;
            velocity *= (stats.movementSpeed + boostFactor);
            anim.SetBool("sprint", true);
        }
        else
        {
            velocity *= stats.movementSpeed;
            anim.SetBool("sprint", false);
        }

        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_Peon") || anim.GetCurrentAnimatorStateInfo(0).IsName("Peon_Sprint"))
        {
            anim.speed = stats.movementSpeed / 10;
        }

        //  float diff = transform.rotation.eulerAngles.y - (transform.position + velocity).y;

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

        if (Input.GetAxisRaw(controller + "_LXaxis") != 0.0f || Input.GetAxisRaw(controller + "_LYaxis") != 0.0f)
        {
            if (lastVelocity != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lastVelocity), 0.3f);
            lastVelocity = rb.velocity;
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

    public void StunSound()
    {
        if (stunSound)
        {
            stunSound.Play();
        }
    }

    public void SetStun()
    {
        isStun = true;
        StunSound();
    }

    public void ResetMining()
    {

        isMining = false;
    }
}

