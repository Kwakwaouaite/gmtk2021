using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFarFog : MonoBehaviour
{
    public float m_scaleX = 1.0f;
    void Update()
    {
	GetComponent<MeshRenderer>().material.SetFloat("scaleX", m_scaleX);
    }
}
