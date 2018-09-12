using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActorManager : MonoBehaviour {

    [SerializeField] private GameObject m_fermionHPgauge = null;
    [SerializeField] private GameObject m_bosonHPgauge = null;
    [SerializeField] private GameObject m_textMesh = null;
    [SerializeField] private GameObject m_childFermionUI = null;

    [SerializeField] private GameObject m_fermion = null;
    [SerializeField] private GameObject m_boson = null;

    private void Update()
    {
        if (!m_fermion) { return; }
        Fermion fermion = m_fermion.GetComponent<Fermion>();
        m_fermionHPgauge.GetComponent<Image>().fillAmount = (float)fermion.HP / (float)fermion.maxHP;

        Boson boson = m_boson.GetComponent<Boson>();
        m_bosonHPgauge.GetComponent<Image>().fillAmount = (float)boson.HP / (float)boson.maxHP;

        if (fermion.childFermionNumber > 0)
        {
            m_childFermionUI.SetActive(true);
            m_textMesh.SetActive(true);

            string text = "X " + fermion.childFermionNumber;
            m_textMesh.GetComponent<TextMeshProUGUI>().text = text;
        }
    }

}
