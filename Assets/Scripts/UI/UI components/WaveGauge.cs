using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveGauge : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI waveText;


    public void Set(string text, float value)
    {
        waveText.text = text;
        slider.value = value;
    }
}
