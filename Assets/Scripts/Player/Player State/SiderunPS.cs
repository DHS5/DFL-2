using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiderunPS : PlayerState
{    
    public SiderunPS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.SIDERUN;

        startSide = _side;
    }


    public override void Enter()
    {
        // anim

        controller.Speed = controller.NormalSpeed;

        base.Enter();
    }


    public override void Update()
    {
        base.Update();


        controller.SideSpeed = controller.NormalSideSpeed * side;


        if (Time.time >= startTime + animTime)
        {
            if (nextState == null)
            {
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
                else if (acc < 0)
                {
                    nextState = new SlowsiderunPS(controller, animator, side / Mathf.Abs(side));
                    stage = Event.EXIT;
                }
                // Run
                else if (side == 0)
                {
                    nextState = new RunPS(controller, animator);
                    stage = Event.EXIT;
                }
            }
            else stage = Event.EXIT;
        }
        else if (Input.GetAxisRaw("Horizontal") * startSide < 0 && acc == 0)
            nextState = new FeintPS(controller, animator, Input.GetAxisRaw("Horizontal"));
    }
}
