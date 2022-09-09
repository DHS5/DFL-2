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


    public void Rain()
    {
        rain.gameObject.SetActive(true);
    }
}
