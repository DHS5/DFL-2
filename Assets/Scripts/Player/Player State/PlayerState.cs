using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PState { RUN , SLOWRUN , SIDERUN , SLOWSIDERUN , SPRINT , JUMP , FEINT , JUKE , SPIN }


public abstract class PlayerState
{
    protected enum Event { ENTER , UPDATE , EXIT }


    public PState name;

    protected Event stage;

    protected PlayerState nextState;


    protected PlayerController controller;
    protected Animator animator;


    protected float acc;
    protected float side;
    protected float startSide;

    protected float startTime;
    protected float animTime;

    public PlayerState(PlayerController _controller, Animator _animator)
    {
        stage = Event.ENTER;

        controller = _controller;
        animator = _animator;
    }


    public virtual void Enter() 
    { 
        stage = Event.UPDATE;
        startTime = Time.time;
    }
    public virtual void Update()
    {
        acc = controller.Acceleration;
        side = controller.Direction;

        // A verifier
        controller.gameObject.transform.localRotation = 
            Quaternion.Slerp(controller.gameObject.transform.localRotation, Quaternion.Euler(controller.Velocity), 0.5f);
    }
    public virtual void Exit() { stage = Event.EXIT; Debug.Log(name + " -> " + nextState.name); }



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

