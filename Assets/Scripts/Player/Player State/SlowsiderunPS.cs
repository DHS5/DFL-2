using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowsiderunPS : PlayerState
{
    public SlowsiderunPS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.SLOWSIDERUN;

        startSide = _side;
    }

    public override void Enter()
    {
        controller.Speed = controller.NormalSpeed;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


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
        // Slow
        else if (side == 0)
        {
            nextState = new SlowrunPS(controller, animator);
            stage = Event.EXIT;
        }
        // Siderun
        else if (acc == 0 && side != 0)
        {
            nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side), true);
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
