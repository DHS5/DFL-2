using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingGauge : MonoBehaviour
{
    private AsyncOperation loadingOperation;

    [Header("UI component")]
    [SerializeField] private Slider slider;

    void LateUpdate()
    {
        if (loadingOperation != null)
        {
            slider.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        }
    }

    public void GetOperation(in AsyncOperation op)
    {
        loadingOperation = op;
    }
}
