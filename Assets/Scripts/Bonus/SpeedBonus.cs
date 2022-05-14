using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : Bonus
{
    [SerializeField] private float bonusSpeed;

    protected override void TriggerBonus()
    {
        playerC.bonusSpeed = bonusSpeed;

        // Invokes the EndBonus method
        base.TriggerBonus();
    }

    /// <summary>
    /// Puts the jump attribute back to his initial state
    /// </summary>
    protected override void EndBonus()
    {
        playerC.bonusSpeed = 0f;
        base.EndBonus();
    }
}
