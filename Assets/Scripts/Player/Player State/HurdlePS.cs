using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdlePS : PlayerState
{
    private bool quittedGround = false;

    public HurdlePS(Player _player) : base(_player)
    {
        name = PState.HURDLE;
    }


    public override void Enter()
    {
        SetTrigger("Hurdle");
        SetFloat("HangTime", 1 / controller.Jump(att.HurdleCost, att.HurdleHeight));

        player.effects.AudioPlayerEffort(false);

        player.playerManager.JumpUIAnimation(att.HurdleCost);
        
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();

        if (!quittedGround && !controller.TouchGround(0.05f))
        {
            quittedGround = true;
            controller.ForceQuitGround();
        }

        if (quittedGround && controller.OnGround)
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
        ResetTrigger("Hurdle");

        base.Exit();
    }
}
