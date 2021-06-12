using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private static SelectionManager s_instance = null;
    public static SelectionManager Instance
    {
        get
        {
            return s_instance;
        }
    }
    private void Awake()
    {
        // if the singleton has already been initialized
        if (s_instance != null && s_instance != this)
        {
            Destroy(this.gameObject);
        }
        s_instance = this;
    }

    private List<PropsHolder> m_SelectedTargets;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectedTargets = new List<PropsHolder>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        for(int i = 0; i < m_SelectedTargets.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, m_SelectedTargets[i].transform.position);
        }
    }

    public void AddTarget(PropsHolder newTarget)
    {
        if(!m_SelectedTargets.Contains(newTarget))
        {
            m_SelectedTargets.Add(newTarget);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newTarget.transform.position);
        }
    }

    public void OnCancelSelection()
    {
        RemoveSelection();
    }

    public void OnSubmitSelection()
    {
        if(m_SelectedTargets.Count >= 2)
        {
            List<EProp> commonProps = m_SelectedTargets[0].PropsToActivate;
            for (int i = 1; i < m_SelectedTargets.Count; i++)
            {
                commonProps = commonProps.FindAll(delegate(EProp eProp) { return m_SelectedTargets[i].PropsToActivate.Contains(eProp); });
            }
            if(commonProps.Count == 0)
            {
                //Lose
            }
            else
            {
                //Win
            }
            RemoveSelection();
        }
    }

    private void RemoveSelection()
    {
        lineRenderer.positionCount = 1;
        m_SelectedTargets.Clear();
    }
}
