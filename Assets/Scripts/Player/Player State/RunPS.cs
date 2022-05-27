using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPS : PlayerState
{
    public RunPS(PlayerController _controller, Animator _animator) : base(_controller, _animator)
    {
        name = PState.RUN;
    }

    public override void Enter()
    {
        // set anim run

        controller.Speed = controller.NormalSpeed;
        controller.FSpeed = controller.NormalSpeed;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new JumpPS(controller, animator);
            stage = Event.EXIT;
        }
        // Acceleration
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(controller, animator);
            stage = Event.EXIT;
        }
        // Slow
        else if (acc < 0)
        {
            nextState = new SlowrunPS(controller, animator);
            stage = Event.EXIT;
        }
        // Siderun
        else if (side != 0)
        {
            nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side));
            stage = Event.EXIT;
        }
    }
}
