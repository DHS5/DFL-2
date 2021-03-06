using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPS : PlayerState
{
    public SpinPS(Player _player, float _side) : base(_player)
    {
        name = PState.SPIN;

        startSide = _side;
    }

    public override void Enter()
    {
        SetTrigger("Spin");
        SetFloat("Dir", startSide);

        animTime = UD.spinTime;

        controller.Speed = att.SpinSpeed;
        controller.SideSpeed = att.SpinSideSpeed * startSide;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (Time.time >= startTime + animTime)
        {
            // Slip
            if (IsRaining)
                nextState = new SlipPS(player);
            // Acceleration
            else if (acc > 0 && controller.CanAccelerate)
            {
                nextState = new SprintPS(player);
            }
            // Slowrun
            else if (acc < 0)
            {
                nextState = new SlowrunPS(player);
            }
            // Siderun
            else if (side != 0)
            {
                nextState = new SiderunPS(player, side / Mathf.Abs(side), false);
            }
            // Run
            else nextState = new RunPS(player);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Spin");

        base.Exit();
    }
}
