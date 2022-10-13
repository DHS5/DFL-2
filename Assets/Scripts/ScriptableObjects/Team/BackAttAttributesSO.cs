using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Team/Back Attacker", order = 1)]
public class BackAttAttributesSO : AttackerAttributesSO
{
    public override AttackerPosition Position { get { return AttackerPosition.BACK; } }
}


