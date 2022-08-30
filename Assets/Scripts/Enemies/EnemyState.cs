using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EState { WAIT, POSITIONNING, INTERCEPT, CHASE, ATTACK, FALL, TIRED, READY }


public abstract class EnemyState
{
    protected enum Event { ENTER, UPDATE, EXIT }


    public EState name;

    protected Event stage;

    protected EnemyState nextState;


    public Enemy enemy;
    public NavMeshAgent agent;
    public Animator animator;



    protected float startTime;


    public EnemyState(Enemy _enemy ,NavMeshAgent _agent, Animator _animator)
    {
        stage = Event.ENTER;

        enemy = _enemy;
        agent = _agent;
        animator = _animator;
    }


    public virtual void Enter()
    {
        stage = Event.UPDATE;
        startTime = Time.time;

        //enemy.state = name.ToString();
    }
    public virtual void Update()
    {
        
    }
    public virtual void Exit() { stage = Event.EXIT; Debug.Log(name + " -> " + nextState.name); }



    public EnemyState Process()
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

    protected void EnemyOrientation()
    {
        //if (controller.playerManager.gameManager.GameOn)
        //{
        //    controller.playerManager.TPPlayer.transform.localRotation =
        //    Quaternion.Slerp(controller.playerManager.TPPlayer.transform.localRotation,
        //        Quaternion.LookRotation(controller.Velocity, Vector3.up), 0.02f);
        //}
    }
}
