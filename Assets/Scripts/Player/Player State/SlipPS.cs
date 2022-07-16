using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipPS : PlayerState
{
    public SlipPS(Player _player) : base(_player)
    {
        name = PState.SLIP;
    }

    public override void Enter()
    {
        base.Enter();

        animTime = UD.slipTime;

        SetTrigger("Slip");

        controller.Speed /= 2;
        controller.SideSpeed /= 2;
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();

        if (Time.time >= startTime + animTime)
        {
            nextState = new RunPS(player);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        ResetTrigger("Slip");
    }
}
