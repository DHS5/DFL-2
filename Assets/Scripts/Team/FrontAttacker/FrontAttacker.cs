using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAttacker : Attacker
{
    public FrontAttAttributesSO Att { get; private set; }

    public override void GetAttribute(AttackerAttributesSO att)
    {
        Att = att as FrontAttAttributesSO;

        currentState = new WaitFAS(this, navMeshAgent, animator);
    }



    // ### Functions ###

    public override Vector3 ClampInZone(Vector3 destination)
    {
        destination.z = Mathf.Clamp(destination.z, playerPos.z + Att.positionRadius / 2, playerPos.z + Att.positionRadius);
        destination.x = Mathf.Clamp(destination.x, playerPos.x - ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2), playerPos.x + ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2));
        return destination;
    }

    public override bool InZone(Vector3 destination)
    {
        if (destination.z < playerPos.z + Att.positionRadius && destination.z > playerPos.z + Att.positionRadius / 2)
            if (destination.x < playerPos.x + ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2) && destination.x > playerPos.x - ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2))
                return true;
        return false;
    }
}
