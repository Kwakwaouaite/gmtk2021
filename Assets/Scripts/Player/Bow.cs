using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Bow : MonoBehaviour
{
    public List<AudioClip> shootSounds;

    public Camera cam;
    public Rigidbody playerRb;
    public GameObject arrowPrefab;
    [SerializeField]
    float m_ShootVelocity = 20;

    bool m_ShootInput;

    // Update is called once per frame
    void Update()
    {
        if(m_ShootInput)
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            Rigidbody newArrowRb = newArrow.GetComponent<Rigidbody>();
            newArrowRb.velocity = cam.transform.forward * m_ShootVelocity;
            m_ShootInput = false;
            GetComponent<AudioSource>().PlayOneShot(GetRandomShootSound());
        }
    }

    public AudioClip GetRandomShootSound()
    {
        return shootSounds[Random.Range(0, shootSounds.Count)];
    }

    public void OnFire()
    {
        m_ShootInput = true;
    }
}
