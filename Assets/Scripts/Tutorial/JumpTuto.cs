using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JumpTuto : TutorialPopup
{
    private void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            CanPass = true;
        }
    }
}
