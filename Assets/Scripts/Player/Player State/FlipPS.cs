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
        SetFloat("HangTime", 1 / att.HangTime);

        controller.Speed = att.FlipSpeed;
        controller.bonusJump = att.FlipHeight;
        controller.Jump();
        
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
        ResetTrigger("Flip");

        base.Exit();
    }
}
