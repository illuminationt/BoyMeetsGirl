using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor
{
   

    protected class IdleState : ActorState
    {
        public override void enter(Actor actor)
        {
            actor.m_animCtrl.SetBool("isJumping", false);
        }

        public override ActorState update(Actor actor)
        {
            ActorState next = this;

            actor.moveHorizontal();
            if (Input.GetButton("fire"))
            {
                actor.fire();
            }


            if (Input.GetButtonDown("jump"))
            {
                actor.jump();
                next = new OnAirCanJumpState();
            }
            else if (!actor.isLanding)
            {
                next = new OnAirCanJumpState();
            }

            return next;
        }
    }

    class OnAirCanJumpState : ActorState
    {
        public override void enter(Actor actor)
        {
            actor.m_animCtrl.SetBool("isJumping", true);
        }

        public override ActorState update(Actor actor)
        {
             ActorState next = this;
             actor.moveHorizontal();

            if (Input.GetButton("fire"))
            {
                actor.fire();
            }

            Vector2 v = actor.GetComponent<Rigidbody2D>().velocity;
            if (Input.GetButtonDown("jump"))
            {
                actor.jump();
                next = new OnAirCannotJumpState();
            }


            else if (actor.isLanding)
            {
                next = new IdleState();
            }


            return next;
        }
    }

    class OnAirCannotJumpState : ActorState
    {
        public override void enter(Actor actor)
        {
            //Debug.Log("enter OnAirCannotJumpState");
            //int hash = actor.m_animCt
            actor.m_animCtrl.Play("Boson_Jump", 0, 0f);
        }

        public override ActorState update(Actor actor)
        {
            ActorState next = this;

            actor.moveHorizontal();
            if (Input.GetButton("fire"))
            {
                actor.fire();
            }

            Vector2 v = actor.GetComponent<Rigidbody2D>().velocity;
            if (actor.isLanding)
            {
                next = new IdleState();
            }



            return next;
        }
    }

}
