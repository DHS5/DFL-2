using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttacker : Attacker
{
    readonly float angleOffset = 5f;

    public SideAttAttributesSO Att { get; private set; }

    public override void GetAttribute(AttackerAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as SideAttAttributesSO;

        currentState = new WaitSAS(this, navMeshAgent, animator);
    }



    // ### Functions ###

    public override Vector3 ClampInZone(Vector3 destination)
    {
        float minX = (Att.Side == SIDE.LEFT) ? playerPos.x - Att.positionRadius : playerPos.x + Att.positionRadius / 2;
        float maxX = (Att.Side == SIDE.LEFT) ? playerPos.x - Att.positionRadius / 2 : playerPos.x + Att.positionRadius;
        destination.x = Mathf.Clamp(destination.x, minX, maxX);
        destination.z = Mathf.Clamp(destination.z, playerPos.z, playerPos.z + (Mathf.Abs(destination.x - playerPos.x) * Mathf.Sqrt(2) / 2));
        return destination;
    }

    public override bool InZone(Vector3 destination)
    {
        float minX = (Att.Side == SIDE.LEFT) ? playerPos.x - Att.positionRadius : playerPos.x + Att.positionRadius / 2;
        float maxX = (Att.Side == SIDE.LEFT) ? playerPos.x - Att.positionRadius / 2 : playerPos.x + Att.positionRadius;
        if (destination.x < maxX && destination.x > minX)
            if (destination.z < playerPos.z + (Mathf.Abs(destination.x - playerPos.x) * Mathf.Sqrt(2) / 2) && destination.z > playerPos.z)
                return true;
        return false;
    }

    public bool IsThreat()
    {
        float enemyPlayerAngle = Vector3.Angle(target.transform.position - player.transform.position, player.controller.Velocity.normalized);
        float xDist = target.transform.position.x - player.transform.position.x;
        if (xDist * (int)Att.Side > 0 && enemyPlayerAngle < teamManager.backAngle + angleOffset)
            return true;
        return false;
    }
}
