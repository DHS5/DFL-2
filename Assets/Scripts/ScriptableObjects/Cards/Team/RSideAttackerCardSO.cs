using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "AttackerCard", menuName = "ScriptableObjects/Card/RSideAttackerCard", order = 1)]
public class RSideAttackerCardSO : AttackerCardSO
{
    public override string Position { get { return "Right Side"; } }
}
