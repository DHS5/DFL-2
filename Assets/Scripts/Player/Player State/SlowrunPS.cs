using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowrunPS : PlayerState
{
    public SlowrunPS(Player _player) : base(_player)
    {
        name = PState.SLOWRUN;
    }


    public override void Enter()
    {
        SetTrigger("Slow");

        controller.SideSpeed = 0;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.Speed = att.NormalSpeed * ( 1 + (1 - att.SlowM) * acc );


        // Juke
        if (att.CanJuke && Time.time - startTime > UD.jukeDelay && Input.GetAxisRaw("Horizontal") != 0)
        {
            nextState = new JukePS(player, Input.GetAxisRaw("Horizontal"));
            stage = Event.EXIT;
        }
        // Flip
        else if (att.CanFlip && Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new FlipPS(player);
            stage = Event.EXIT;
        }
        // Jump
        else if (!att.CanFlip && Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }
        // Sprint
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(player);
            stage = Event.EXIT;
        }
        // Run
        else if (acc == 0)
        {
            nextState = new RunPS(player);
            stage = Event.EXIT;
        }
        // Slowsiderun
        else if (!att.CanJuke && side != 0)
        {
            nextState = new SlowsiderunPS(player, side / Mathf.Abs(side));
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Slow");

        base.Exit();
    }
}
