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
    Quaternion m_previousRot;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!m_HitSomething)
        {
            m_previousRot = transform.rotation;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!m_HitSomething)
        {
            m_HitSomething = true;
            transform.rotation = m_previousRot; //To cancel the movement caused by the collision
            rb.constraints = RigidbodyConstraints.FreezeAll;

            PropsHolder target = collision.gameObject.GetComponent<PropsHolder>();
            if(target && target.GetComponent<AIMove>().Merger == null )
            {
                SelectionManager.Instance.AddTarget(target);
                EndGameManager.Instance.LoseBullet();
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
