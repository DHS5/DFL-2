using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintPS : PlayerState
{
    public SprintPS(PlayerController _controller, Animator _animator) : base(_controller, _animator)
    {
        name = PState.SPRINT;
    }


    public override void Enter()
    {
        // anim

        controller.Sprint();

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        controller.Speed = controller.NormalSpeed * (1 - (1 - controller.AccelerationM) * acc);

        controller.SideSpeed = controller.AccSideSpeed * side;

        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new JumpPS(controller, animator);
            stage = Event.EXIT;
        }
        // Run
        else if ((acc == 0 && side == 0) || (!controller.CanAccelerate && side == 0))
        {
            nextState = new RunPS(controller, animator);
            stage = Event.EXIT;
        }
        // Siderun
        else if ((acc == 0 && side != 0) || (!controller.CanAccelerate && side != 0))
        {
            nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side));
            stage = Event.EXIT;
        }
    }
}
