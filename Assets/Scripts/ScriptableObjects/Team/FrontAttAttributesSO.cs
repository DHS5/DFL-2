using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Team/Front Attacker", order = 1)]
public class FrontAttAttributesSO : AttackerAttributesSO
{
    public override AttackerPosition Position { get { return AttackerPosition.FRONT; } }

    public float blockDistance;
    public float blockLate;
    public float blockAngle;
}


