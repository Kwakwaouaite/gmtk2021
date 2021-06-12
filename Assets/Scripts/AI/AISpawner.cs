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
        int spawnGroupIndex = UnityEngine.Random.Range(0, 2);

        SpawnPointGroup startingGroup = m_SpawnPointGroups[spawnGroupIndex];
        SpawnPointGroup destinationGroup = m_SpawnPointGroups[1 - spawnGroupIndex];


        Transform spawnTransform = startingGroup.m_SpawnPoints[UnityEngine.Random.Range(0, startingGroup.m_SpawnPoints.Length)];
        Transform destinationTransform = destinationGroup.m_SpawnPoints[UnityEngine.Random.Range(0, destinationGroup.m_SpawnPoints.Length)];

        GameObject newAgent = Instantiate(m_AIPrefab, spawnTransform.position, spawnTransform.rotation, this.transform);

        AIMove moverNewAgent = newAgent.GetComponent<AIMove>();

        moverNewAgent.SetDestination(destinationTransform);
    }
}
