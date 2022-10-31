using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stadium : MonoBehaviour
{
    [Header("Stadium's audio sources")]
    [SerializeField] private AudioSource[] bleachersAS;
    [SerializeField] private AudioClip bleachersClip;
    public bool crowd;

    [Header("Lights")]
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject lightsToSwitchOff;

    public GameObject SpawnPosition
    {
        get
        {
            if (spawnPositions.Length == 1 || FindObjectOfType<Player>() == null) return spawnPositions[0];
            else return spawnPositions[FindObjectOfType<Player>().transform.position.x <= 0 ? 0 : 1];
        }
    }
    public GameObject[] spawnPositions;
    public Camera stadiumCamera;

    public Material enemyMaterial;
    public Color fogColor;
    public float coinsPercentage;

    [SerializeField] private ParticleSystem rain;


    // ### Functions ###

    private void Start()
    {
        SetBleachersSound(bleachersClip);
        StartBleachersSound();
    }


    public void Rain()
    {
        rain.gameObject.SetActive(true);
    }

    public void SwitchLightsOff()
    {
        if (lightsToSwitchOff != null) lightsToSwitchOff.SetActive(false);
    }

    private void SetBleachersSound(AudioClip clip)
    {
        foreach (AudioSource a in bleachersAS)
            a.clip = clip;
    }
    public void StartBleachersSound()
    {
        foreach (AudioSource a in bleachersAS)
            a.Play();
    }

    public void MuteBleachersSound()
    {
        foreach (AudioSource a in bleachersAS)
            a.enabled = false;
    }
}
