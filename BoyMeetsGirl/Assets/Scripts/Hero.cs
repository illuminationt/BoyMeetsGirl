using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : EnemyBase {

    //private Animator m_animCtrl = null;
    [SerializeField]private float m_jumpForce = 5f;
    [SerializeField]private float m_speed = 0.1f;
    [SerializeField] private ActorCtrl m_actorCtrl = null;
    [SerializeField] private GameObject m_goal = null;
    public bool isLanding { get; private set; }

    public enum wantAction
    {
        LEFT,
        RIGHT,
        JUMP,
        ATTACK,
        IDLE,
    }
    public wantAction want { get; private set; }

    StateBase<Hero> m_state = null;

    private float m_countAction = 0f;

    public Animator animCtrl
    {
        get { return m_animCtrl; }
    }
    

    protected override void Start()
    {
        base.Start();
        m_animCtrl = GetComponent<Animator>();
        m_state = new StateIdle();
    }

    public override void update(float timeScale=1f)
    {
        base.update();
        if (!isLookedByUser)
        {
            return;
        }
        m_countAction += Time.deltaTime;
        if (m_countAction < 5f)
        {
            StateBase<Hero> next = m_state.update(this);

            if (next != m_state)
            {
                m_state.exit(this);
                m_state = next;
                m_state.enter(this);
            }

        }else if (m_countAction > 7f)
        {
            m_countAction = 0f;
        }


        //ヒーローにしてほしい行動をここで記述
        want = wantAction.IDLE;
        Vector2 a = m_actorCtrl.position();
        Vector2 h = transform.position;
        if (a.x+2f < h.x)
        {
            want = wantAction.LEFT;
        }else if (a.x > h.x+2f)
        {
            want = wantAction.RIGHT;
        }
        else
        {
            if (a.y > h.y)
            {
                want = wantAction.JUMP;
            }
            else
            {
                want = wantAction.ATTACK;
            }
        }

        if (Random.Range(0, 400) == 0)
        {
            want = wantAction.JUMP;
        }

#if false
        switch (m_state)
        {
            case State.IDLE:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jump();
                    m_state = State.JUMP;
                    m_animCtrl.SetBool("isJumping", true);
                    break;
                }

                bool run = move();
                if (run)
                {
                    m_animCtrl.SetBool("isRunning", true);
                    m_state = State.RUN;
                }
                else
                {
                    m_animCtrl.SetBool("isRunning", false);
                }

                if (Input.GetKeyDown(KeyCode.B))
                {
                    attack();
                }

                break;

            case State.JUMP:
                move();
                if (m_isLanding)
                {
                    m_state = State.IDLE;
                    m_animCtrl.SetBool("isJumping", false);
                }
                break;

            case State.RUN:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jump();
                    m_state = State.JUMP;
                    m_animCtrl.SetBool("isJumping", true);
                    break;
                }

                bool ran = move();

                if(!ran)
                {
                    m_animCtrl.SetBool("isRunning", false);
                    m_state = State.IDLE;
                }

                if (Input.GetKeyDown(KeyCode.B))
                {
                    attack();
                }
                break;
        }
#endif

    }

    protected override void die()
    {
        m_goal.SetActive(true);
        m_goal.GetComponent<Goal>().explode();

        Instantiate(m_deadEffect, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
        //base.die();
    }
    public override void fire()
    {
        int length = m_fireAngle.Length;
        GameObject[] GOs = new GameObject[length];

        float scaleX = transform.localScale.x;
        Vector2 pos = new Vector2(transform.position.x + Mathf.Sign(scaleX) * m_bulletStartPos.x, transform.position.y + m_bulletStartPos.y);

        for (int j = 0; j < length; j++)
        {
            GOs[j] = Instantiate(m_bullet, pos, Quaternion.identity);
            GOs[j].transform.SetParent(transform);
            GOs[j].GetComponent<Bullet>().setLife(m_fireAngle[j]);
        }
    }

    public void attack()
    {
        m_animCtrl.SetTrigger("trgAttack");
    }

    public void jump()
    {
        Vector3 v = GetComponent<Rigidbody2D>().velocity;
        v.y = m_jumpForce;
        GetComponent<Rigidbody2D>().velocity = v;
        isLanding = false;
    }

    public bool move()
    {

        Vector3 mov = Vector3.zero;
        if (want == wantAction.LEFT)
        {
            mov.x--;
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (want == wantAction.RIGHT)
        {
            mov.x++;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        mov.x *= m_speed;


        transform.position += mov;
        
        if (mov != Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Stage":
                isLanding = true;
                break;
        }
    }
}
