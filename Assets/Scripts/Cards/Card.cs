using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Card : MonoBehaviour
{
    [Header("Card's game objects")]

    [Tooltip("")]
    [SerializeField] protected TextMeshProUGUI titleText;

    [Tooltip("Card scriptable object")]
    [HideInInspector] public CardSO cardSO;

    protected virtual void Start()
    {
        titleText.text = cardSO.Title;
    }
}
