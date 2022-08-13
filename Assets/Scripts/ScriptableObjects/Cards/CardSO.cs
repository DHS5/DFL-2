using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CardSO : ScriptableObject
{
    public string Title;
    public Sprite mainSprite;

    public virtual void SetActive() { }
    public virtual void SetActive(int index) { }
}
