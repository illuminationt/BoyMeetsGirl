using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : Behaviour {

    public void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 0.0f);
    }

    public void move()
    {

    }






    private void OnTriggerEnter2D(Collider2D col)
{
        string tag = col.gameObject.tag;
        switch (tag)
        {
            case "Stage":
                Vector2 v = GetComponent<Rigidbody2D>().velocity;
                Vector3 s = transform.localScale;
                v.x *= -1.0f;
                s.x *= -1.0f;
                GetComponent<Rigidbody2D>().velocity = v;
                transform.localScale = s;
                break;

            case "Bullet":
                Destroy(col.gameObject);
                Destroy(this.gameObject);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        string tag = col.gameObject.tag;

        switch (tag)
        {
            

        }
    }
}
