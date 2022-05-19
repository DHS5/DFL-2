using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSource : MonoBehaviour
{
    public static MusicSource InstanceMusicSource { get; private set; }


    [HideInInspector] public AudioSource audioSource;

    [Header("Music's list")]
    [Tooltip("Music's list")]
    [SerializeField] private AudioClip[] musics;

    [Header("Music's volume slider")]
    [Tooltip("Music's volume slider")]
    [SerializeField] private Slider musicSlider;


    private int musicNumber;
    private float musicVolume;
    private bool musicOn;
    private bool loopOn;


    // ### Properties ###
    /// <summary>
    /// Index of the music in the musics list
    /// </summary>
    public int MusicNumber
    {
        get { return musicNumber; }
        set { musicNumber = value; }
    }
    /// <summary>
    /// Whether the music is playing
    /// </summary>
    public bool MusicOn
    {
        get { return musicOn; }
        set
        {
            if (value == true && !musicOn) audioSource.UnPause();
            else if (value == false && musicOn) audioSource.Pause();

            musicOn = value;
        }
    }
    /// <summary>
    /// Volume of the music
    /// </summary>
    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            audioSource.volume = musicVolume;
        }
    }
    /// <summary>
    /// Whether the musics are looping
    /// </summary>
    public bool LoopOn
    {
        get { return loopOn; }
        set
        {
            loopOn = value;
            audioSource.loop = value;
        }
    }




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

    private void Start()
    {
        PlayFromBeginning(0); // A remplacer !!!
        MusicOn = true; //
        MusicVolume = 0.5f; //  par LoadAudioData(dataManager.audioData);
        musicSlider.value = musicVolume;
    }

    private void Update()
    {
        if (musicOn && !audioSource.isPlaying) // Plays the next music when the previous has ended
        {
            NextMusic();
        }
    }


    // ### Functions ###

    private void LoadAudioData(AudioData data)
    {
        MusicOn = data.musicOn;
        MusicVolume = data.musicVolume;
        MusicNumber = data.musicNumber;

        LoopOn = data.loopOn;
    }


    /// <summary>
    /// Plays the chosen clip from the beginning
    /// </summary>
    /// <param name="clipNumber">Number of the music clip</param>
    protected void PlayFromBeginning(int clipNumber)
    {
        audioSource.clip = musics[clipNumber];
        audioSource.time = 0f;
        audioSource.Play();
    }
    /// <summary>
    /// Plays the chosen clip from the chosen start time
    /// </summary>
    /// <param name="clipNumber">Number of the music clip</param>
    /// <param name="startTime">Start time of the music clip</param>
    protected void PlayFromTime(int clipNumber, float startTime)
    {
        audioSource.clip = musics[clipNumber];
        audioSource.time = startTime;
        audioSource.Play();
    }

    /// <summary>
    /// Plays the next music in the list
    /// </summary>
    public void NextMusic()
    {
        if (musicNumber == musics.Length - 1) MusicNumber = 0;
        else MusicNumber++;

        PlayFromBeginning(musicNumber);
    }
    /// <summary>
    /// Plays the previous music in the list
    /// </summary>
    public void PreviousMusic()
    {
        if (musicNumber == 0) MusicNumber = musics.Length - 1;
        else MusicNumber--;

        PlayFromBeginning(musicNumber);
    }
}
