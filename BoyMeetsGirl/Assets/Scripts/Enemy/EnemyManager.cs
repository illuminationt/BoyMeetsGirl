using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour {
    private EnemyBase[] m_enemies = null;

    [SerializeField] private GameObject m_enemyHpGauge = null;
    [SerializeField] private GameObject m_enemyHpGaugeBackground = null;
    [SerializeField] private GameObject m_textMesh = null;

    [SerializeField] private GameObject m_boson = null;
    private int m_HPgaugeShownEnemyID = -1;

    private void Start()
    {
        m_enemies = FindObjectsOfType<EnemyBase>();
    }

    private void Update()
    {
        if (StageManager.Instance.paused)
        {
            return;
        }

        foreach(EnemyBase enemy in m_enemies)
        {
            if (enemy != null)
            {
                float timeScale = 1f;
                if (StageManager.Instance.isTimeStop)
                {
                    timeScale = StageManager.Instance.timeDelay;
                }
                enemy.update(timeScale);
            }
        }

        for(int j = 0; j < m_enemies.Length; j++)
        {
            if (m_enemies[j].isDamaged)
            {
                if (m_enemies[j].wasDamaged())
                {
                    m_HPgaugeShownEnemyID = j;

                    break;
                }
            }
           
        }
        if (m_HPgaugeShownEnemyID == -1)
        {
            return;
        }
        showHPgauge(m_enemies[m_HPgaugeShownEnemyID]);
        showEnemyName(m_enemies[m_HPgaugeShownEnemyID]);
    }

    public bool dontEnemyKill()
    {
        foreach (EnemyBase enemy in m_enemies)
        {
            if (enemy == null)
            {
                
                return false;
            }
        }

        return true;
    }

    private void showHPgauge(EnemyBase enemy)
    {
        m_enemyHpGaugeBackground.SetActive(true);
        m_enemyHpGauge.SetActive(true);

        m_enemyHpGauge.GetComponent<Image>().fillAmount = (float)enemy.HP / (float)enemy.maxHP;

    }

    private void showEnemyName(EnemyBase enemy)
    {
        if (m_textMesh == null)
        {
            Debug.LogError("m_textMesh ==null");
            return;
        }
        m_textMesh.SetActive(true);

        if (enemy == null)
        {
            m_textMesh.GetComponent<TextMeshProUGUI>().text = "";
            return;
        }
        m_textMesh.GetComponent<TextMeshProUGUI>().text = enemy.name;
    }
}
