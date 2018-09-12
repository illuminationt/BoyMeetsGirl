using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCtrl : MonoBehaviour {

    [SerializeField] private GameObject[] m_masks;
    [SerializeField] private int m_atomNumber;
    [SerializeField] private Color32 m_notReleaseColor;
    [SerializeField] private GameObject m_explosion = null;

    [SerializeField] private GameObject m_Li = null;
    [SerializeField] private GameObject m_B = null;
    [SerializeField] private GameObject m_Na = null;
    [SerializeField] private GameObject m_K = null;
    [SerializeField] private GameObject m_Ca = null;
    [SerializeField] private GameObject m_As = null;
    [SerializeField] private GameObject m_Cu = null;
    [SerializeField] private GameObject m_Ba = null;
    [SerializeField] private GameObject m_Sr_Pb = null;






    private void Start()
    {
        Color c;
        for (int j = 0; j <= m_atomNumber; j++)
        {
            if (SaveData.Instance.hasTheAtom(j))
            {
                c = m_masks[j].GetComponent<Image>().color;
                c.a = 0;
                m_masks[j].GetComponent<Image>().color = c;
                if (!SaveData.Instance.releaseTheAtom(j))
                {
                    SaveData.Instance.releaseAtom(j);
                    explode(m_masks[j].transform.position,j);
                }
                
            }
            else
            {
                m_masks[j].GetComponent<Image>().color = m_notReleaseColor;
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("fire"))
        {
            TransitionManager.Instance.transite("scene_title", 0.3f,0.3f);
        }
    }

    private void explode(Vector3 pos,int atomicNumber)
    {
        GameObject o = null;
        switch (atomicNumber)
        {
            case 3:o = m_Li;break;
            case 5:o = m_B;break;
            case 11:o = m_Na;break;
            case 19:o = m_K;break;
            case 20:o = m_Ca;break;
            case 29:o = m_Cu;break;
            case 33:o = m_As;break;
            case 38:o = m_Sr_Pb;break;
            case 56:o = m_Ba;break;
            case 82:o=m_Sr_Pb;break;

            default:o = m_explosion;break;

        }
        Instantiate(o, pos, Quaternion.identity);
    }
}
