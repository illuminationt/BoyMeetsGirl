using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skelton : EnemyBase{

    [SerializeField] private float m_speed;

    protected override void Start()
    {
        base.Start();
        m_countFire = 0f;
        float s = transform.localScale.x;
        GetComponent<Rigidbody2D>().velocity = -transform.right * m_speed;
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

        if (timeScale != 1f)
        {
            Vector2 v = GetComponent<Rigidbody2D>().velocity;
            v.x = Mathf.Sign(v.x) * m_speed * timeScale;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

        switch (col.gameObject.tag)
        {
            case "Stage":
                Vector3 s = transform.localScale;
                Vector3 v = GetComponent<Rigidbody2D>().velocity;
                s.x *= -1f;

                transform.localScale = s;
                if (v.x * s.x > 0f)
                {
                    GetComponent<Rigidbody2D>().velocity = v;
                }
                else
                {
                    v.x *= -1f;
                    GetComponent<Rigidbody2D>().velocity = v;
                }
                break;
        }
    }
}
