using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPS : PlayerState
{
    public SpinPS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.SPIN;

        startSide = _side;
    }

    public override void Enter()
    {
        animator.SetTrigger("Spin");
        animTime = controller.spinTime;

        controller.Speed = controller.SpinSpeed;
        controller.SideSpeed = controller.SpinSideSpeed * startSide;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (Time.time >= startTime + animTime)
        {
            // Acceleration
            if (acc > 0 && controller.CanAccelerate)
            {
                nextState = new SprintPS(controller, animator);
            }
            // Slowrun
            else if (acc < 0)
            {
                nextState = new SlowrunPS(controller, animator);
            }
            // Siderun
            else if (side != 0)
            {
                nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side), false);
            }
            // Run
            else nextState = new RunPS(controller, animator);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("Spin");

        base.Exit();
    }
}
