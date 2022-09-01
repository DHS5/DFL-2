using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "AttackerCard", menuName = "ScriptableObjects/Card/BackAttackerCard", order = 1)]
public class BackAttackerCardSO : AttackerCardSO
{
    public override string Position { get { return "Back"; } }
}
