using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinTuto : TutorialPopup
{
    private float startSide = 0;

    private void Update()
    {
        if (startSide == 0) startSide = Input.GetAxis("Horizontal");

        if (active)
        {
            if (Input.GetAxisRaw("Horizontal") * startSide < 0)
            {
                Time.timeScale = 1;
                CanPass = true;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
}
