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
        animator.SetTrigger("Slow");

        controller.SideSpeed = 0;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        controller.Speed = controller.NormalSpeed * ( 1 + (1 - controller.SlowM) * acc );


        // Juke
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            nextState = new JukePS(controller, animator, Input.GetAxisRaw("Horizontal"));
            stage = Event.EXIT;
        }
        // Jump
        else if (Input.GetKeyDown(KeyCode.Space))
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

    public override void Exit()
    {
        animator.ResetTrigger("Slow");

        base.Exit();
    }
}
