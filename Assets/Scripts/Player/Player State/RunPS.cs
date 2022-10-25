using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPS : PlayerState
{
    private bool anim;
    public RunPS(Player _player, bool _anim) : base(_player)
    {
        name = PState.RUN;

        anim = _anim;
    }

    public override void Enter()
    {
        if (anim) SetTrigger("Run");
        SetFloat("Dir", 0f);

        controller.Speed = att.NormalSpeed;
        controller.SideSpeed = 0f;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && controller.CanJump(att.JumpCost))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }
        // Acceleration
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(player);
            stage = Event.EXIT;
        }
        // Slow
        else if (acc < 0)
        {
            nextState = new SlowrunPS(player);
            stage = Event.EXIT;
        }
        // Siderun
        else if (side != 0)
        {
            nextState = new SiderunPS(player, side / Mathf.Abs(side), false, true);// Time.time >= startTime + UD.siderunDelay);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Run");
        
        base.Exit();
    }
}
