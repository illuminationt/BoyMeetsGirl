using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private Type m_bulletType = Type.FIRE;
    [SerializeField] private float m_lifeTime = 5.0f;
    [SerializeField] private GameObject m_explosionParticle = null;
    
    private float m_count = 0f;

    public Type bulletType
    {
        get { return m_bulletType; }
    }

    public enum Type
    {
        FIRE,
        ICE,
        PLASMA,
    }

    private void Start()
    {

        m_count = 0f;
    }

    private void Update()
    {
        m_count += Time.deltaTime;

        if (m_count > m_lifeTime)
        {
            Destroy(this.gameObject);
        }

        if (this.gameObject.tag == "EnemyBullet")
        {
            Vector2 dir = GetComponent<Rigidbody2D>().velocity.normalized;
            if (StageManager.Instance.isTimeStop)
            {
                GetComponent<Rigidbody2D>().velocity = dir * m_speed * StageManager.Instance.timeDelay;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = dir * m_speed;
            }
        }
    }

    
    //これが呼ばれる時点でLifeBaseの子になってる
    public void setLife(float angleZ)
    {
        if (transform.parent == null)
        {
            Debug.LogError(this.name + "dont have parent");
        }

        float parentScaleX = transform.parent.transform.localScale.x;
        Vector2 parentVel = transform.parent.GetComponent<Rigidbody2D>().velocity;

        Vector2 dir = new Vector2(Mathf.Cos(angleZ * Mathf.Deg2Rad), Mathf.Sin(angleZ * Mathf.Deg2Rad));
        dir.x *= Mathf.Sign(parentScaleX);

        Vector2 v = dir * m_speed;
        v.x += parentVel.x;
        GetComponent<Rigidbody2D>().velocity = v;

        if (v.x < 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            angleZ *= -1f;
        }

        Vector3 rot = transform.eulerAngles;
        rot.z = angleZ;
        transform.eulerAngles = rot;

        //親から生命を受け取ったら親離れ
        transform.parent = null;
    }

    public void explode()
    {
        Instantiate(m_explosionParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;

        switch (tag)
        {
            case "Stage":
                explode();
                break;
        }
    }
}
