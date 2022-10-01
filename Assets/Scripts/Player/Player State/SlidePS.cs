using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePS : PlayerState
{
    public SlidePS(Player _player) : base(_player)
    {
        name = PState.SLIDE;
    }


    public override void Enter()
    {
        SetTrigger("Slide");

        controller.Slide();
        controller.Speed = att.SlideSpeed;
        controller.SideSpeed = Vector3.Dot(controller.Velocity.normalized * att.SlideSpeed, Vector3.right);

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (Time.time > startTime + UD.slideTime)
        {
            // Sprint
            if (acc > 0 && controller.CanAccelerate)
                nextState = new SprintPS(player);
            // Siderun
            else if (acc == 0 && side != 0)
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true);
            // Slowsiderun
            else if (acc < 0 && side != 0)
                nextState = new SlowsiderunPS(player, side / Mathf.Abs(side));
            // Slow
            else if (acc < 0)
                nextState = new SlowrunPS(player);
            // Run
            else
                nextState = new RunPS(player);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Slide");

        base.Exit();
    }
}
