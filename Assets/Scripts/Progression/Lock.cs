using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lock : MonoBehaviour
{
    [Header("Content")]
    public bool forceLocked;
    [Space, Space]
    [TextArea] public string content;
    public bool anchorUp;

    [Header("UI component")]
    [SerializeField] private TextMeshProUGUI lockText;
    [SerializeField] private RectTransform layout;

    [Header("UI parent")]
    [SerializeField] private Button button;
    [SerializeField] private Toggle toggle;

    private bool gotInfos = false;

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
        set 
        { 
            lockText.text = value;
            content = value;
        }
    }

    private void Awake()
    {
        if (!gotInfos)
            ApplyLockInfos();

        if (anchorUp) AnchorUp();
    }

    private void LockParent(bool locked)
    {
        if (button != null)
        {
            button.interactable = !locked;
        }
        if (toggle != null)
        {
            toggle.interactable = !locked;
        }
    }

    public void ApplyLockInfos(bool locked, string text)
    {
        Locked = locked || forceLocked;
        Content = forceLocked ? "Not available in this version" : text;
        gotInfos = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
    }
    public void ApplyLockInfos()
    {
        Content = content;
        Locked = true;
    }


    public void Rebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
    }

    private void AnchorUp()
    {
        layout.pivot = new Vector2(0.5f, 1);
        layout.anchoredPosition = Vector2.zero;
    }
}
