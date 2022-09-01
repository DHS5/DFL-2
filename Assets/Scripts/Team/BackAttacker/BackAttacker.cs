using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAttacker : Attacker
{
    public BackAttAttributesSO Att { get; private set; }

    public override void GetAttribute(AttackerAttributesSO att)
    {
        Att = att as BackAttAttributesSO;

        currentState = new WaitBAS(this, navMeshAgent, animator);
    }


    


    // ### Functions ###

    public override Vector3 ClampInZone(Vector3 destination)
    {
        destination.z = Mathf.Clamp(destination.z, playerPos.z - Att.positionRadius, playerPos.z - Att.positionRadius / 2);
        destination.x = Mathf.Clamp(destination.x, playerPos.x - ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2), playerPos.x + ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2));
        return destination;
    }

    public override bool InZone(Vector3 destination)
    {
        if (destination.z > playerPos.z - Att.positionRadius && destination.z < playerPos.z - Att.positionRadius / 2)
            if (destination.x < playerPos.x + ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2) && destination.x > playerPos.x - ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2))
                return true;
        return false;
    }
}
