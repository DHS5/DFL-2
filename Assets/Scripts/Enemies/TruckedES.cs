using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckedES : EnemyState
{
    Vector3 impact;
    Vector3 impactDir;
    float impactPower;

    readonly float impactMin = 10;
    readonly float impactMax = 20;

    public TruckedES(Enemy _enemy, NavMeshAgent _agent, Animator _animator, Collision _collision) : base(_enemy, _agent, _animator)
    {
        name = EState.TRUCKED;

        enemy = _enemy;

        agent.ResetPath();
        agent.isStopped = true;
        agent.updateRotation = false;

        impactDir = (_collision.GetContact(0).point - enemy.transform.position).normalized;
        impactPower = Mathf.Clamp(_collision.impulse.magnitude / Time.fixedDeltaTime, impactMin, impactMax);
        impact = impactPower * new Vector3(impactDir.x, 0, -Mathf.Max(Mathf.Abs(impactDir.z), Mathf.Abs(impactDir.x))).normalized;

        //Debug.Log(impact);

        enemy.transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(impact).eulerAngles.y, 0);
        agent.velocity = -impact;

        //Debug.Log(agent.velocity);

        animator.SetLayerWeight(animator.GetLayerIndex("Trucked Layer"), 1);
        animator.SetTrigger("Trucked");
    }

    public override void Enter()
    {
        base.Enter();

        agent.ResetPath();
        agent.isStopped = true;

        agent.velocity = -impact;
    }
}
