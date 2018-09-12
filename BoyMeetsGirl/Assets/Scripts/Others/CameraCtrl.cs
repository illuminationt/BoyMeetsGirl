using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    private Camera m_mainCamera;

    [SerializeField] private GameObject m_actorCtrl;
    //セクション範囲定義用
    [SerializeField] private Rect m_sectionRect;
    [SerializeField] private Rect m_cameraRect;

    [SerializeField] private bool m_scroll;
    [SerializeField] private Vector2[] m_scrollNodes;
    [SerializeField] private float[] m_scrollSpeeds;

    private Background[] m_backGrounds = null;


    private int m_nextNode = 1;
    private Vector3 m_nowPosBetweenNodes;
    private Vector3? m_prevPos = null;

    private void Start()
    {
        m_mainCamera = Camera.main;
        m_nowPosBetweenNodes = Vector3.zero;

        m_backGrounds = FindObjectsOfType<Background>();
    }

    private void Update()
    {
        if (m_scroll)
        {
            scrollUpdate();
        }
        else
        {
            update();
        }
    }

    private void update()
    {
        //基本的にはプレイヤーに追従
        transform.position = m_actorCtrl.GetComponent<ActorCtrl>().position();

        //時間を止める
        if (StageManager.Instance.isTimeStop||true)
        {
            m_mainCamera.GetComponent<CameraScript>().enabled = true;
        }
        else
        {
            //m_mainCamera.GetComponent<CameraScript>().enabled = false;
        }

        //キャラが端とか下に行きすぎたときにカメラがそこまで
        //ついて行かなくするためのコード
        Vector3 nowPos = transform.position;
        nowPos.x = Mathf.Clamp(nowPos.x, m_cameraRect.xMin + m_sectionRect.width/2f, m_cameraRect.xMax - m_sectionRect.width / 2f);
        nowPos.y = Mathf.Clamp(nowPos.y, m_cameraRect.yMin + m_sectionRect.height / 2f, m_cameraRect.yMax - m_sectionRect.height / 2f);
        transform.position = nowPos;


        //背景を動かすコード
        if (m_prevPos != null)
        {
            Vector3 dis = nowPos -(Vector3)m_prevPos;
            foreach (Background b in m_backGrounds)
            {
                if (b != null)
                {
                    b.update(dis);
                }
            }
        }


        m_prevPos = nowPos;
    }

    private void scrollUpdate()
    {
        if (StageManager.Instance.paused)
        {
            return;
        }
        if (TransitionManager.Instance.isSalvated)
        {
            transform.position = m_scrollNodes[0];
            m_nextNode = 1;
            return;
        }
        if (m_nextNode > m_scrollNodes.Length-1)
        {
            return;
        }
        Vector2 d = m_scrollNodes[m_nextNode] - m_scrollNodes[m_nextNode - 1];
        Vector3 n = new Vector3(d.x,d.y,0f);//Vector2は構造体なので同じアドレスじゃあない。
        n.Normalize();

        m_nowPosBetweenNodes += n * m_scrollSpeeds[m_nextNode-1];
        transform.position += n * m_scrollSpeeds[m_nextNode - 1];

        //ゴールのノードを越えた
        if (m_nowPosBetweenNodes.sqrMagnitude > d.sqrMagnitude)
        {
            m_nowPosBetweenNodes = Vector3.zero;
            m_nextNode++;
        }
    }


    
    void OnDrawGizmos()
    {
        Vector3 top_left = new Vector3(m_cameraRect.xMin, m_cameraRect.yMax, 0);
        Vector3 top_right = new Vector3(m_cameraRect.xMax, m_cameraRect.yMax, 0);
        Vector3 bottom_left = new Vector3(m_cameraRect.xMin, m_cameraRect.yMin, 0);
        Vector3 bottom_right = new Vector3(m_cameraRect.xMax, m_cameraRect.yMin, 0);
        // カメラ描画範囲を表示
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(bottom_left, top_left);
        Gizmos.DrawLine(top_left, top_right);
        Gizmos.DrawLine(top_right, bottom_right);
        Gizmos.DrawLine(bottom_right, bottom_left);
    }
    /*
    void OnDrawGizmos()
    {
        Camera camera = Camera.main;
        float fov = camera.fov;
        float size = camera.orthographicSize;
        float max = camera.farClipPlane;
        float min = camera.nearClipPlane;
        float aspect = camera.aspect;

        Color tempColor = Gizmos.color;
        Gizmos.color = Color.blue;

        Matrix4x4 tempMat = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(this.transform.position, this.transform.rotation, new Vector3(aspect, 1.0f, 1.0f));
        if (camera.orthographic)
        {
            //OrthoGraphicカメラ設定
            Gizmos.DrawWireCube(new Vector3(0.0f, 0.0f, ((max - min) / 2.0f) + min), new Vector3(size * 2.0f, size * 2.0f, max - min));
        }
        else
        {
            //Perspectiveカメラ設定
            Gizmos.DrawFrustum(Vector3.zero, fov, max, min, 1.0f);
        }
        Gizmos.color = tempColor;
        Gizmos.matrix = tempMat;

    }
    */

}
