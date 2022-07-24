using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordInputField : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public bool State
    {
        set
        {
            if (value)
                inputField.contentType = TMP_InputField.ContentType.Standard;
            else
                inputField.contentType = TMP_InputField.ContentType.Password;

            inputField.ForceLabelUpdate();
        }
    }
}
