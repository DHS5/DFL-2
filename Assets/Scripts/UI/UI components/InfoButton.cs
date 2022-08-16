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
    [SerializeField] private float ySize;

    [Header("UI components")]
    [SerializeField] private GameObject infoButtonObject;
    [SerializeField] private RectTransform windowRect;
    [SerializeField] private TextMeshProUGUI textComponent;

    // Eventual parent
    private Button parentButton;
    private Toggle parentToggle;
    private TMP_Dropdown parentDropdown;
    private Slider parentSlider;

    private bool parentButtonInteractable;
    private bool parentToggleInteractable;
    private bool parentDropdownInteractable;
    private bool parentSliderInteractable;


    private void Awake()
    {
        textComponent.text = text;

        windowRect.anchoredPosition = new Vector2(xPos, yPos);

        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xSize);
        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ySize);

        SearchParent();

        Rescale();
    }

    private void OnEnable()
    {
        SetActive(SettingsManager.InfoButtonsOn);
    }


    // ### Functions ###

    private void SearchParent()
    {
        parentButton = transform.parent.GetComponent<Button>();
        parentToggle = transform.parent.GetComponent<Toggle>();
        parentDropdown = transform.parent.GetComponent<TMP_Dropdown>();
        parentSlider = transform.parent.GetComponent<Slider>();
    }

    private void Rescale()
    {
        Vector3 parentScale = transform.parent.localScale;

        transform.localScale = new Vector3(1/parentScale.x, 1/parentScale.y, 1/parentScale.z);
    }

    private void GetParentState()
    {
        if (parentButton != null) parentButtonInteractable = parentButton.interactable;
        if (parentToggle != null) parentToggleInteractable = parentToggle.interactable;
        if (parentDropdown != null) parentDropdownInteractable = parentDropdown.interactable;
        if (parentSlider != null) parentSliderInteractable = parentSlider.interactable;
    }

    public void Open(bool state)
    {
        if (state == true) GetParentState();

        if (parentButton != null) parentButton.interactable = !state & parentButtonInteractable;
        if (parentToggle != null) parentToggle.interactable = !state & parentToggleInteractable;
        if (parentDropdown != null) parentDropdown.interactable = !state & parentDropdownInteractable;
        if (parentSlider != null) parentSlider.interactable = !state & parentSliderInteractable;
    }


    public void SetActive(bool state)
    {
        infoButtonObject.SetActive(state);
    }
}
