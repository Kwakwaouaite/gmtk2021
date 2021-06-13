using System;
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

    Animator m_Animator;

    [SerializeField]
    float m_BaseSpeed = 1.0f;

    [SerializeField]
    float m_RunSpeed = 4.0f;

    [SerializeField]
    float m_DistMinForExtraRunSpeed = 10.0f;

    [SerializeField]
    float m_PercentageBoostRunFar = 1.2f;

    [SerializeField]
    float m_MaxDistanceExtraSpeed = 40.0f;


    [SerializeField]
    ParticleSystem m_LoveParticle;

    Merger m_Merger;

    public Merger Merger { set { m_Merger = value; } get { return m_Merger; } }

    //float m_CurrentNormalSpeed = 1.0f;

    public void StopMovement()
    {
        m_Agent.isStopped = true;

        m_Animator.SetBool("IsStopped", true);

        StartLove();
    }

    public void StartMovement(Transform newDestination = null, bool run = false)
    {
        if (m_Agent.isOnNavMesh)
        {
            m_Agent.isStopped = false;

            SetDestination(newDestination ? newDestination : m_Destination);
        }

        m_Animator.SetBool("IsStopped", false);

        m_Animator.SetBool("Run", run);

        m_Agent.speed =  run ? m_RunSpeed : m_BaseSpeed;



        if (run)
        {
            float distanceToDest = (m_Destination.position - transform.position).magnitude;

            if (distanceToDest > m_DistMinForExtraRunSpeed)
            {
                float percentageBoost = (distanceToDest - m_DistMinForExtraRunSpeed) / (m_MaxDistanceExtraSpeed - m_DistMinForExtraRunSpeed);

                percentageBoost = Mathf.Clamp(percentageBoost, 0.0f, 1.0f);

                m_Agent.speed *= (1 + percentageBoost * m_PercentageBoostRunFar);
            }

            //m_Agent.speed *= (1 + (distanceToDest - m_DistMinForExtraRunSpeed) / (m_MaxExtraSpeedMultiplier - m_DistMinForExtraRunSpeed)) * m_MaxExtraSpeedMultiplier;
        }
        else
        {
            StopLove();
        }
    }

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        m_Animator = GetComponentInChildren <Animator>();
    }

    private void Update()
    {
        if (m_Destination)
        {
            if (IsCloseTo(m_Destination.position) && !m_Agent.isStopped)
            {
                if (m_Merger != null)
                {
                    StopMovement();
                    m_Merger.OnAIArriveInZone(this);
                }
                else
                {
                    AISpawner.GetInstance().DeactivatePawn(this);
                }
            }
        }

    }

    public void Init(Transform startingTransform, Transform destTransform)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.Warp (startingTransform.position);

        m_Merger = null;

        SetDestination(destTransform);

        agent.speed = m_BaseSpeed;

        StopLove();
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

    internal void OnTargetRemoved()
    {
        if (m_Agent.isOnNavMesh && m_Agent.isStopped)
        {
            StartMovement();
        }
    }

    private void StartLove()
    {
        m_LoveParticle.Play();
    }

    private void StopLove()
    {
        m_LoveParticle.Stop();
    }
}
