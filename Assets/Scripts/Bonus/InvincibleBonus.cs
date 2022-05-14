using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBonus : Bonus
{
    protected override void TriggerBonus()
    {
        playerG.isInvincible = true;

        // Invokes the EndBonus method
        base.TriggerBonus();
    }

    /// <summary>
    /// Puts the jump attribute back to his initial state
    /// </summary>
    protected override void EndBonus()
    {
        playerG.isInvincible = false;
        base.EndBonus();
    }
}
