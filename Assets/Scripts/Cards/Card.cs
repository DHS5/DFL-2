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
    public GameObject prefab;


    [Header("Card's attributes")]
    [Tooltip("")]
    [SerializeField] protected string title;


    protected virtual void Start()
    {
        titleText.text = title;
    }
}
