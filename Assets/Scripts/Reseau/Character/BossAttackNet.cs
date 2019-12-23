using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossAttackNet : NetworkBehaviour
{
    //-------------PRIVATE VARIABLES----------------//
    /*bool isAttacking = false;
    bool canAttacking = true;
    Animator anim;

    [SerializeField]
    AudioClip[] clips;

    AudioSource audioBoss;

    [SerializeField]
    AudioSource tourbilolSound;

    //skills
    Rigidbody rb;
    float chargeSpeed = 50.0f;
    public bool isCharging = false;
    bool hadCharged = false;
    float timerCharge = 0.0f;

    List<EnemyNet> enemyToAttack = new List<EnemyNet>();

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }*/

    // Update is called once per frame
    //void Update()
    //{
    //    if (!isLocalPlayer)
    //        return;

    //    if (!BossNet.instance.isBusy)
    //    {
    //        if (!GetComponent<BossNet>().IsDead)
    //        {
    //            BasicAttack();
    //            TourbiLol();
    //            Charge();
    //        }
    //    }
    //}

    //public void TourbilolSound()
    //{
    //    if (tourbilolSound)
    //    {
    //        tourbilolSound.Play();
    //    }
    //}

    //void BasicAttack()
    //{
    //    if (BossNet.instance.Controller == Controller.K1)
    //    {
    //        if (Input.GetMouseButtonDown(0) && canAttacking)
    //        {
    //            if (!isServer)
    //            {
    //                CmdAttack();
    //            }

    //            canAttacking = false;
    //            bool twohand = true;
    //            if (BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.WOODSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.IRONSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.DIAMONDSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.MITHRILSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.NONE)
    //            {
    //                twohand = false;
    //            }

    //            anim.SetBool("TwoHands", twohand);
    //            anim.SetTrigger("attack");
    //            Invoke("SetAttackBool", 0.1f);
    //            Invoke("ResetAttackBool", 0.6f);
    //            Invoke("ResetCanAttackBool", 1);
    //        }
    //    }
    //    else
    //    {
    //        if (Input.GetButtonDown(BossNet.instance.Controller + "_A") && canAttacking)
    //        {
    //            if (!isServer)
    //            {
    //                CmdAttack();
    //            }

    //            canAttacking = false;
    //            bool twohand = true;
    //            if (BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.WOODSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.IRONSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.DIAMONDSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.MITHRILSWORD
    //                || BossNet.instance.equippedWeapon == WeaponRecipe.Weapon.NONE)
    //            {
    //                twohand = false;
    //            }

    //            anim.SetBool("TwoHands", twohand);
    //            anim.SetTrigger("attack");
    //            Invoke("SetAttackBool", 0.1f);
    //            Invoke("ResetAttackBool", 0.6f);
    //            Invoke("ResetCanAttackBool", 1);
    //        }
    //    }
    //}

    //void TourbiLol()
    //{
    //    if (BossNet.instance.Controller == Controller.K1)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Alpha1))
    //        {
    //        }
    //    }
    //    else
    //    {
    //        //contrôle à la manette
    //    }

    //}

    //void Charge()
    //{
    //    if (BossNet.instance.Controller == Controller.K1)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Alpha2) && !isCharging && !hadCharged)
    //        {
    //            rb.velocity = BossNet.instance.transform.forward * chargeSpeed;
    //            anim.SetBool("charge", true);
    //            isCharging = true;
    //            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    //        }
    //    }
    //    else
    //    {
    //        if (Input.GetButtonDown(BossNet.instance.Controller + "_X") && !isCharging && !hadCharged)
    //        {
    //            BossNet.instance.transform.forward = Vector3.Normalize(BossNet.instance.transform.forward);
    //            rb.velocity = BossNet.instance.transform.forward * chargeSpeed;
    //            anim.SetBool("charge", true);
    //            isCharging = true;
    //            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    //        }
    //    }

    //    //fin de la charge
    //    if (isCharging && rb.velocity.x <= 1.0f && rb.velocity.z <= 1.0f)
    //    {
    //        anim.SetBool("charge", false);
    //        isCharging = false;
    //        hadCharged = true;
    //        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    //    }
    //    else if (isCharging)
    //    {
    //        BossNet.instance.GetCameraController().transform.position = transform.position;
    //    }

    //    //cool down
    //    if (hadCharged)
    //    {
    //        if (timerCharge < 10.0f)
    //        {
    //            timerCharge += Time.deltaTime;
    //        }
    //        else
    //        {
    //            hadCharged = false;
    //            timerCharge = 0.0f;
    //        }
    //    }
    //}

    //public void HitSound()
    //{
    //    //if (clips.Length > 0)
    //    //{
    //    //    audioBoss.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    //    //}
    //}

    //void SetAttackBool()
    //{
    //    isAttacking = true;
    //}

    //void ResetAttackBool()
    //{
    //    isAttacking = false;
    //    foreach (EnemyNet en in enemyToAttack)
    //    {
    //        if (en)
    //        {
    //            en.canBeAttacking = true;
    //        }
    //    }
    //    enemyToAttack.Clear();
    //}

    //void ResetCanAttackBool()
    //{
    //    canAttacking = true;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (isAttacking && other.tag == "Enemy")
    //    {
    //        if (other.gameObject.GetComponent<EnemyNet>().canBeAttacking)
    //        {
    //            HitSound();
    //            other.gameObject.GetComponent<EnemyNet>().HealthPoint -= BossNet.instance.stats.attack + (BossNet.instance.equippedWeapon != WeaponRecipe.Weapon.NONE ? PeonNet.instance.GiveWeaponRecipe((int)BossNet.instance.equippedWeapon).weaponAttack : 0);
    //            other.gameObject.GetComponent<EnemyNet>().canBeAttacking = false;
    //            enemyToAttack.Add(other.gameObject.GetComponent<EnemyNet>());
    //            if (BossNet.instance.equippedWeapon != WeaponRecipe.Weapon.NONE)
    //            {
    //                PeonNet.instance.GiveWeaponRecipe((int)BossNet.instance.equippedWeapon).durability--;

    //                if (PeonNet.instance.GiveWeaponRecipe((int)BossNet.instance.equippedWeapon).durability <= 0)
    //                {
    //                    BossNet.instance.DestroyEquipedWeapon();
    //                    PeonNet.instance.GiveWeaponRecipe((int)BossNet.instance.equippedWeapon).durability = PeonNet.instance.GiveWeaponRecipe((int)BossNet.instance.equippedWeapon).durabilityMax;
    //                }
    //            }
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == "Enemy" && isCharging)
    //    {
    //        collision.collider.GetComponent<EnemyNet>().SetKnockDown(5.0f);
    //    }
    //}

    //[Command]
    //void CmdAttack()
    //{
    //    canAttacking = false;
    //    anim.SetTrigger("attack");
    //    Invoke("SetAttackBool", 0.1f);
    //    Invoke("ResetAttackBool", 0.6f);
    //    Invoke("ResetCanAttackBool", 1);
    //}
}
