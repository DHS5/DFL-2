using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPS : PlayerState
{
    public JumpPS(Player _player) : base(_player)
    {
        name = PState.JUMP;
    }


    public override void Enter()
    {
        SetTrigger("Jump");

        controller.Jump();
        
        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (controller.OnGround)
        {
            // Sprint
            if (acc > 0 && controller.CanAccelerate)
                nextState = new SprintPS(player);
            // Slow
            else if (acc < 0)
                nextState = new SlowrunPS(player);
            // Siderun
            else if (acc == 0 && side != 0)
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true);
            // Slowsiderun
            else if (acc < 0 && side != 0)
                nextState = new SlowsiderunPS(player, side / Mathf.Abs(side));
            // Run
            else
                nextState = new RunPS(player);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Jump");

        base.Exit();
    }
}