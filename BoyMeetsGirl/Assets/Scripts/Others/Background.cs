using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    [SerializeField] private float m_moveRate = 1f;
    [SerializeField] private bool m_isChaseCameraX = true;
    [SerializeField] private bool m_isChaseCameraY = false;

    public void update(Vector3 cameraMove)
    {
        Vector3 move = Vector3.zero;
        if (m_isChaseCameraX)
        {
            move.x += -cameraMove.x;
        }
        if (m_isChaseCameraY)
        {
            move.y += -cameraMove.y;
        }
        move *= m_moveRate;
        transform.position += move;
    }
}
