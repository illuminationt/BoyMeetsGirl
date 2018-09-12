using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WarpHole : MonoBehaviour {

    [SerializeField] private int m_connectedStage = 1;
    [SerializeField] private int m_maxFermionNumber = 1;
    private TextMeshProUGUI m_TMP = null;

    private void Start()
    {
        m_TMP = GetComponentInChildren<TextMeshProUGUI>();

        bool highScore = (SaveData.Instance.highScore(m_connectedStage) == m_maxFermionNumber);
        bool maxHP = SaveData.Instance.isClearMaxHP(m_connectedStage);
        bool clear = SaveData.Instance.isClear(m_connectedStage);
        Color32 c = Color.black;
        if (highScore)
        {
            c += Color.blue;
        }
        if (maxHP)
        {
            c += Color.red;
        }
        if (clear)
        {
            c += Color.green;
        }
        m_TMP.color = c;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Player":
                if (!col.gameObject.GetComponent<Actor>().isUtilized)
                {
                    return;
                }

                StartCoroutine(blackHole(col, 1f));

                
                    string sceneName = "scene_stage";
                    if (m_connectedStage <= 9)
                    {
                        sceneName += "0" + m_connectedStage;
                    }
                    else
                    {
                        sceneName += m_connectedStage;
                    }
                    if (!TransitionManager.Instance.isTransited)
                    {
                        TransitionManager.Instance.transite(sceneName, 1f, 0.2f);
                    }
                
                    break;
        }
    }

    private IEnumerator blackHole(Collider2D col,float time)
    {
       
        
        TransitionManager.Instance.stageNo = m_connectedStage;
        

        float t = 0;
        Vector2 oldPos = col.gameObject.transform.position;
        while (t <= time)
        {
            t += Time.deltaTime;
            col.gameObject.transform.position = Vector2.Lerp(oldPos, transform.position, t / time);
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            yield return null;
        }

      
    }
}
