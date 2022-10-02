using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPS : PlayerState
{
    public GameOverPS(Player _player) : base(_player)
    {
        name = PState.GAMEOVER;

        controller.Speed = 0;
        controller.SideSpeed = 0;
    }
}
