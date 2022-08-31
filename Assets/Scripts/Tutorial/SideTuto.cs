using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideTuto : TutorialPopup
{
    private float side;

    private void Update()
    {
        if (active)
        {
            if (Mathf.Abs(main.PlayerManager.player.transform.position.x) > 2.5f)
            {
                Time.timeScale = 1;
                CanPass = true;
            }

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                Time.timeScale = 0;
            }
            else if (Input.GetAxisRaw("Horizontal") * side >= 0)
            {
                Time.timeScale = 1;
                side = Input.GetAxisRaw("Horizontal");
            }
        }
    }
}
