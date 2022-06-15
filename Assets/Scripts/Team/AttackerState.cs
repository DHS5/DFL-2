using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AState { PROTECT, DEFEND, BACK }


public abstract class AttackerState
{
    protected enum Event { ENTER, UPDATE, EXIT }


    public AState name;

    protected Event stage;

    protected AttackerState nextState;


    public Attacker attacker;
    public NavMeshAgent agent;
    public Animator animator;



    protected float startTime;


    public AttackerState(Attacker _attacker, NavMeshAgent _agent, Animator _animator)
    {
        stage = Event.ENTER;

        attacker = _attacker;
        agent = _agent;
        animator = _animator;
    }


    public virtual void Enter()
    {
        stage = Event.UPDATE;
        startTime = Time.time;

        //attacker.state = name.ToString();
    }
    public virtual void Update()
    {

    }
    public virtual void Exit() { stage = Event.EXIT; Debug.Log(name + " -> " + nextState.name); }



    public AttackerState Process()
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
}
