using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultCtrl : MonoBehaviour {

    [SerializeField] private GameObject m_TMP = null;

    private void Start()
    {
        int score = TransitionManager.Instance.score;
        string name = GlobalData.Instance.atomName[score];
        string text = "手に入れたフェルミオン... " + score + " 個\n元素表 No. " + score + " : " + name + " を手に入れた\n 弾丸キーで戻る";
        m_TMP.GetComponent<TextMeshProUGUI>().text = text;

    }

    private void Update()
    {
        if (Input.GetButtonDown("fire"))
        {
            int score = TransitionManager.Instance.score;
            if (!SaveData.Instance.releaseTheAtom(score))
            {
                TransitionManager.Instance.transite("scene_collection", 0.2f, 0.1f);
            }
            else
            {
                TransitionManager.Instance.transite("scene_stageSelect",0.2f,0.1f);
            }

        }
    }
}
