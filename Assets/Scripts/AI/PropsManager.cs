using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{

    [System.Serializable]
    public struct PropGroup
    {
        public string name;
        public List<EProp> m_EProps;
    }

    public List<PropGroup> m_PropGroups;
    public List<Color> m_PropColors;

    private static PropsManager s_Instance;
    static public PropsManager GetInstance()
    {
        if (s_Instance == null)
        {
            Debug.LogError("No PropsManager found in the level");
        }

        return s_Instance;
    }

    private void Awake()
    {
        s_Instance = this;
    }

    public List<EProp> GenerateRandomProps()
    {
        List<EProp> randomProps = new List<EProp>();

        foreach (PropGroup group in m_PropGroups)
        {
            randomProps.Add(group.m_EProps[Random.Range(0, group.m_EProps.Count)]);
        }

        return randomProps;
    }

    public Color GetRandomColor()
    {
        return m_PropColors[Random.Range(0, m_PropColors.Count)];
    }
}
