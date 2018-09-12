using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : LifeBase {

    //このEnemyのHPゲージが画面に表示されてるか？
    public  bool isHPgaugeShown { get; set; }
    public bool isDamaged { get; private set; }
    public bool isLookedByUser { get; set; }

    protected Actor m_actor;
    protected Animator m_animCtrl = null;
    private bool m_wasDamaged;
    protected override void Start()
    {
        base.Start();
        //堂々と使う
        m_actor = FindObjectOfType<Actor>();
        isHPgaugeShown = false;
        isLookedByUser = false;
        m_animCtrl = GetComponent<Animator>();
    }


    public override void update(float timeScale=1f)
    {
        base.update();

        if (!isLookedByUser)
        {
            if (GetComponent<SpriteRenderer>().isVisible)
            {
                isLookedByUser = true;
            }
        }

        m_animCtrl.speed = timeScale;
       
        if (!isLookedByUser)
        {
            return;
        }

        if (TransitionManager.Instance.isSalvated)
        {
            isLookedByUser = false;
        }
    }

    protected void LateUpdate()
    {
        m_wasDamaged = isDamaged;
        isDamaged = false;
    }

    public bool wasDamaged()
    {
        if (!m_wasDamaged && isDamaged)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void die()
    {
        base.die();
        m_wasDamaged = false;
        isDamaged = false;
        Destroy(this.gameObject);
    }

    public override void fire()
    {
        if (!isLookedByUser)
        {
            return;
        }
        base.fire();
    }

    protected override void damage(GameObject col)
    {
        if (!isLookedByUser)
        {
            return;
        }
        isDamaged = true;
        base.damage(col);
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        switch (tag)
        {
            case "Bullet":
                damage(col.gameObject);
                
                col.GetComponent<Bullet>().explode();

                break;
        }
    }
    

}
