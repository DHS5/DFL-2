using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownLock : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        Instantiate(prefab, gameObject.transform);
    }
}
