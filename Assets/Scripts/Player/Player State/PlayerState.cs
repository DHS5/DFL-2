using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PState { RUN , SLOWRUN , SIDERUN , SPRINT , JUMP , FEINT , JUKE , SPIN }


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


    public PlayerState(PlayerController _controller, Animator _animator)
    {
        stage = Event.ENTER;

        controller = _controller;
        animator = _animator;
    }


    public virtual void Enter() { stage = Event.UPDATE; }
    public virtual void Update()
    {
        acc = Input.GetAxis("Vertical");
        side = Input.GetAxis("Horizontal");
    }
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

