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

    public List<AudioClip> winSounds;
    public List<AudioClip> loseSounds;

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
        target.GetComponent<AIMove>().OnTargetRemoved();
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

            bool isSuccess = commonProps.Count > 0;

            if (isSuccess)
            {
                ScoreManager.Instance.GainPoints(m_SelectedTargets.Count, commonProps.Count);
            }
            else
            {
                ScoreManager.Instance.LosePoints();
            }


            CreateMerger(m_SelectedTargets, isSuccess);
            
            RemoveSelection();
        }
    }

    public void PlaySound(bool isSuccess)
    {
        if (isSuccess)
        {
            GetComponent<AudioSource>().PlayOneShot(GetRandomWinSound());
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(GetRandomLoseSound());
        }
    }

    private void CreateMerger(List<PropsHolder> holders, bool isSuccess)
    {
        GameObject mergerGO = Instantiate(AISpawner.GetInstance().MergerPrefab);

        Merger merger = mergerGO.GetComponentInChildren<Merger>();

        if (merger == null)
        {
            merger = mergerGO.AddComponent<Merger>();
        }

        merger.StartMerge(holders, isSuccess);
    }

    private void RemoveSelection()
    {
        for (int i = 0; i < m_SelectedTargets.Count;) // No need to advance in the list cause we remove them in the function
        {
            RemoveTarget(m_SelectedTargets[i]);
        }
    }

    public AudioClip GetRandomWinSound()
    {
        return winSounds[Random.Range(0, winSounds.Count)];
    }

    public AudioClip GetRandomLoseSound()
    {
        return loseSounds[Random.Range(0, loseSounds.Count)];
    }
}
