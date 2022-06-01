using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukePS : PlayerState
{
    public JukePS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.JUKE;

        startSide = _side;
    }

    public override void Enter()
    {
        animator.SetTrigger("Juke");
        animator.SetFloat("Dir", startSide);
        animTime = controller.jukeTime;

        controller.Speed = controller.JukeSpeed;
        controller.SideSpeed = controller.JukeSideSpeed * startSide;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (Input.GetAxisRaw("Horizontal") * startSide < 0 && acc == 0)
            nextState = new SpinPS(controller, animator, Input.GetAxisRaw("Horizontal"));

        if (Time.time >= startTime + animTime)
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
                    nextState = new SlowsiderunPS(controller, animator, side / Mathf.Abs(side));
                }
                // Siderun
                else if (acc == 0 && side * startSide > 0)
                {
                    nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side), false);
                }
                // Slowrun
                else if (acc < 0)
                {
                    nextState = new SlowrunPS(controller, animator);
                }
                // Run
                else nextState = new RunPS(controller, animator);
            }
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("Juke");

        base.Exit();
    }
}
