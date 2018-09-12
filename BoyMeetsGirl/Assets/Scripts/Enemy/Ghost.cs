using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyBase {



    protected override void Start()
    {
        base.Start();
        m_countFire = 0f;
    }

    public override void update(float timeScale=1f)
    {
        base.update(timeScale);
        m_countFire += Time.deltaTime * timeScale;
        if (m_countFire > m_fireInterval)
        {
            fire();
            m_countFire = 0f;
        }

        Vector3 s = transform.localScale;
        if (transform.position.x < m_actor.transform.position.x)
        {
           
            s.x = Mathf.Abs(s.x);
            
        }
        else
        {
            s.x = -Mathf.Abs(s.x);
        }
        transform.localScale = s;
    }

    /*
    protected void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        switch (tag)
        {
            case "Bullet":
                damage(col);
                break;
        }
    }
    */
}
