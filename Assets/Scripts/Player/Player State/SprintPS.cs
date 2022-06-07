using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintPS : PlayerState
{
    public SprintPS(Player _player) : base(_player)
    {
        name = PState.SPRINT;
    }


    public override void Enter()
    {
        SetTrigger("Sprint");

        controller.Sprint();

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.Speed = controller.NormalSpeed * (1 - (1 - controller.AccelerationM) * acc);

        controller.SideSpeed = controller.AccSideSpeed * side;

        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }
        // Run
        else if ((acc == 0 && side == 0) || (!controller.CanAccelerate && side == 0))
        {
            nextState = new RunPS(player);
            stage = Event.EXIT;
        }
        // Siderun
        else if ((acc == 0 && side != 0) || (!controller.CanAccelerate && side != 0))
        {
            nextState = new SiderunPS(player, side / Mathf.Abs(side), true);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Sprint");
        
        base.Exit();
    }
}
