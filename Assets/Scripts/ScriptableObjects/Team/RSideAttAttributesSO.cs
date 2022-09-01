using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Team/R Side Attacker", order = 1)]
public class RSideAttAttributesSO : SideAttAttributesSO
{
    public override AttackerType Type { get { return AttackerType.RSIDE; } }

    public override SIDE Side { get { return SIDE.RIGHT; } }
}


