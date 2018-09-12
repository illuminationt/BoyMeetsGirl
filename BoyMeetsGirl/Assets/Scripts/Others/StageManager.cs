using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
    [SerializeField] private float m_timeDelay = 0.5f;
    [SerializeField] private float m_timeStopDuration = 3f;

    [SerializeField] private Vector2[] m_startPositions;
    public bool isTimeStop { get; set; }
    public float timeDelay { get { return m_timeDelay; } }
    public Vector2[] startPos { get { return m_startPositions; } }
    public bool paused { get; set; }
    
    private float m_count;

    private void Update()
    {
        if (isTimeStop)
        {
            m_count += Time.deltaTime;
            if (m_count > m_timeStopDuration)
            {
                isTimeStop = false;
                m_count = 0f;
            }
        }
    }


   
}
