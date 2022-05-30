using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiderunPS : PlayerState
{
    private bool anim;

    public SiderunPS(PlayerController _controller, Animator _animator, float _side, bool _anim) : base(_controller, _animator)
    {
        name = PState.SIDERUN;

        startSide = _side;
        anim = _anim;
    }


    public override void Enter()
    {
        animator.SetFloat("Dir", startSide);
        if (anim)
        {
            animator.SetTrigger("Side");
            animTime = 0.4f;
        }
        //Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

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
        else if (anim && Input.GetAxisRaw("Horizontal") * startSide < 0 && acc == 0)
        {
            nextState = new FeintPS(controller, animator, Input.GetAxisRaw("Horizontal"));
            animator.SetTrigger("Feint");
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("Side");

        base.Exit();
    }
}
