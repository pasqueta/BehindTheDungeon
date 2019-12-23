using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaineMenuForce : MonoBehaviour
{

    Rigidbody2D rb;
    float timer = 0;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer < 1)
        {
            if (rb!=null)
            {
                timer += Time.deltaTime;
                rb.velocity = new Vector2(10 * Screen.width / 1920, 0);
            }
            else
            {
                rb = GetComponent<Rigidbody2D>();
            }
           
        }
    }
}
