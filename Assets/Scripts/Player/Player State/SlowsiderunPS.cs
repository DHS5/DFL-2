using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowsiderunPS : PlayerState
{
    public SlowsiderunPS(Player _player, float _side) : base(_player)
    {
        name = PState.SLOWSIDERUN;

        startSide = _side;
    }

    public override void Enter()
    {
        SetTrigger("Run");
        
        controller.Speed = att.NormalSpeed;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.SideSpeed = att.SlowSideSpeed * side;


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
        // Slow
        else if (side == 0)
        {
            nextState = new SlowrunPS(player);
            stage = Event.EXIT;
        }
        // Siderun
        else if (acc == 0 && side != 0)
        {
            nextState = new SiderunPS(player, side / Mathf.Abs(side), false);
            stage = Event.EXIT;
        }
        // Run
        else if (acc == 0)
        {
            nextState = new RunPS(player);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Run");

        base.Exit();
    }
}
