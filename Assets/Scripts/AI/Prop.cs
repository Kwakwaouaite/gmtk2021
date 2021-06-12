using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
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

        PropManager propManager = transform.GetComponentInParent<PropManager>();

        propManager.RegisterProp(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
