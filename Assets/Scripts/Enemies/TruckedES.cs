using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckedES : EnemyState
{
    Vector3 impact;
    readonly float impactSpeed = 15;

    public TruckedES(Enemy _enemy, NavMeshAgent _agent, Animator _animator, Vector3 _impact) : base(_enemy, _agent, _animator)
    {
        name = EState.TRUCKED;

        enemy = _enemy;
        if (_impact != Vector3.zero) impact = new Vector3(_impact.x, _impact.y, -Mathf.Abs(_impact.z));
        else impact = -Vector3.forward;


        agent.isStopped = true;
        agent.updateRotation = false;


        Debug.Log(impact + ";" + Quaternion.LookRotation(impact).eulerAngles);
        enemy.transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(impact).eulerAngles.y, 0);
        agent.velocity = -impact.normalized * impactSpeed;

        animator.SetTrigger("Trucked");
    }
}
