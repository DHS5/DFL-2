using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "AttackerCard", menuName = "ScriptableObjects/Card/FrontAttackerCard", order = 1)]
public class FrontAttackerCardSO : AttackerCardSO
{
    public override string Position { get { return "Front"; } }
}
