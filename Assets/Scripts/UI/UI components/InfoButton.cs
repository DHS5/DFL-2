using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoButton : MonoBehaviour
{
    private SettingsManager settingsManager;
    private SettingsManager SettingsManager
    {
        get
        {
            if (settingsManager == null)
                settingsManager = FindObjectOfType<SettingsManager>();
            return settingsManager;
        }
    }


    [Header("Content")]
    [SerializeField] [Multiline] private string text;

    [Header("Parameters")]
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;
    [Space]
    [SerializeField] private float xSize;

    [Header("UI components")]
    [SerializeField] private GameObject infoButtonObject;
    [SerializeField] private RectTransform windowRect;
    [SerializeField] private TextMeshProUGUI textComponent;

    // Eventual parent
    private Button parentButton;
    private Toggle parentToggle;
    private TMP_Dropdown parentDropdown;
    private Slider parentSlider;

    private bool parentInteractable;


    private void Awake()
    {
        textComponent.text = text;

        SearchParent();

        LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.transform as RectTransform);
        Reposition();
    }

    private void OnEnable()
    {
        SetActive(SettingsManager.InfoButtonsOn);
    }


    // ### Functions ###

    private void Reposition()
    {
        windowRect.anchoredPosition = new Vector2(xPos, yPos);

        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xSize);
    }

    private void SearchParent()
    {
        parentButton = transform.parent.GetComponent<Button>();
        parentToggle = transform.parent.GetComponent<Toggle>();
        parentDropdown = transform.parent.GetComponent<TMP_Dropdown>();
        parentSlider = transform.parent.GetComponent<Slider>();
    }

    private void GetParentState()
    {
        if (parentButton != null) parentInteractable = parentButton.interactable;
        if (parentToggle != null) parentInteractable = parentToggle.interactable;
        if (parentDropdown != null) parentInteractable = parentDropdown.interactable;
        if (parentSlider != null) parentInteractable = parentSlider.interactable;
    }

    public void Open(bool state)
    {
        if (state == true) GetParentState();

        bool i = !state & parentInteractable;
        if (parentButton != null) parentButton.interactable = i;
        if (parentToggle != null) parentToggle.interactable = i;
        if (parentDropdown != null) parentDropdown.interactable = i;
        if (parentSlider != null) parentSlider.interactable = i;

        LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.transform as RectTransform);
    }


    public void SetActive(bool state)
    {
        infoButtonObject.SetActive(state);
    }
}
