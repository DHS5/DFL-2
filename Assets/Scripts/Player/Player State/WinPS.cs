using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPS : PlayerState
{
    public WinPS(Player _player) : base(_player)
    {
        name = PState.WIN;
        SetTrigger("Win");
    }
}
