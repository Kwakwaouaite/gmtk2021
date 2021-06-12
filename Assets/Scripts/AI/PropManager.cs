using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    List<Prop> m_Props;

    List<EProp> m_PropsToActivate;

    bool m_PropsActivationDirtyFlag;

    private void Awake()
    {
        if (m_Props == null)
        {
            m_Props = new List<Prop>();
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
            m_PropsActivationDirtyFlag = true;

            foreach (Prop prop in m_Props)
            {
                if (m_PropsToActivate.Contains(prop.Type))
                {
                    prop.gameObject.SetActive(true);
                }
                else
                {
                    prop.gameObject.SetActive(false);
                }
            }
        }
    }
}
