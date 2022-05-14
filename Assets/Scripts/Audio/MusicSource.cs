using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    public static MusicSource InstanceMusicSource { get; private set; }


    [HideInInspector] public AudioSource audioSource;


    private void Awake()
    {
        if (InstanceMusicSource != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceMusicSource = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }
}
