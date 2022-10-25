using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPS : PlayerState
{
    public FlipPS(Player _player) : base(_player)
    {
        name = PState.FLIP;
    }


    public override void Enter()
    {
        SetTrigger("Flip");
        SetFloat("HangTime", 1 / controller.Jump(att.FlipCost, att.FlipHeight));

        controller.Speed = att.FlipSpeed;

        player.playerManager.JumpUIAnimation(att.FlipCost);

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
                nextState = new SlowsiderunPS(player, side / Mathf.Abs(side), true);
            // Slow
            else if (acc < 0)
                nextState = new SlowrunPS(player);
            // Siderun
            else if (acc == 0 && side != 0)
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true, true);
            // Run
            else
                nextState = new RunPS(player, true);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Flip");

        base.Exit();
    }
}
