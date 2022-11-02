using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectif : MonoBehaviour
{
    [Tooltip("Objectif Manager of the game")]
    [HideInInspector] public ObjectifManager objectifManager;

    private bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !done)
        {
            DestroyObj();
            done = true;
        }
    }

    private void DestroyObj()
    {
        gameObject.SetActive(false);
        Destroy(this);
        Destroy(gameObject);
        objectifManager.NextObj();
    }
}
