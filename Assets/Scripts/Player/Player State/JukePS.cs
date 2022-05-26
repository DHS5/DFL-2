using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukePS : PlayerState
{
    private float startSide;
    private float startTime;
    private float animTime;

    public JukePS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.JUKE;

        startSide = _side;
    }

    public override void Enter()
    {
        // anim
        //animTime = Animation.time;

        startTime = Time.time;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        controller.Speed /= 2;

        controller.SideSpeed = controller.JukeSideSpeed * startSide;


        //if (side * startSide < 0 && acc == 0)
        //    nextState = new SpinPS(controller, animator, side / Mathf.Abs(side));

        if (Time.time > startTime + animTime)
        {
            if (nextState == null)
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
                // SlowSiderun
                else if (acc < 0 && side * startSide > 0)
                {
                    //nextState = new SlowsiderunPS(controller, animator, side / Mathf.Abs(side));
                }
                // Slowrun
                else if (acc < 0)
                {
                    nextState = new SlowrunPS(controller, animator);
                }
                nextState = new RunPS(controller, animator);
            }
            stage = Event.EXIT;
        }
    }
}
