using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiderunPS : PlayerState
{
    private float startSide;
    
    public SiderunPS(PlayerController _controller, Animator _animator, float _side) : base(_controller, _animator)
    {
        name = PState.SIDERUN;

        startSide = _side;
    }


    public override void Enter()
    {
        // anim
        
        base.Enter();
    }


    public override void Update()
    {
        base.Update();

        controller.SideSpeed = controller.NormalSideSpeed * side;

        // A verifier
        controller.gameObject.transform.localRotation = Quaternion.Slerp(controller.gameObject.transform.localRotation, Quaternion.Euler(controller.Velocity), 0.5f);


        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextState = new JumpPS(controller, animator);
            stage = Event.EXIT;
        }
        // Sprint
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(controller, animator);
            stage = Event.EXIT;
        }
        // Slow
        else if (acc < 0)
        {
            nextState = new SlowrunPS(controller, animator);
            stage = Event.EXIT;
        }
        // Run
        else if (side == 0)
        {
            nextState = new RunPS(controller, animator);
            stage = Event.EXIT;
        }
    }
}
