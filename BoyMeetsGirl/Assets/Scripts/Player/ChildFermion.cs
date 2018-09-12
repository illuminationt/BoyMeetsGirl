using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildFermion : MonoBehaviour {
    private Fermion m_fermion = null;
    [SerializeField] private float m_detectRadius = 2f;
    [SerializeField] private float m_lerpDuration = 1f;
    private bool m_isInterpolationg;
    private float m_countLerp;
    private Vector2 m_posBeforeLerp;

    private void Start()
    {
        m_isInterpolationg = false;
        m_fermion = FindObjectOfType<Fermion>();
        m_countLerp = 0f;
    }

    
    private void detectFermion()
    {
        Vector2 d = m_fermion.transform.position - transform.position;
        if (d.sqrMagnitude < m_detectRadius * m_detectRadius)
        {
            m_isInterpolationg = true;
            m_posBeforeLerp = transform.position;
        }
    }

    private void Update()
    {
        if (!m_isInterpolationg)
        {
            if(m_fermion.gameObject.activeSelf)
            detectFermion();
        }

        if (m_isInterpolationg)
        {
            m_countLerp += Time.deltaTime;
            transform.position = Vector2.Lerp(m_posBeforeLerp, m_fermion.transform.position, m_countLerp);

            if (m_countLerp > m_lerpDuration)
            {
                m_fermion.addChildFermion();
                Destroy(this.gameObject);
            }
        }
    }

}
