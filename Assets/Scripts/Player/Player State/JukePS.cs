using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukePS : PlayerState
{
    public JukePS(Player _player, float _side) : base(_player)
    {
        name = PState.JUKE;

        startSide = _side;
    }

    public override void Enter()
    {
        player.effects.AudioPlayerEffort(false);

        SetTrigger("Juke");
        SetFloat("Dir", startSide);
        animTime = UD.jukeTime;

        controller.Speed = att.JukeSpeed;
        controller.SideSpeed = att.JukeSideSpeed * startSide;

        SlowMotion(UD.jukeTime, 8f, 3);

        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (att.CanJukeSpin && Input.GetAxisRaw("Horizontal") * startSide < 0 && acc == 0)
            nextState = new SpinPS(player, Input.GetAxisRaw("Horizontal"));

        if (Time.time >= startTime + animTime)
        {
            if (nextState == null)
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
                // SlowSiderun
                else if (acc < 0 && side * startSide > 0)
                {
                    nextState = new SlowsiderunPS(player, side / Mathf.Abs(side), true);
                }
                // Siderun
                else if (acc == 0 && side * startSide > 0)
                {
                    nextState = new SiderunPS(player, side / Mathf.Abs(side), true, false);
                }
                // Slowrun
                else if (acc < 0)
                {
                    nextState = new SlowrunPS(player);
                }
                // Run
                else nextState = new RunPS(player, true);
            }
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Juke");

        base.Exit();
    }
}
