using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveGauge : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI resultText;


    public void Set(string wave, string result, float value)
    {
        waveText.text = wave;
        resultText.text = result;
        slider.value = value;
    }
}
