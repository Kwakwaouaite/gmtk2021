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
    GameObject m_AIPrefab;

    [SerializeField]
    SpawnPointGroup[] m_SpawnPointGroups;

    List<GameObject> m_ActiceAIs;
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

    public void OnPawnReachedDestination(AIMove aIMove)
    {
        aIMove.gameObject.SetActive(false);

        m_InactiveAIs.Add(aIMove.gameObject);
    }

    private void Awake()
    {
        s_Instance = this;

        m_ActiceAIs = new List<GameObject>();
        m_InactiveAIs = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LastSpawn = Time.time;
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

    private void SpawnAI()
    {
        GameObject newAgent;
        
        if (m_InactiveAIs.Count > 0)
        {
            newAgent = m_InactiveAIs[0];
            m_InactiveAIs.RemoveAt(0);

            newAgent.SetActive(true);
        }
        else
        {
            newAgent = Instantiate(m_AIPrefab, this.transform);
        }

        InitializeAI(newAgent);
    }

    private void InitializeAI(GameObject newAgent)
    {
        int spawnGroupIndex = UnityEngine.Random.Range(0, 2);

        SpawnPointGroup startingGroup = m_SpawnPointGroups[spawnGroupIndex];
        SpawnPointGroup destinationGroup = m_SpawnPointGroups[1 - spawnGroupIndex];

        Transform spawnTransform = startingGroup.m_SpawnPoints[UnityEngine.Random.Range(0, startingGroup.m_SpawnPoints.Length)];
        Transform destinationTransform = destinationGroup.m_SpawnPoints[UnityEngine.Random.Range(0, destinationGroup.m_SpawnPoints.Length)];

        AIMove moverNewAgent = newAgent.GetComponent<AIMove>();

        moverNewAgent.Init(spawnTransform, destinationTransform);


    }
}
