using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    float m_TimeForDisintegrate = 1;

    MeshRenderer mesh;
    Rigidbody rb;
    bool m_HitSomething = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!m_HitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!m_HitSomething)
        {
            m_HitSomething = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            AIMove target = collision.gameObject.GetComponent<AIMove>();
            if(target)
            {
                SelectionManager.Instance.AddTarget(target);
            }

            StartCoroutine(Disintegrate());
        }
    }

    IEnumerator Disintegrate()
    {
        float timeSinceStart = 0;
        while(timeSinceStart < m_TimeForDisintegrate)
        {
            yield return new WaitForFixedUpdate();
            timeSinceStart += Time.fixedDeltaTime;
            mesh.material.SetFloat("_Intensity", timeSinceStart / m_TimeForDisintegrate);
        }
        Destroy(gameObject);
   }
}
