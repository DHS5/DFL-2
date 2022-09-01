using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "AttackerCard", menuName = "ScriptableObjects/Card/LSideAttackerCard", order = 1)]
public class LSideAttackerCardSO : AttackerCardSO
{
    public override string Position { get { return "Left Side"; } }
}
