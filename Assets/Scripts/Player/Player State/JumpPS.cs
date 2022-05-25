using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPS : PlayerState
{
    public JumpPS(PlayerController _controller, Animator _animator) : base(_controller, _animator)
    {
        name = PState.JUMP;
    }


    public override void Enter()
    {
        // anim

        controller.PlayerRigidbody.AddForce(controller.JumpPower + controller.bonusJump, ForceMode.Impulse);
        
        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (controller.OnGround)
        {
            // Sprint
            if (acc > 0 && controller.CanAccelerate)
                nextState = new SprintPS(controller, animator);
            // Slow
            else if (acc < 0)
                nextState = new SlowrunPS(controller, animator);
            // Siderun
            else if (acc == 0 && side != 0)
                nextState = new SiderunPS(controller, animator, side / Mathf.Abs(side));
            // Run
            else
                nextState = new RunPS(controller, animator);

            stage = Event.EXIT;
        }
    }
}
