using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TerrainCard : Card
{
    protected virtual void Awake()
    {
        image.sprite = cardSO.sprite150x100;
    }

    //public override bool InfoActive { get => false; set => { } }
    public override bool InfoActive { get => false; set => infoToggle = null; }
}
