using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorCtrl : MonoBehaviour
{
    [SerializeField] private float m_toggleDuration = 1.0f;
    [SerializeField] private float m_goalDuration = 1f;
    [SerializeField] private float m_backDistance = 30.0f;
    [SerializeField] private EnemyManager m_enemyMan = null;
    [SerializeField] private bool m_scroll;

    private Fermion m_fermion = null;
    private Boson m_boson = null;
    private Goal m_goal = null;

    private float m_countToggle;
    private float m_countGoal;
    private bool m_isToggling;
    private bool m_isGoaling;
    private Vector2 m_fermionPosBeforeToggle;
    private Vector2 m_bosonPosBeforeToggle;
    private Vector2 m_actorPosBeforeGoal;
    private bool m_canToggle;

    private void Start()
    {
        m_countToggle = 0.0f;
        m_countGoal = 0f;

        m_fermion = transform.GetComponentInChildren<Fermion>();
        m_boson = transform.GetComponentInChildren<Boson>();
        
        m_isToggling = false;
        m_isGoaling = false;
        m_canToggle = true;

        //最初はフェルミオンで始まる
        m_boson.trulyUsedMaxSpeed = m_fermion.trulyUsedMaxSpeed=m_fermion.uniqueMaxSpeed();
        m_boson.gameObject.layer = LayerMask.NameToLayer("NotUsedActor");
    }

    private void Update()
    {
        //Debug.Log("pause : " + StageManager.Instance.paused);
        if (!m_fermion.isAlive&&!m_boson.isAlive)
        {
            //ゲームオーバー
            TransitionManager.Instance.transite("scene_stageSelect", 1f, 1f);
        }

        if (StageManager.Instance.paused)
        {
            if (Input.GetButtonDown("pause"))
            {
                StageManager.Instance.paused = false;
            }
            return;
        }

        if (m_fermion.isGoal || m_boson.isGoal)
        {
            //ゴール
            Debug.LogError("Goal");
            processGoal();
            return;
        }

        if (m_fermion.isUtilized)
        {
            m_fermion.update();
            m_boson.notUtilizedUpdate(m_fermion);
        }
        else {
            m_boson.update();
            m_fermion.notUtilizedUpdate(m_boson);
        };

        //キャラが画面外に出ないように調整
        if (m_scroll)
        {

            correctFpos(ref m_fermion);
            correctBpos(ref m_boson);

            
        }


        if (!m_scroll)
        {
            processSalvation();
        }
        bool change =(!m_fermion.isAlive && m_boson.isAlive)||(m_fermion.isAlive&&!m_boson.isAlive);

        processToggle(change);

        if (Input.GetButtonDown("pause"))
        {
            StageManager.Instance.paused = true;
        }
    }

    private void processToggle(bool change)
    {
        if (m_fermion.isUtilized == m_boson.isUtilized)
        {
            Debug.LogError("Both fermi and bose is ACTIVE !");
        }
        

        //キャラ切り替え
        
        if (((Input.GetButtonDown("change")&&!m_isToggling&&m_canToggle)
            ||change&&m_canToggle))
        {
            if (change)
            {
                m_canToggle = false;
            }

            m_isToggling = true;
            m_countToggle = 0.0f;
            //切り替えアニメの補間の初めと終わり
            m_fermionPosBeforeToggle = m_fermion.transform.position;
            m_bosonPosBeforeToggle = m_boson.transform.position;
            m_fermionPosBeforeToggle.y += 1.0f;
            m_bosonPosBeforeToggle.y += 1.0f;

            //使用キャラ入れ替え
            m_fermion.isUtilized = !m_fermion.isUtilized;
            m_boson.isUtilized = !m_boson.isUtilized;

            //速度を使用キャラのものに。
            if (m_fermion.isUtilized)
            {
                m_fermion.trulyUsedMaxSpeed = m_boson.trulyUsedMaxSpeed = m_fermion.uniqueMaxSpeed();
                m_fermion.setWhiteColor();
                m_boson.setNotUsedColor();

                m_fermion.gameObject.layer = LayerMask.NameToLayer("Fermion");
                m_boson.gameObject.layer = LayerMask.NameToLayer("NotUsedActor");
            }
            else
            {
                m_fermion.trulyUsedMaxSpeed = m_boson.trulyUsedMaxSpeed = m_boson.uniqueMaxSpeed();
                m_fermion.setNotUsedColor();
                m_boson.setWhiteColor();

                m_boson.gameObject.layer = LayerMask.NameToLayer("Boson");
                m_fermion.gameObject.layer = LayerMask.NameToLayer("NotUsedActor");
            }

            m_fermion.GetComponent<Rigidbody2D>().velocity = m_boson.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        }

        if (!m_isToggling)
        {
            return;
        }
        
        m_countToggle += Time.deltaTime;

        if (m_boson.isUtilized)
        {
            m_boson.transform.position = Vector2.Lerp(m_bosonPosBeforeToggle, m_fermionPosBeforeToggle, m_countToggle);
            
        }
        else
        {
            m_fermion.transform.position = Vector2.Lerp(m_fermionPosBeforeToggle, m_bosonPosBeforeToggle, m_countToggle);

        }

        if (m_countToggle > m_toggleDuration)
        {
            m_isToggling = false;
        }

    }

    private void processSalvation()
    {
        Vector2 d = m_fermion.transform.position - m_boson.transform.position;
        float disSq = d.sqrMagnitude;

        if (disSq > m_backDistance * m_backDistance)
        {
            if (m_fermion.isUtilized)
            {
                Vector2 newPos=m_fermion.transform.position;
                newPos.y += 2.0f;
                m_boson.transform.position = newPos;
                m_boson.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                Vector2 newPos=m_boson.transform.position;
                newPos.y += 2.0f;
                m_fermion.transform.position = newPos;
                m_fermion.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
    private void processGoal()
    {    
        m_countGoal += Time.deltaTime;
        Actor actor = null;
        if (m_fermion.isUtilized)
        {
            actor = m_fermion;
        }
        else
        {
            actor = m_boson;
        }
        if (!m_isGoaling)
        {
            m_actorPosBeforeGoal = actor.transform.position;
            m_isGoaling = true;
        }
        if (m_goal == null)
        {
            m_goal = transform.GetComponentInChildren<Goal>();
        }
        actor.transform.position = Vector2.Lerp(m_actorPosBeforeGoal, m_goal.transform.position, m_countGoal);
 
        if (m_countGoal > m_goalDuration)
        {
            if (m_isGoaling)
            {
                TransitionManager.Instance.score = m_fermion.childFermionNumber;
                SaveData.Instance.setNewAtom(m_fermion.childFermionNumber);
                SaveData.Instance.setHighScore(TransitionManager.Instance.stageNo, m_fermion.childFermionNumber);
                SaveData.Instance.setClear(TransitionManager.Instance.stageNo);
                if (m_fermion.HP == m_fermion.maxHP &&
                    m_boson.HP == m_boson.maxHP)
                {
                    
                    if (m_enemyMan==null||m_enemyMan.dontEnemyKill())
                    {
                        SaveData.Instance.setMaxHPclear(TransitionManager.Instance.stageNo);
                    }
                }

                TransitionManager.Instance.transite("ResultScene", 0.3f, 0.8f);
                m_isGoaling = false;
            }        
        }
    }

    public Vector2 position()
    {
        if (m_isToggling)
        {
            if (m_fermion.isUtilized)
            {
                return m_bosonPosBeforeToggle;
            }
            else
            {
                return m_fermionPosBeforeToggle;
            }
            
        }

        if (m_fermion.isUtilized)
        {
            return m_fermion.transform.position;
        }
        else
        {
            return m_boson.transform.position;
        }
    }

    private void correctPos(ref Vector3 pos)
    {
        Camera c = Camera.main;
        Vector2 min = c.ViewportToWorldPoint(new Vector2(0f, 0f));
        Vector2 max = c.ViewportToWorldPoint(new Vector2(1f, 1f));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
    }

    private void correctFpos(ref Fermion f)
    {
        if (!f.isUtilized)
        {
            return;
        }
        Camera c = Camera.main;
        Vector2 min = c.ViewportToWorldPoint(new Vector2(0f, 0f));
        Vector2 max = c.ViewportToWorldPoint(new Vector2(1f, 1f));

        Vector2 pos = f.transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        f.transform.position = pos;
        if (pos.y < min.y - 3f)
        {
            int stageNo = TransitionManager.Instance.stageNo;
            string s = "scene_stage0" + stageNo;
            TransitionManager.Instance.transite(s, 0.5f, 0.2f);
        }
    }
    private void correctBpos(ref Boson b)
    {
        if (!b.isUtilized)
        {
            return;
        }
        Camera c = Camera.main;
        Vector2 min = c.ViewportToWorldPoint(new Vector2(0f, 0f));
        Vector2 max = c.ViewportToWorldPoint(new Vector2(1f, 1f));

        Vector2 pos = b.transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        b.transform.position = pos;
        if (pos.y < min.y - 3f)
        {
            int stageNo = TransitionManager.Instance.stageNo;
            string s = "scene_stage0" + stageNo;
            TransitionManager.Instance.transite(s, 0.5f, 0.2f);
        }
    }
}
