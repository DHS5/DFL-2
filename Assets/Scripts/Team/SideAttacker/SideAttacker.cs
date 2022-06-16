using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttacker : Attacker
{
    [System.Serializable]
    private enum SIDE { LEFT = -1, RIGHT = 1}

    [SerializeField] private SIDE side;



    readonly private float angleOffset = 5f;



    protected override void Awake()
    {
        base.Awake();

        type = (side == SIDE.LEFT) ? AttackerType.LSIDE : AttackerType.RSIDE;

        currentState = new WaitSAS(this, navMeshAgent, animator, (int) side);
    }


    public override void ProtectPlayer()
    {
        base.ProtectPlayer();

        currentState = currentState.Process();

        if (player.gameplay.onField && !gameOver)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (reactivity != 0 && !gameOver)
        {
            Invoke(nameof(ProtectPlayer), reactivity);
        }
    }


    private void Update()
    {
        if (reactivity == 0 && player.gameplay.onField && !gameOver)
        {
            ProtectPlayer();
        }
    }


    // ### Functions ###

    public override Vector3 ClampInZone(Vector3 destination)
    {
        float minX = (side == SIDE.LEFT) ? playerPos.x - positionRadius : playerPos.x + positionRadius / 2;
        float maxX = (side == SIDE.LEFT) ? playerPos.x - positionRadius / 2 : playerPos.x + positionRadius;
        destination.x = Mathf.Clamp(destination.x, minX, maxX);
        destination.z = Mathf.Clamp(destination.z, playerPos.z, playerPos.z + (Mathf.Abs(destination.x - playerPos.x) * Mathf.Sqrt(2) / 2));
        return destination;
    }

    public override bool InZone(Vector3 destination)
    {
        float minX = (side == SIDE.LEFT) ? playerPos.x - positionRadius : playerPos.x + positionRadius / 2;
        float maxX = (side == SIDE.LEFT) ? playerPos.x - positionRadius / 2 : playerPos.x + positionRadius;
        if (destination.x < maxX && destination.x > minX)
            if (destination.z < playerPos.z + (Mathf.Abs(destination.x - playerPos.x) * Mathf.Sqrt(2) / 2) && destination.z > playerPos.z)
                return true;
        return false;
    }

    public bool IsThreat()
    {
        float enemyPlayerAngle = Vector3.Angle(target.transform.position - player.transform.position, player.controller.Velocity.normalized);
        float xDist = target.transform.position.x - player.transform.position.x;
        if (xDist * (int) side > 0 && enemyPlayerAngle < teamManager.backAngle + angleOffset)
            return true;
        return false;
    }
}
