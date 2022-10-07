using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImageCard : Card
{
    [Tooltip("Image")]
    [SerializeField] protected Image image;


    protected override void Start()
    {
        base.Start();

        image.sprite = cardSO.mainSprite;
    }
}
