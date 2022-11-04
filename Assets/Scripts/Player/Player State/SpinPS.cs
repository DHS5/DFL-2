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

        player.effects.AudioPlayerEffort(true);

        animTime = UD.spinTime;

        controller.Speed = att.SpinSpeed;
        controller.SideSpeed = att.SpinSideSpeed * startSide;

        SlowMotion(UD.spinTime, 5f, 1);

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation(true);


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
            // Slowsiderun
            else if (acc < 0 && side != 0 && side * startSide > 0)
            {
                nextState = new SlowsiderunPS(player, side / Mathf.Abs(side), true);
                stage = Event.EXIT;
            }
            // Slowrun
            else if (acc < 0)
            {
                nextState = new SlowrunPS(player);
            }
            // Siderun
            else if (side != 0)
            {
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true, false);
            }
            // Run
            else nextState = new RunPS(player, true);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Spin");

        base.Exit();
    }
}
