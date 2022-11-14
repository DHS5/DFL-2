using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AState { WAIT, PROTECT, DEFEND, BACK }


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

    //protected Vector3 DestinationDir

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
    public virtual void Exit() { stage = Event.EXIT; } // Debug.Log(name + " -> " + nextState.name); }



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



    protected Vector3 AnticipationDir(bool anticipationType)
    {
        return anticipationType ? attacker.playerDir : Vector3.zero;
    }
    protected Vector3 EnemyDir(bool anticipationType, float anticipation)
    {
        if (anticipationType) return attacker.player2TargetDir * attacker.playerTargetDist;
        Vector3 enemyDest = attacker.targetPos + attacker.targetDir * anticipation;
        float dist = Vector3.Distance(enemyDest, attacker.playerPos);
        return (enemyDest - attacker.playerPos).normalized * dist;
    }
}
