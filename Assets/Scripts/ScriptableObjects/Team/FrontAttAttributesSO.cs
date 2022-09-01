using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Team/Front Attacker", order = 1)]
public class FrontAttAttributesSO : AttackerAttributesSO
{
    public override AttackerType Type { get { return AttackerType.FRONT; } }
}


