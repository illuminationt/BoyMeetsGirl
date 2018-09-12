using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle:StateBase<Hero> {

    public override void enter(Hero owner)
    {
        Debug.Log("hero : idle");
    }

    public override StateBase<Hero> update(Hero hero)
    {
        StateBase<Hero> next = this;

        if (hero.want==Hero.wantAction.JUMP)
        {
            hero.jump();
            hero.animCtrl.SetBool("isJumping", true);
            hero.fire();
            next = new StateJump();
        }

        bool run = hero.move();
        if (run)
        {
            hero.animCtrl.SetBool("isRunning", true);
            next = new StateRun();
        }


        if (hero.want == Hero.wantAction.ATTACK)
        {
            hero.attack();
        }

        return next;
    }


}

public class StateRun : StateBase<Hero>
{
    public override void enter(Hero owner)
    {
        Debug.Log("hero : run");
    }

    public override StateBase<Hero> update(Hero hero)
    {
        StateBase<Hero> next = this;

        if (hero.want==Hero.wantAction.JUMP)
        {
            hero.jump();
            hero.animCtrl.SetBool("isJumping", true);
            next = new StateJump();
            hero.fire();
            return next;
        }

        bool ran = hero.move();

        if (!ran)
        {
            hero.animCtrl.SetBool("isRunning", false);
            next = new StateIdle();
        }

        if (hero.want == Hero.wantAction.ATTACK)
        {
            hero.attack();
        }

        return next;
    }
}

public class StateJump : StateBase<Hero>
{
    public override void enter(Hero owner)
    {
        Debug.Log("hero : jump");
    }

    public override StateBase<Hero> update(Hero hero)
    {
        StateBase<Hero> next = this;

        hero.move();
        if (hero.isLanding)
        {
            next = new StateIdle();
            hero.animCtrl.SetBool("isJumping", false);
        }

        if (hero.want == Hero.wantAction.ATTACK)
        {
            hero.attack();
        }

        return next;
    }
}