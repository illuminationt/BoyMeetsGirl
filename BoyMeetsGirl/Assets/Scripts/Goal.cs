using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    [SerializeField] float m_lastSize = 1f;
    [SerializeField] private GameObject m_explosion = null;

    public void goalExplode()
    {/*
        m_countScale += Time.deltaTime*m_explodeRate;
        Vector3 scale = transform.localScale;
        scale.x = scale.y = m_countScale;
        transform.localScale = scale;

        Color c = m_fadeOut.GetComponent<Image>().color;
        c.a += Time.deltaTime * m_fadeOutRate;
        m_fadeOut.GetComponent<Image>().color = c;

        
        if (m_countScale > m_transitionScale)
        {
            SceneManager.LoadScene("ResultScene");
        }
        */
    }
    public IEnumerator goalExplode(float fadeTime,Fermion fermion)
    {
        

        Vector3 scale = transform.localScale;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float s = (m_lastSize - 1f) * t / fadeTime + 1f;
            scale.x = scale.y = s;
            transform.localScale = scale;

            yield return null;
        }
        
    }

    public void explode()
    {
        Instantiate(m_explosion, transform.position, Quaternion.identity);
    }
}
