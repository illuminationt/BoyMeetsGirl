using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FermionLight : MonoBehaviour {
    private Fermion m_fermion = null;

    private void Start()
    {
        m_count = 0.0f;
        m_fermion = transform.GetComponentInParent<Fermion>();
    }

    void Update () {
        m_count += Time.deltaTime;
        GetComponent<Light>().range = 3f + Mathf.Sin(m_count);

       
	}

    private float m_count;
}
