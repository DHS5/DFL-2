using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPS : PlayerState
{
    public RunPS(Player _player) : base(_player)
    {
        name = PState.RUN;
    }

    public override void Enter()
    {
        SetTrigger("Run");
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
        if (Input.GetKeyDown(KeyCode.Space))
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
            nextState = new SiderunPS(player, side / Mathf.Abs(side), true);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Run");
        
        base.Exit();
    }
}
