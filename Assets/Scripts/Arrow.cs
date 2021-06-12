using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hitSomething = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitSomething = true;
        Disintegrate();
    }

   private void Disintegrate()
    {
        Destroy(gameObject);
    }
}
