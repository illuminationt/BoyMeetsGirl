using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor : LifeBase {

    [SerializeField] protected float        m_uniqueMaxSpeed = 10.0f;
    [SerializeField] private float        m_frictionRate = 0.9f;
    [SerializeField] protected float      m_jumpForce=5.0f;
    [SerializeField] protected Vector3    m_pairPosition;
    [SerializeField] protected float      m_avoidDuration = 1.0f;
    [SerializeField] protected Color32    m_avoidColor;
    [SerializeField] protected Color32    m_notUsedColor;
    

    [SerializeField] private float m_lerpRate = 1.0f;

    public Vector3 pairPosition {
        
        get {
            Vector3 p = new Vector3(Mathf.Sign(transform.localScale.x) * m_pairPosition.x, m_pairPosition.y,0f);
            return transform.position + p;
        }
    }

    protected Animator m_animCtrl = null;
    
    
    protected float m_countAvoid = 0.0f;

    public bool isLanding { get; protected set; }
    public bool isAvoiding { get; protected set; }
    public bool isUtilized { get; set; }
    public float trulyUsedMaxSpeed { protected get; set; }
    public float uniqueMaxSpeed() { return m_uniqueMaxSpeed; }
    public bool isAlive { get; protected set; }
    protected bool isInPlasma { get; private set; }
    public bool isGoal { get; protected set; }

    protected override void Start()
    {
        base.Start();
        m_animCtrl = GetComponent<Animator>();
        isAlive = true;
        isGoal = false;
    }

    public override void update(float timeScale=1f)
    {
        if (!isAlive)
        {
            return;
        }
        base.update();

        m_countFire += Time.deltaTime;
        m_countAvoid += Time.deltaTime;

        processAvoid();

        if (TransitionManager.Instance.isSalvated)
        {
            m_HP = m_maxHP;
        }

    }

    public virtual void notUtilizedUpdate(Actor other)
    {
        Vector2 goal = other.pairPosition;
        Vector2 start = transform.position;

        Vector2 d = goal - start;
        d *= m_lerpRate;
        GetComponent<Rigidbody2D>().velocity = d;

        //向き変え
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(other.transform.localScale.x);
        transform.localScale = scale;

        GetComponent<SpriteRenderer>().color = m_notUsedColor;

        if (isAlive)
        {
            if (TransitionManager.Instance.isSalvated)
            {
                m_HP = m_maxHP;
            }
        }
    }
    protected abstract class ActorState
    {
        ~ActorState() { }
        public virtual void enter(Actor actor) { }
        public virtual void exit(Actor actor) { }
        public abstract ActorState update(Actor actor);
    }

    public override void fire() {
        if (isAvoiding)
        {
            return;
        }

        if (!isUtilized)
        {
            return;
        }

        base.fire();

    }


    public virtual void jump() {
        Vector2 v = GetComponent<Rigidbody2D>().velocity;
        v.y = m_jumpForce;
        GetComponent<Rigidbody2D>().velocity = v;
    }
    protected virtual void moveHorizontal() {
        Vector2 dv = Vector3.zero;
        if (Input.GetKey(KeyCode.J))
        {
            dv.x--;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            dv.x++;
        }

        float lsh = Input.GetAxis("L_Stick_H");
        dv.x += lsh;

        GetComponent<Rigidbody2D>().velocity += dv;
        Vector2 v = GetComponent<Rigidbody2D>().velocity;

        if (v.x >= trulyUsedMaxSpeed)
        {
            v.x = trulyUsedMaxSpeed;
        }
        else if (v.x <= -trulyUsedMaxSpeed)
        {
            v.x = -trulyUsedMaxSpeed;
        }
        if (dv.x == 0.0f)
        {
            v.x *= m_frictionRate;
        }
        GetComponent<Rigidbody2D>().velocity = v;

        //向き変え
        if (dv.x != 0.0f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * Mathf.Sign(v.x);
           transform.localScale = s;
        }
    }
    protected virtual void processAvoid()
    {
        Color32 color = GetComponent<SpriteRenderer>().color;

        if (m_countAvoid > m_avoidDuration&&isAvoiding)
        {
            isAvoiding = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (isAvoiding)
        {
            color = m_avoidColor; 
            return;
        }



        if (Input.GetButtonDown("transparent"))
        {
            isAvoiding = true;
            m_countAvoid = 0.0f;
            GetComponent<SpriteRenderer>().color = m_avoidColor;

        }
    }

    public void setWhiteColor() { GetComponent<SpriteRenderer>().color = Color.white; }
    public void setNotUsedColor() { GetComponent<SpriteRenderer>().color = m_notUsedColor; }

    protected override void damage(GameObject col)
    {
        base.damage(col);
    }

    protected override void die()
    {
        base.die();
        this.gameObject.SetActive(false);
        isAlive = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        switch (tag)
        {
            case "Stage":
                isLanding = true;
                break;

            case "EnemyBullet":
                if (isAvoiding)
                {
                    return;
                }
                damage(col.gameObject);
                col.gameObject.GetComponent<Bullet>().explode();
                break;

            case "Sword":
                if (isAvoiding)
                {
                    return;
                }
                if (!isUtilized)
                {
                    return;
                }
                damage(col.gameObject);
                break;

            case "Goal":
                isGoal = true;
                break;

            case "Enemy":
                if (isAvoiding)
                {
                    return;
                }
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                damage(col.gameObject);
                break;

            case "DeathArea":
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (!TransitionManager.Instance.isSalvated)
                {
                    TransitionManager.Instance.salvate(0.5f, 0.5f);
                }
                break;

            case "Needle":
                if (isAvoiding || !isUtilized)
                {
                    break;
                }

                    int stageNo = TransitionManager.Instance.stageNo;
                    string s = "scene_stage0" + stageNo;
                    TransitionManager.Instance.transite(s, 0.5f, 0.2f);
                break;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Plasma":
                isInPlasma = true;
                break;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {

        string tag = col.gameObject.tag;
        switch (tag)
        {
            case "Stage":
                
                    isLanding = false;
                
                break;
            case "Plasma":
                isInPlasma = false;
                break;
        }


    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            
        }
    }
}
