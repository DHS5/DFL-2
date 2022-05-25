using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukePS : PlayerState
{
    public JukePS(PlayerController _controller, Animator _animator) : base(_controller, _animator)
    {
        name = PState.JUKE;
    }
}
