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
        for(int i = 0; i < m_SelectedTargets.Count; i++)
        {
            lineRenderer.SetPosition(i, m_SelectedTargets[i].transform.position + Vector3.up);
        }
    }

    public void AddTarget(PropsHolder newTarget)
    {
        if(!m_SelectedTargets.Contains(newTarget))
        {
            m_SelectedTargets.Add(newTarget);
            lineRenderer.positionCount++;
        }

        newTarget.GetComponent<AIMove>().StopMovement();

    }

    public void RemoveTarget(PropsHolder newTarget)
    {
        if (m_SelectedTargets.Remove(newTarget))
        {
            lineRenderer.positionCount--;
        }

        OnTargetRemoved(newTarget);
    }

    private void OnTargetRemoved(PropsHolder target)
    {
        target.GetComponent<AIMove>().StartMovement();
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
                ScoreManager.Instance.LosePoints();
            }
            else
            {
                ScoreManager.Instance.GainPoints(m_SelectedTargets.Count, commonProps.Count);
            }


            CreateMerger(m_SelectedTargets);
            
            RemoveSelection();
        }
    }

    private void CreateMerger(List<PropsHolder> holders)
    {
        GameObject newObj = new GameObject("Merger");

        GameObject mergerGO = Instantiate(newObj);

        Merger merger = mergerGO.AddComponent<Merger>();

        merger.StartMerge(holders);
    }

    private void RemoveSelection()
    {
        for (int i = 0; i < m_SelectedTargets.Count;) // No need to advance in the list cause we remove them in the function
        {
            RemoveTarget(m_SelectedTargets[i]);
        }
    }
}
