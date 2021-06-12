using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Camera cam;
    public Rigidbody playerRb;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    [SerializeField]
    float m_ShootVelocity = 20;

    bool m_ShootInput;

    // Update is called once per frame
    void Update()
    {
        if(m_ShootInput)
        {
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
            Rigidbody newArrowRb = newArrow.GetComponent<Rigidbody>();
            newArrowRb.velocity = cam.transform.forward * m_ShootVelocity;
            m_ShootInput = false;
        }
    }

    public void OnFire()
    {
        m_ShootInput = true;
    }
}
