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

    public void StopMovement()
    {
        m_Agent.isStopped = true;
    }

    public void StartMovement()
    {
        if (m_Agent.isOnNavMesh)
        {
            m_Agent.isStopped = false;

            SetDestination(m_Destination);
        }
    }

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
            }
        }

    }

    public void Init(Transform startingTransform, Transform destTransform)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.Warp (startingTransform.position);

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
