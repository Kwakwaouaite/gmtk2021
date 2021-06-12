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

    private List<AIMove> m_SelectedTargets;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectedTargets = new List<AIMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTarget(AIMove newTarget)
    {
        if(!m_SelectedTargets.Contains(newTarget))
        {
            m_SelectedTargets.Add(newTarget);
        }
    }

    public void OnCancelSelection()
    {
        m_SelectedTargets.Clear();
    }

    public void OnSubmitSelection()
    {
        //TODO check result
        m_SelectedTargets.Clear();
    }
}
