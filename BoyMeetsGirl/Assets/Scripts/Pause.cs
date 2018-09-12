using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Pause : MonoBehaviour {
    [SerializeField] private GameObject m_plane = null;
    [SerializeField] private GameObject m_canvas = null;
    [SerializeField] private GameObject[] m_TMPs;
    private int m_cursorPos = 0;
    private bool m_isChangeCursorPos = false;
    private void Start()
    {
        m_cursorPos = 0;
        m_canvas.SetActive(false);
    }

    void Update() {
        if (StageManager.Instance.paused)
        {
            m_canvas.SetActive(true);
            Time.timeScale = 0f;
            Color c = m_plane.GetComponent<Image>().color;
            c.a = 0.3f;
            m_plane.GetComponent<Image>().color = c;

            float lsh = Input.GetAxis("L_Stick_V");
            if (!m_isChangeCursorPos)
            {
                if (lsh > 0.8f)
                {
                    m_cursorPos++;
                    m_isChangeCursorPos = true;
                }
                else if (lsh < -0.8f)
                {
                    m_cursorPos--;
                    m_isChangeCursorPos = true;
                }
                
                if (Input.GetKeyDown(KeyCode.I))
                {
                    m_cursorPos--;
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    m_cursorPos++;
                }
            }
            else
            {
                if (Mathf.Abs(lsh) < 0.8f)
                {
                    m_isChangeCursorPos = false;
                }
            }

            if (m_cursorPos >= 3)
            {
                m_cursorPos = 3;
            }
            else if (m_cursorPos <= 0)
            {
                m_cursorPos = 0;
            }

            for (int j = 0; j < 4; j++)
            {
                Color color = Color.white;
                if (m_cursorPos == j)
                {
                    color = Color.red;
                }
                m_TMPs[j].GetComponent<TextMeshProUGUI>().color = color;
            }

            if (Input.GetButtonDown("fire"))
            {
                StageManager.Instance.paused = false;
                switch (m_cursorPos)
                {
                    case 0:
                       
                        break;
                    case 1:
                        int no = TransitionManager.Instance.stageNo;
                        if (no == 0)
                        {
                            break;
                        }
                        string s = "scene_stage0" + no;
                        TransitionManager.Instance.transite(s, 0.5f, 0.1f);
                        break;
                    case 2:
                        TransitionManager.Instance.transite("scene_stageSelect", 1f, 0.4f);
                        break;
                    case 3:
                        TransitionManager.Instance.transite("scene_title", 1f, 0.3f);
                        break;
                }
            }
            Debug.Log("cursorPos : " + m_cursorPos);
        }

        else
        {
            Time.timeScale = 1f;
            m_cursorPos = 0;
            m_canvas.SetActive(false);
        }
        
    }
}