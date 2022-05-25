using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowrunPS : PlayerState
{
    public SlowrunPS(PlayerController _controller, Animator _animator) : base(_controller, _animator)
    {
        name = PState.SLOWRUN;
    }


    public override void Enter()
    {
        // anim

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        controller.Speed = controller.NormalSpeed * acc * controller.SlowM;

        controller.SideSpeed = controller.SlowSideSpeed * side;


        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new JumpPS(controller, animator);
            stage = Event.EXIT;
        }
        // Sprint
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(controller, animator);
            stage = Event.EXIT;
        }
        // Run
        else if (acc == 0)
        {
            nextState = new RunPS(controller, animator);
            stage = Event.EXIT;
        }
    }
}
