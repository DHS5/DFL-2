using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Team/L Side Attacker", order = 1)]
public class LSideAttAttributesSO : SideAttAttributesSO
{
    public override AttackerType Type { get { return AttackerType.LSIDE; } }

    public override SIDE Side { get { return SIDE.LEFT; } }
}


