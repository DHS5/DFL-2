using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiderunPS : PlayerState
{
    private bool anim;

    public SiderunPS(Player _player, float _side, bool _anim) : base(_player)
    {
        name = PState.SIDERUN;

        startSide = _side;
        anim = _anim;
    }


    public override void Enter()
    {
        SetFloat("Dir", startSide);
        if (anim)
        {
            SetTrigger("Side");
            animTime = controller.siderunTime;
        }
        else SetTrigger("Run");

        controller.Speed = controller.NormalSpeed;
        controller.SideSpeed = controller.NormalSideSpeed * side;

        base.Enter();
    }


    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.SideSpeed = controller.NormalSideSpeed * side;


        if (Time.time >= startTime + animTime)
        {
            if (nextState == null)
            {
                // Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    nextState = new JumpPS(player);
                    stage = Event.EXIT;
                }
                // Sprint
                else if (acc > 0 && controller.CanAccelerate)
                {
                    nextState = new SprintPS(player);
                    stage = Event.EXIT;
                }
                // Slowsiderun
                else if (acc < 0 && side * startSide > 0)
                {
                    nextState = new SlowsiderunPS(player, side / Mathf.Abs(side));
                    stage = Event.EXIT;
                }
                // Slow
                else if (acc < 0 && side == 0)
                {
                    nextState = new SlowrunPS(player);
                    stage = Event.EXIT;
                }
                // Other side
                else if (side * startSide < 0)
                {
                    nextState = new SiderunPS(player, -startSide, false);
                    stage = Event.EXIT;
                }
                // Run
                else if (side == 0)
                {
                    nextState = new RunPS(player);
                    stage = Event.EXIT;
                }
            }
            else stage = Event.EXIT;
        }
        else if (anim && Input.GetAxisRaw("Horizontal") * startSide < 0 && acc == 0)
        {
            nextState = new FeintPS(player, -startSide);
            SetFloat("Dir", -startSide);
            SetTrigger("Feint");
        }
    }

    public override void Exit()
    {
        ResetTrigger("Side");
        ResetTrigger("Run");

        base.Exit();
    }
}
