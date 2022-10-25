using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowsiderunPS : PlayerState
{
    private bool anim;
    public SlowsiderunPS(Player _player, float _side, bool _anim) : base(_player)
    {
        name = PState.SLOWSIDERUN;

        startSide = _side;
        anim = _anim;
    }

    public override void Enter()
    {
        if (anim) SetTrigger("Run");
        
        controller.Speed = att.NormalSpeed;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.SideSpeed = (att.NormalSideSpeed + ((att.SlowSideSpeed - att.NormalSideSpeed) * Mathf.Abs(acc))) * side;


        // Spin
        if (att.CanSpin && Input.GetAxisRaw("Horizontal") * startSide < 0)
        {
            nextState = new SpinPS(player, -startSide);
            stage = Event.EXIT;
        }
        // Jump
        else if (Input.GetKeyDown(KeyCode.Space) && controller.CanJump(att.JumpCost))
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
        // Slow
        else if (side == 0)
        {
            nextState = new SlowrunPS(player);
            stage = Event.EXIT;
        }
        // Siderun
        else if (acc == 0 && side != 0)
        {
            nextState = new SiderunPS(player, side / Mathf.Abs(side), false, false);
            stage = Event.EXIT;
        }
        // Run
        else if (acc == 0)
        {
            nextState = new RunPS(player, false);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Run");

        base.Exit();
    }
}
