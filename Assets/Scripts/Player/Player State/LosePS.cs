using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePS : PlayerState
{
    public LosePS(Player _player) : base(_player)
    {
        name = PState.LOSE;
        SetTrigger("Lose");
    }
}
