using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsHolder : MonoBehaviour
{
    List<Prop> m_Props;

    List<EProp> m_PropsToActivate;

    public List<EProp> PropsToActivate
    {
        get { return m_PropsToActivate; }
    }

    bool m_PropsActivationDirtyFlag;

    private void Awake()
    {
        if (m_Props == null)
        {
            m_Props = new List<Prop>();
        }
        if (m_PropsToActivate == null)
        {
            m_PropsToActivate = new List<EProp>();
        }
    }

    public void RegisterProp(Prop prop)
    {
        m_Props.Add(prop);
    }

    public void ResetAndActivateProps(List<EProp> propsToActivate)
    {
        m_PropsToActivate = propsToActivate;

        m_PropsActivationDirtyFlag = true;
    }

    private void Update()
    {
        if (m_PropsActivationDirtyFlag == true)
        {
            m_PropsActivationDirtyFlag = false;

            foreach (Prop prop in m_Props)
            {
                if (m_PropsToActivate.Contains(prop.Type))
                {
                    prop.gameObject.SetActive(true);
                    prop.Init();
                }
                else
                {
                    prop.gameObject.SetActive(false);
                }
            }
        }
    }
}
