using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBonus : Bonus
{
    [SerializeField] private float bonusJumpPower;

    protected override void TriggerBonus()
    {
        playerC.bonusJump = new Vector3(0, bonusJumpPower, 0);

        // Invokes the EndBonus method
        base.TriggerBonus();
    }

    /// <summary>
    /// Puts the jump attribute back to his initial state
    /// </summary>
    protected override void EndBonus()
    {
        playerC.bonusJump = new Vector3(0,0,0);
        base.EndBonus();
    }
}
