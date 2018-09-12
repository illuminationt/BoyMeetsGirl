using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBase : MonoBehaviour {

    [SerializeField] protected GameObject m_bullet = null;
    [SerializeField] protected GameObject m_deadEffect = null;
    [SerializeField] protected Vector2 m_bulletStartPos;
    [SerializeField] protected float m_fireInterval = 1.2f;
    [SerializeField] protected float[] m_fireAngle;

    [SerializeField] protected int m_maxHP = 100;
    [SerializeField] protected int m_weakWeaponDamage = 5;
    [SerializeField] protected int m_toleranceWeaponDamage = 1;
    [SerializeField] protected int m_nietherWeaponDamage = 2;
    [SerializeField] protected Weak m_weak = Weak.NIETHER;
    [SerializeField] protected Color32 m_damagedColor = Color.red;

    [SerializeField] private float m_damageEffectParam = 1f;
    [SerializeField] private float m_damageEffectDuration = 0.2f;
    public int HP { get { return m_HP; } }
    public int maxHP { get { return m_maxHP; } }

    private bool m_isDie;
    

    //弱点となる弾丸
    protected enum Weak
    {
        FIRE,
        ICE,
        NIETHER,
    }
    

    protected float m_countFire = 0f;
    private float m_countDamage = 0f;
    private bool m_wasRecieveDamage = false;
    protected int m_HP;
    protected AudioSource m_audioSource = null;

    protected virtual void Start()
    {
        m_HP = m_maxHP;
        m_isDie = false;
        m_audioSource = GetComponent<AudioSource>();
    }
    public virtual void update(float timeScale=1f)
    {
        processDamageEffect();

        if (m_HP <= 0f&&!m_isDie)
        {
            m_isDie = true;
            die();
        }
    }


    public virtual void fire()
    {
        if (m_countFire > m_fireInterval)
        {

            int length = m_fireAngle.Length;
            GameObject[] GOs = new GameObject[length];

            float scaleX = transform.localScale.x;
            Vector2 pos = new Vector2(transform.position.x + Mathf.Sign(scaleX) * m_bulletStartPos.x, transform.position.y+m_bulletStartPos.y);

            for (int j = 0; j < length; j++)
            {
                GOs[j] = Instantiate(m_bullet, pos, Quaternion.identity);
                GOs[j].transform.SetParent(transform);
                GOs[j].GetComponent<Bullet>().setLife(m_fireAngle[j]);
            }


            if (m_audioSource != null)
            {
                m_audioSource.PlayOneShot(m_audioSource.clip);
            }
            m_countFire = 0f;
        }
    }


    //OnTrigger～関数内で呼び出す
    protected virtual void damage(GameObject col)
    {
        Bullet b = col.gameObject.GetComponent<Bullet>();
        Bullet.Type type;
        if (b == null)
        {
            type = Bullet.Type.PLASMA;
        }
        else { type = b.bulletType; }

        StartCoroutine(damageRed(m_damageEffectDuration));

        m_wasRecieveDamage = true;
        switch (m_weak)
        {
            case Weak.FIRE:
                switch (type)
                {
                    case Bullet.Type.FIRE:
                        m_HP -= m_weakWeaponDamage;
                        //effectFireDamage();
                        break;
                    case Bullet.Type.ICE:
                        m_HP -= m_toleranceWeaponDamage;
                        //effectIceDamage();
                        break;
                    case Bullet.Type.PLASMA:
                        m_HP -= m_nietherWeaponDamage;
                        break;
                }
                break;

            case Weak.ICE:
                switch (type)
                {
                    case Bullet.Type.FIRE:
                        m_HP -= m_toleranceWeaponDamage;
                       // effectFireDamage();
                        break;
                    case Bullet.Type.ICE:
                        m_HP -= m_weakWeaponDamage;
                        // effectIceDamage();
                        break;
                    case Bullet.Type.PLASMA:
                        m_HP -= m_nietherWeaponDamage;
                        break;
                }
                break;

            case Weak.NIETHER:
                m_HP -= m_nietherWeaponDamage;
               // effectNietherDamage();
                break;
        }
    }
    protected virtual void damage()
    {
        m_HP -= m_nietherWeaponDamage;
    }
    protected void processDamageEffect()
    {
        
        
    }

    protected virtual void die()
    {
        Instantiate(m_deadEffect, transform.position, Quaternion.identity);
    }

    protected IEnumerator damageRed(float duration)
    {
        GetComponent<SpriteRenderer>().color = m_damagedColor;

        yield return new WaitForSeconds(duration);

        GetComponent<SpriteRenderer>().color = Color.white;

    }
}

