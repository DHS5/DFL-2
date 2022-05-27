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
        // anim
        //animTime = Animation.time;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        controller.Speed /= controller.SpinFSpeedD;

        controller.SideSpeed = controller.SpinSideSpeed * startSide;


        if (Time.time >= startTime + animTime)
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nextState = new JumpPS(controller, animator);
            }
            // Acceleration
            else if (acc > 0 && controller.CanAccelerate)
            {
                nextState = new SprintPS(controller, animator);
            }
            // Slowrun
            else if (acc < 0)
            {
                nextState = new SlowrunPS(controller, animator);
            }
            // Run
            else nextState = new RunPS(controller, animator);

            stage = Event.EXIT;
        }
    }
}
