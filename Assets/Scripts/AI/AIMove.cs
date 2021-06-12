using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{
    public Transform m_Destination;

    [SerializeField]
    float m_CloseToEpsilon = 1.0f;

    NavMeshAgent m_Agent;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (m_Destination)
        {
            if (IsCloseTo(m_Destination.position))
            {
                AISpawner.GetInstance().OnPawnReachedDestination(this);
                //Destroy(this.gameObject);
            }
        }

    }

    public void Init(Transform startingTransform, Transform destTransform)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.Warp(startingTransform.position);

        SetDestination(destTransform);
    }

    private void SetDestination(Transform transform)
    {
        m_Destination = transform;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = m_Destination.position;
    }

    private bool IsCloseTo(Vector3 position)
    {
        Vector3 shift = this.transform.position - position;
        return shift.magnitude < m_CloseToEpsilon;
    }
}
