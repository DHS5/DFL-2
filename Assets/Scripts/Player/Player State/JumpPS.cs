using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPS : PlayerState
{
    private bool quittedGround = false;

    public JumpPS(Player _player) : base(_player)
    {
        name = PState.JUMP;
    }


    public override void Enter()
    {
        SetTrigger("Jump");
        SetFloat("HangTime", 1 / controller.Jump(att.JumpCost, 0));

        player.effects.AudioPlayerEffort(false);

        player.playerManager.JumpUIAnimation(att.JumpCost);
        
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();

        if (!quittedGround && !controller.TouchGround())
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
        ResetTrigger("Jump");

        base.Exit();
    }
}
