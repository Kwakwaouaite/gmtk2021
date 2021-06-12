using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Camera cam;
    public Rigidbody playerRb;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public float shootVelocity = 20;

    private bool shootInput;

    // Update is called once per frame
    void Update()
    {
        if(shootInput)
        {
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
            Rigidbody newArrowRb = newArrow.GetComponent<Rigidbody>();
            newArrowRb.velocity = cam.transform.forward * shootVelocity;
            shootInput = false;
        }
    }

    public void OnFire()
    {
        shootInput = true;
    }
}
