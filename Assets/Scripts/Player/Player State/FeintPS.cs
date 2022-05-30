using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeintPS : PlayerState
{
    public FeintPS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.FEINT;

        startSide = _side;
    }

    public override void Enter()
    {
        animTime = 0.1f;

        controller.Speed = controller.FeintSpeed;
        controller.SideSpeed = controller.FeintSideSpeed * startSide;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();        


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
            // Siderun
            else if (side != 0 && side * startSide > 0)
            {
                nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side), false);
                stage = Event.EXIT;
            }
            // Run
            else nextState = new RunPS(controller, animator);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("Feint");

        base.Exit();
    }
}
