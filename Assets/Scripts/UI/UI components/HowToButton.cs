using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HowToButton : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private UnityEvent[] events;

    [Header("UI components")]
    [SerializeField] private Toggle[] toggles;
    [SerializeField] private Image image;


    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            toggles[i].gameObject.SetActive(true);
        }
    }


    public void SetSprite(int i)
    {
        image.sprite = sprites[i];
        if (events.Length > i) events[i]?.Invoke();
    }
}
