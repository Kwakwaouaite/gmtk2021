using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Material propMaterial;

    [SerializeField]
    EProp m_Type;

    public EProp Type { get { return m_Type; } }

    // Start is called before the first frame update
    void Start()
    {
        if (m_Type == EProp.Invalid)
        {
            Debug.LogError("A prop have an invalid tag");
        }

        PropsHolder propManager = transform.GetComponentInParent<PropsHolder>();

        propManager.RegisterProp(this);

        Renderer renderer = GetComponent<Renderer>();
        if(renderer)
        {
            renderer.material = propMaterial;
        }
    }

    public void Init()
    {
        Renderer renderer = GetComponent<Renderer>();
        if(renderer)
        {
            renderer.material.color = PropsManager.GetInstance().GetRandomColor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
