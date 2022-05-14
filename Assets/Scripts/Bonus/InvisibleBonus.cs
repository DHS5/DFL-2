using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBonus : Bonus
{
    protected override void TriggerBonus()
    {
        playerG.isVisible = false;

        // Invokes the EndBonus method
        base.TriggerBonus();
    }

    /// <summary>
    /// Puts the jump attribute back to his initial state
    /// </summary>
    protected override void EndBonus()
    {
        playerG.isVisible = true;
        base.EndBonus();
    }
}
