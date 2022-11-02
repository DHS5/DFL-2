using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeintPS : PlayerState
{
    public FeintPS(Player _player, float _side) : base(_player)
    {
        name = PState.FEINT;

        startSide = _side;
    }

    public override void Enter()
    {
        player.effects.AudioPlayerEffort(true);

        animTime = UD.feintTime;

        controller.Speed = att.FeintSpeed;
        controller.SideSpeed = att.FeintSideSpeed * startSide;

        SlowMotion(UD.feintTime, 8f, 2);

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        if (Time.time >= startTime + animTime)
        {
            // Slip
            if (IsRaining)
                nextState = new SlipPS(player);
            // Jump
            else if (Input.GetKeyDown(KeyCode.Space) && controller.CanJump(att.JumpCost))
            {
                nextState = new JumpPS(player);
            }
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
            else if (side != 0 && side * startSide > 0)
            {
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true, false);
                stage = Event.EXIT;
            }
            // Run
            else nextState = new RunPS(player, true);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Feint");

        base.Exit();
    }
}
