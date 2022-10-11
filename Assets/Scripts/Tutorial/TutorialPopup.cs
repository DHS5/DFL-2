using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPopup : MonoBehaviour
{
    protected MainManager main;

    [Header("Content")]
    [SerializeField] [TextArea] private string contentText;
    [Tooltip("Time between the previous popup and this one")]
    public float timeBeforeShowingUp;
    [SerializeField] private bool showOkButton;


    // UI Components 
    private TextMeshProUGUI text;
    private Button okButton;

    protected bool active = false;

    public bool CanPass { get; protected set; }

    private void Awake()
    {
        main = FindObjectOfType<MainManager>();

        text = GetComponentInChildren<TextMeshProUGUI>();
        okButton = text.gameObject.GetComponentInChildren<Button>();
    }

    private void Start()
    {
        text.text = contentText;
        okButton.gameObject.SetActive(showOkButton);

        LayoutRebuilder.ForceRebuildLayoutImmediate(text.transform as RectTransform);

        CanPass = false;
    }

    public virtual void PreCondition()
    {
        active = true;
        gameObject.SetActive(true);
        if (showOkButton)
            CursorManager.ForceUnlockCursor();

        Time.timeScale = 0;
    }

    public void Ok()
    {
        CanPass = true;

        Time.timeScale = 1;
        CursorManager.ForceLockCursor();
    }
}
