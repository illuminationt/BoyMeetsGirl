using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fermion : Actor {

    [SerializeField] private float m_floatDistance = 2.0f;
    [SerializeField] private float m_ceilingBack = 1.2f;
    [SerializeField] private GameObject m_fermionShadow = null;
    [SerializeField] private float m_gravity = 2f;


    private Light m_light = null;
    public int childFermionNumber { get; protected set; }

	protected override void Start()
    {
        base.Start();
        isUtilized = true;
        m_light = GetComponentInChildren<Light>();
        childFermionNumber = 0;
    }

    // Update is called once per frame
    public override void update(float timeScale=1f)
    {
        base.update();

        moveHorizontal();
        moveVertical(isInPlasma);

        processAvoid();
        if (Input.GetButton("fire"))
        {
            fire();
        }

        if (isAvoiding)
        {
            m_light.gameObject.SetActive(false);
        }
        else
        {
            m_light.gameObject.SetActive(true);
        }

        m_fermionShadow.gameObject.SetActive(true);

    }

    public override void notUtilizedUpdate(Actor other)
    {
        base.notUtilizedUpdate(other);
        m_fermionShadow.SetActive(false);
    }

    protected void moveVertical(bool isConduct)
    {
        Vector2 dv = Vector2.zero;
        if (Input.GetKey(KeyCode.I))
        {
            dv.y++;
        }else if (Input.GetKey(KeyCode.K))
        {
            dv.y--;
        }

        float lsv = Input.GetAxis("L_Stick_V");
        dv.y -= lsv;

        Vector2 v = GetComponent<Rigidbody2D>().velocity;
        

        //浮遊コード
        Vector2 start = transform.position;
        Vector2 dir = -transform.up;
        int layer = LayerMask.GetMask("Stage");
        RaycastHit2D hitInfo = Physics2D.Raycast(start, dir, Mathf.Infinity, layer);
        if (hitInfo.collider == null&&!isConduct)
        {
            //地面が無い場合
            dv.y -= m_gravity;
            v.y = dv.y;
            GetComponent<Rigidbody2D>().velocity= v;
            m_fermionShadow.transform.position = new Vector3(0f, 0f, 10f);
            return;
        }


        Vector2 ceiling = hitInfo.point + new Vector2(0f, m_floatDistance);
        dv.y *= uniqueMaxSpeed();
        if (isConduct)
        {
            ceiling.y = Mathf.Infinity / 100f;
        }
        if (transform.position.y > ceiling.y * m_ceilingBack)
        {
            dv.y = -uniqueMaxSpeed();
        }
        else if (transform.position.y > ceiling.y)
        {
            if (dv.y > 0f)
            {
                dv.y = 0f;
            }
        }

        dv.x = GetComponent<Rigidbody2D>().velocity.x;
        
        GetComponent<Rigidbody2D>().velocity = dv;

        m_fermionShadow.transform.position = hitInfo.point;
    }

    public void addChildFermion()
    {
        childFermionNumber++;
    }

    

    protected override void OnTriggerExit2D(Collider2D col)
    {
        base.OnTriggerExit2D(col);

        switch (col.gameObject.tag)
        {
            
        }
    }
}


