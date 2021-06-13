using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MonoBehaviour
{
    List<AIMove> m_AIs;

    private void InitList(List<PropsHolder> propsHolders)
    {
        m_AIs = new List<AIMove>();

        foreach (PropsHolder propsHolder in propsHolders)
        {
            m_AIs.Add(propsHolder.GetComponent<AIMove>());
        }
    }


    public void StartMerge(List<PropsHolder> propsHolders)
    {
        InitList(propsHolders);

        ComputeCenter();

        foreach (AIMove ai in m_AIs)
        {
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
}