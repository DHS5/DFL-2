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


    public Player player;
    public PlayerController controller;
    public Animator[] animators = new Animator[2];


    protected float acc;
    protected float side;
    protected float startSide;

    protected float startTime;
    protected float animTime;

    public PlayerState(Player _player)
    {
        stage = Event.ENTER;

        player = _player;

        controller = player.controller;

        animators[0] = player.fpAnimator;
        animators[1] = player.tpAnimator;
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
    }
    public virtual void Exit() { stage = Event.EXIT; Debug.Log(name + " -> " + nextState.name); }



    public PlayerState Process()
    {
        if (stage == Event.ENTER) Enter();
        else if (stage == Event.UPDATE) Update();
        else if (stage == Event.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    protected void PlayerOrientation()
    {
        if (player.gameManager.GameOn)
        {
            player.TPPlayer.transform.localRotation =
            Quaternion.Slerp(player.TPPlayer.transform.localRotation,
                Quaternion.LookRotation(controller.Velocity, Vector3.up), 0.02f);

            player.FPPlayer.transform.localRotation =
                Quaternion.Slerp(player.FPPlayer.transform.localRotation,
                    Quaternion.LookRotation(controller.Velocity, Vector3.up), 0.02f);
        }
    }

    // Animators functions

    protected void SetTrigger(string name)
    {
        foreach (Animator a in animators)
        {
            a.SetTrigger(name);
        }
    }
    protected void ResetTrigger(string name)
    {
        foreach (Animator a in animators)
        {
            a.ResetTrigger(name);
        }
    }
    protected void SetFloat(string name, float value)
    {
        foreach (Animator a in animators)
        {
            a.SetFloat(name, value);
        }
    }
}

