using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighKneePS : PlayerState
{
    public HighKneePS(Player _player) : base(_player)
    {
        name = PState.HIGHKNEE;
    }


    public override void Enter()
    {
        SetTrigger("HighKnee");
        player.gameplay.isHighKneeing = true;

        SetFloat("HangTime", 1 / controller.Jump(att.HighKneeCost, att.HighKneeHeight));

        player.playerManager.JumpUIAnimation(att.HighKneeCost);

        SlowMotion(0.5f, 8f, 2);

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();

        if (controller.OnGround)
        {
            // Slip
            if (IsRaining)
                nextState = new SlipPS(player);
            // Sprint
            else if (acc > 0 && controller.CanAccelerate)
                nextState = new SprintPS(player);
            // Slowsiderun
            else if (acc < 0 && side != 0)
                nextState = new SlowsiderunPS(player, side / Mathf.Abs(side));
            // Slow
            else if (acc < 0)
                nextState = new SlowrunPS(player);
            // Siderun
            else if (acc == 0 && side != 0)
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true);
            // Run
            else
                nextState = new RunPS(player);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("HighKnee");

        player.gameplay.isHighKneeing = false;

        base.Exit();
    }
}
