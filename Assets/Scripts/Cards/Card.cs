using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Card : MonoBehaviour
{
    [Header("Card's game objects")]
    [Tooltip("")]
    [SerializeField] protected Image image;

    [Tooltip("")]
    [SerializeField] protected TextMeshProUGUI titleText;

    [Tooltip("")]
    [SerializeField] protected Toggle infoToggle;

    [Tooltip("Card scriptable object")]
    [HideInInspector] public CardSO cardSO;


    public virtual bool InfoActive
    {
        get { return infoToggle.isOn; }
        set { infoToggle.isOn = value; }
    }


    protected virtual void Start()
    {
        titleText.text = cardSO.Title;
        image.sprite = cardSO.mainSprite;
    }
}
