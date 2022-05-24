using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PState { RUN , SLOWRUN , SIDERUN , ACCELERATE , FEINT , JUKE , SPIN }


public class PlayerState
{
    protected enum Event { ENTER , UPDATE , EXIT }


    public PState name;

    protected Event stage;

    protected PlayerState nextState;


    protected PlayerController controller;


    public PlayerState(PlayerController _controller)
    {
        stage = Event.ENTER;
    }


    public virtual void Enter() { stage = Event.UPDATE; }
    public virtual void Update() { stage = Event.UPDATE; }
    public virtual void Exit() { stage = Event.EXIT; }



    public PlayerState Process()
    {
        if (stage == Event.ENTER) Enter();
        if (stage == Event.UPDATE) Update();
        if (stage == Event.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }
}


public class RunPS : PlayerState
{
    public RunPS(PlayerController _controller) : base(_controller)
    {
        name = PState.RUN;

        controller = _controller;
    }

    public override void Enter()
    {
        // set anim run
        
        base.Enter();
    }

    public override void Update()
    {
        //if (controller.Acceleration > 0)
        //{
        //    nextState = new AcceleratePS(controller);
        //    Exit();
        //}
        
        base.Update();
    }
}
