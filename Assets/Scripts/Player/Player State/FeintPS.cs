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
        animTime = controller.feintTime;

        controller.Speed = controller.FeintSpeed;
        controller.SideSpeed = controller.FeintSideSpeed * startSide;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();        


        if (Time.time >= startTime + animTime)
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
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
                nextState = new SiderunPS(player, side / Mathf.Abs(side), false);
                stage = Event.EXIT;
            }
            // Run
            else nextState = new RunPS(player);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Feint");

        base.Exit();
    }
}
