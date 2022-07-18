using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lock : MonoBehaviour
{
    public TextMeshProUGUI text;

    public bool Locked
    {
        set
        {
            gameObject.SetActive(value);
            transform.parent.GetComponent<Toggle>().interactable = !value;
        }
    }

    private void Awake()
    {
        Locked = true;
    }
}
