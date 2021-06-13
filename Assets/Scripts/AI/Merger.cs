using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MonoBehaviour
{
    List<AIMove> m_AIs;

    List<AIMove> m_AIReadyToMerge;

    bool m_SmokeStarted;

    [SerializeField]
    ParticleSystem m_Smoke;

    private void InitList(List<PropsHolder> propsHolders)
    {
        m_AIs = new List<AIMove>();
        m_AIReadyToMerge = new List<AIMove>();

        foreach (PropsHolder propsHolder in propsHolders)
        {
            m_AIs.Add(propsHolder.GetComponent<AIMove>());
        }
    }

    private void Awake()
    {
        if (m_Smoke)
        {
            m_Smoke.Stop();
        }
    }


    public void StartMerge(List<PropsHolder> propsHolders)
    {
        InitList(propsHolders);

        ComputeCenter();

        foreach (AIMove ai in m_AIs)
        {
            ai.Merger = this;
            ai.StartMovement(this.transform, true);
        }
    }

    private void ComputeCenter()
    {
        if (m_AIs.Count == 0)
        {
            Debug.LogError("Try to merge 0 AIs");
        }

        Vector3 center = Vector3.zero;

        foreach (AIMove ai in m_AIs)
        {
            center += ai.transform.position;
        }

        center /= m_AIs.Count;

        this.transform.position = center;
    }

    public void OnAIArriveInZone(AIMove ai)
    {
        m_AIReadyToMerge.Add(ai);

        if (m_SmokeStarted)
        {
            AISpawner.GetInstance().DeactivatePawn(ai);
        }
        else
        {
            if (m_AIReadyToMerge.Count > 1)
            {
                if(m_Smoke)
                {
                    m_Smoke.Play();
                }

                foreach (AIMove aiMove in m_AIReadyToMerge)
                {
                    AISpawner.GetInstance().DeactivatePawn(aiMove);
                }
            }
        }

    }
}
