using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{
    public Transform m_Destination;

    [SerializeField]
    float m_CloseToEpsilon = 1.0f;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.
    }

    private void Update()
    {
        if (m_Destination)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = m_Destination.position;


            if (IsCloseTo(m_Destination.position))
            {
                Destroy(this.gameObject);
            }
        }

    }


    public void SetDestination(Transform transform)
    {
        m_Destination = transform;

        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.destination = m_Destination.position;
    }

    private bool IsCloseTo(Vector3 position)
    {
        Vector3 shift = this.transform.position - position;
        Debug.Log(shift.magnitude);
        return shift.magnitude < m_CloseToEpsilon;
    }
}
