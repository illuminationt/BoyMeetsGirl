using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boson : Actor {

    private float m_count;
    private ActorState m_actorState = null;

    

    protected override void Start()
    {
        base.Start();
        isUtilized = false;
        m_count = 0f;
        m_actorState = new IdleState();
    }

    public override void notUtilizedUpdate(Actor other)
    {
        base.notUtilizedUpdate(other);

        m_count += Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, 0f, m_count*120f);
    }

    public override void update(float timeScale=1f)
    {
        base.update();
        Actor a = GetComponent<Actor>();
        ActorState next = m_actorState.update(a);

        if (next != m_actorState)
        {
            m_actorState.exit(this);
            m_actorState = next;
            m_actorState.enter(this);
        }
        transform.eulerAngles = Vector3.zero;

        if (isInPlasma)
        {
            if (!isAvoiding)
            {
                damage();
            }
        }

        Vector2 v = (GetComponent<Rigidbody2D>().velocity);
        if (Mathf.Abs(v.x) > m_uniqueMaxSpeed * 0.2f)
        {
            m_animCtrl.SetBool("isRunning", true);
        }
        else
        {
            m_animCtrl.SetBool("isRunning", false);
        }

        if (Mathf.Abs(v.y) < 0.05f)
        {
            m_animCtrl.SetBool("isJumping", false);
        }
        else
        {
            m_animCtrl.SetBool("isJumping", true);
        }

/*
        if (!StageManager.Instance.isTimeStop)
        {
            if (Input.GetButtonDown("special"))
            {
                StageManager.Instance.isTimeStop = true;
            }
        }
        */


    }

    
    
}
