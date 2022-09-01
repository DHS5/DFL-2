using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Team/Back Attacker", order = 1)]
public class BackAttAttributesSO : AttackerAttributesSO
{
    public override AttackerType Type { get { return AttackerType.BACK; } }
}


