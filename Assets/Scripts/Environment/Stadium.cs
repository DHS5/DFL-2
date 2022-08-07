using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stadium : MonoBehaviour
{
    [Header("Stadium's audio sources")]
    public AudioSource[] entryAS;
    public AudioSource[] exitAS;
    public AudioSource[] bleachersAS;
    public AudioSource[] ouuhAS;
    public AudioSource[] boohAS;

    public GameObject SpawnPosition;
    public Camera stadiumCamera;

    public Material enemyMaterial;

    [SerializeField] private ParticleSystem rain;


    public void Rain()
    {
        rain.gameObject.SetActive(true);
    }
}
