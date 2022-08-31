using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShiftTuto : TutorialPopup
{
    private void Update()
    {
        if (active && Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") < 0)
        {
            Time.timeScale = 1;
            CanPass = true;
        }
    }
}
