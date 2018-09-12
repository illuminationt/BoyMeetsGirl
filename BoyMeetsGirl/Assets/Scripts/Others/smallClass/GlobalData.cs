using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : SingletonMonoBehaviour<GlobalData>
{
    [SerializeField] private string[] m_atomName;

    public string[] atomName
    {
        get
        {
            return m_atomName;
        }
    }
}
