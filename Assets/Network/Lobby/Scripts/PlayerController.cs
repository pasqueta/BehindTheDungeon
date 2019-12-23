using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    
    Rigidbody rb;

    public float speed = 4.0f;
    public float angularSpeed = 2.0f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //var x = Input.GetAxis("Horizontal") /** Time.deltaTime*/ * 10.0f;
        //var z = Input.GetAxis("Vertical") /** Time.deltaTime*/ * 0.5f;

        //transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(x, 0.0f, z), Time.deltaTime);

        //transform.Rotate(0, x, 0);
        //transform.Translate(0, 0, z);
        //x = 0.0f;
        //z = 0.0f;
        

        Vector3 velocity = transform.forward * Input.GetAxisRaw("Vertical");
        velocity += transform.right * Input.GetAxisRaw("Horizontal");

        velocity.Normalize();

        velocity *= speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity *= 0.3f;
        }

        rb.velocity = velocity;
        
        rb.angularVelocity = Vector3.up * Input.GetAxis("Mouse X") * angularSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}