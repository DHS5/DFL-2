using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lock : MonoBehaviour
{
    [Header("Content")]
    [TextArea] public string content;

    [Header("UI component")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("UI parent")]
    [SerializeField] private Button button;
    [SerializeField] private Toggle toggle;

    public bool Locked
    {
        set
        {
            gameObject.SetActive(value);
            LockParent(value);
        }
    }
    public string Content
    {
        set { text.text = value; }
    }

    private void Awake()
    {
        Locked = true;

        text.text = content;
    }

    private void LockParent(bool locked)
    {
        if (button != null) button.interactable = !locked;
        if (toggle != null) toggle.interactable = !locked;
    }

    public void ApplyLockInfos(bool locked, string text)
    {
        Locked = locked;
        Content = text;
    }
}
