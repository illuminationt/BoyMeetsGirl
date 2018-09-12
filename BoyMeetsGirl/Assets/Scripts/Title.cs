using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour {

    [SerializeField] float m_transitionDuration = 2f;


    [SerializeField] GameObject m_titleTMP = null;
    [SerializeField] GameObject m_gameTMP = null;
    [SerializeField] GameObject m_collectionTMP = null;
    [SerializeField] GameObject m_deleteTMP = null;
    [SerializeField] GameObject m_cursor = null;

    [SerializeField] Vector3[] m_cursorPositions;

    [SerializeField] private AudioClip m_clip = null;
    private int m_cursorPos;
    private bool m_isChangeCursorPos = false;

    private int m_trueCursorPos;
   
    private void Start()
    {
       
        m_cursor.transform.position = m_gameTMP.transform.position;
        m_cursorPos = 0;
        m_trueCursorPos = 0;
    }

    private void Update()
    {


#if false
        if (Input.GetKeyDown(KeyCode.J) ||
            Input.GetKeyDown(KeyCode.L))
        {
            switch (m_cursorPos)
            {
                case 0:
                    m_cursorPos = 1;
                    m_cursor.transform.position = m_collectionTMP.transform.position;
                    break;
                case 1:
                    m_cursorPos = 0;
                    m_cursor.transform.position = m_gameTMP.transform.position;
                    break;
            }
        }
#endif
        if (Input.GetKeyDown(KeyCode.J))
        {
            m_cursorPos--;
            m_trueCursorPos--;
        }else if (Input.GetKeyDown(KeyCode.L))
        {
            m_cursorPos++;
            m_trueCursorPos++;
        }

        float lsh = Input.GetAxis("L_Stick_H");
        if (!m_isChangeCursorPos)
        {
            
            if (lsh >0.8f)
            {
                m_cursorPos++;
                m_trueCursorPos++;
                m_isChangeCursorPos = true;
            }
            else if (lsh <-0.8f)
            {
                m_cursorPos--;
                m_trueCursorPos--;
                m_isChangeCursorPos = true;
            }

        }
        else
        {
            if (Mathf.Abs(lsh) < 0.8f)
            {
                m_isChangeCursorPos = false;
            }
        }

        if (m_cursorPos < 0)
        {
            m_cursorPos = 0;
        }else if (m_cursorPos > 1)
        {
            m_cursorPos = 1;
        }

        switch (m_cursorPos)
        {
            case 0:
                m_cursor.transform.position = m_gameTMP.transform.position;
                break;
            case 1:
                m_cursor.transform.position = m_collectionTMP.transform.position;
                break;
        }


        if (m_trueCursorPos > 100)
        {
            m_cursor.transform.position = m_deleteTMP.transform.position;
            m_deleteTMP.GetComponent<TextMeshProUGUI>().color = Color.black;
            if (Input.GetButtonDown("fire"))
            {
                SaveData.Instance.delete();
                m_trueCursorPos = m_cursorPos;
                TransitionManager.Instance.transite("scene_title", 1f, 1f);
                AudioSource a = GetComponent<AudioSource>();
                a.PlayOneShot(m_clip);
            }
            return;
        }

        if (Input.GetButtonDown("fire"))
        {
            switch (m_cursorPos)
            {
                case 0:
                    TransitionManager.Instance.transite("scene_stageSelect", m_transitionDuration, 1f);
                    break;
                case 1:
                    TransitionManager.Instance.transite("scene_collection", 1f, 0.4f);
                    break;

            }

            AudioSource a= GetComponent<AudioSource>();
            a.PlayOneShot(m_clip);

        }

        

    }  
}
