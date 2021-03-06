using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Create a custom struct and apply [Serializable] attribute to it
[Serializable]
public struct SpawnPointGroup
{
    [SerializeField]
    public Transform[] m_SpawnPoints;
}

public class AISpawner : MonoBehaviour
{

    [SerializeField]
    float m_TimeBetweenSpawn = 10.0f;

    float m_LastSpawn = 0.0f;

    [SerializeField]
    int m_MaxAICount = 60;

    [SerializeField]
    GameObject m_AIPrefab;

    [SerializeReference]
    GameObject m_MergerPrefab;
    public GameObject MergerPrefab { get { return m_MergerPrefab; } }

    [SerializeField]
    SpawnPointGroup[] m_SpawnPointGroups;

    [SerializeField]
    List<GameObject> m_SpawnPointMiddle;

    int m_CreatedAICount = 0;
    List<GameObject> m_InactiveAIs;

    private static AISpawner s_Instance;
    static public AISpawner GetInstance()
    {
        if (s_Instance == null)
        {
            Debug.LogError("No AISpawner found in the level");
        }

        return s_Instance;
    }

    public void DeactivatePawn(AIMove aIMove)
    {
        aIMove.gameObject.SetActive(false);

        m_InactiveAIs.Add(aIMove.gameObject);

        SelectionManager selectionManager = SelectionManager.Instance;

        if (selectionManager)
        {
            selectionManager.RemoveTarget(aIMove.GetComponent<PropsHolder>());
        }
    }

    private void Awake()
    {
        s_Instance = this;

        m_InactiveAIs = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();

        foreach (GameObject spawnPoint in m_SpawnPointMiddle)
        {
            SpawnAI(spawnPoint.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - m_LastSpawn > m_TimeBetweenSpawn)
        {
            m_LastSpawn = Time.time;

            SpawnAI();
        }
    }

    public void Reset()
    {
        m_LastSpawn = Time.time;
    }

    private void SpawnAI(Transform start = null)
    {

        GameObject newAgent = null;
        
        if (m_InactiveAIs.Count > 0)
        {
            newAgent = m_InactiveAIs[0];
            m_InactiveAIs.RemoveAt(0);

            newAgent.SetActive(true);
        }
        else if (m_CreatedAICount < m_MaxAICount)
        {
            newAgent = Instantiate(m_AIPrefab, this.transform);

            m_CreatedAICount++;
        }

        if (newAgent != null)
        { 
            InitializeNavmeshAgent(newAgent, start);

            InitializeProp(newAgent);
        }

    }

    private void InitializeProp(GameObject newAgent)
    {
        PropsHolder propManager = newAgent.GetComponent<PropsHolder>();

        List<EProp> propsToUse = PropsManager.GetInstance().GenerateRandomProps();

        propManager.ResetAndActivateProps(propsToUse);
    }

    private void InitializeNavmeshAgent(GameObject newAgent, Transform forceStart = null)
    {
        int spawnGroupIndex = UnityEngine.Random.Range(0, 2);

        SpawnPointGroup startingGroup = m_SpawnPointGroups[spawnGroupIndex];
        SpawnPointGroup destinationGroup = m_SpawnPointGroups[1 - spawnGroupIndex];

        Transform spawnTransform = forceStart != null ? forceStart : startingGroup.m_SpawnPoints[UnityEngine.Random.Range(0, startingGroup.m_SpawnPoints.Length)];

        Transform destinationTransform = destinationGroup.m_SpawnPoints[UnityEngine.Random.Range(0, destinationGroup.m_SpawnPoints.Length)];

        AIMove moverNewAgent = newAgent.GetComponent<AIMove>();

        moverNewAgent.Init(spawnTransform, destinationTransform);
    }
}
