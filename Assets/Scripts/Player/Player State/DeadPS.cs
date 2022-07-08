using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPS : PlayerState
{
    public DeadPS(Player _player) : base(_player)
    {
        name = PState.DEAD;
        SetTrigger("Dead");
    }
}
