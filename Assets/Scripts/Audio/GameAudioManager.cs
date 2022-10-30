using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;
    
    private AudioSource[] audioSources;
    private AudioSource[] soundSources;

    [Header("Sounds bank")]
    [SerializeField] private AudioClip[] playerEntryClips;
    [SerializeField] private AudioClip[] touchdownCelebrationClips;
    [SerializeField] private AudioClip[] bigplayReactionClips;
    [SerializeField] private AudioClip[] surpriseClips;
    [SerializeField] private AudioClip[] bouhClips;
    [SerializeField] private AudioClip[] winClips;

    [Header("Audio source")]
    [SerializeField] private AudioSource audioSource;



    private bool crowdSound = true;

    // ### Properties ###

    public float SoundVolume
    {
        set
        {
            foreach (AudioSource a in soundSources)
            {
                a.volume = value;
            }
        }
    }
    public bool SoundOn
    {
        get { return main.DataManager.audioData.soundOn; }
        set { MuteSound(!value); }
    }


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    public void Pause(bool state)
    {
        if (state) GetSoundSources();

        MuteSound(state || !SoundOn);
    }


    public void GetSoundSources()
    {
        audioSources = FindObjectsOfType<AudioSource>();

        List<AudioSource> list = new List<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            if (!source.CompareTag("MusicSource"))
                list.Add(source);
        }
        soundSources = list.ToArray();
    }

    public void GenerateAudio()
    {
        GetSoundSources();

        // here case by case
        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE || (main.GameManager.gameData.gameMode == GameMode.DRILL && main.GameManager.gameData.gameDrill != GameDrill.PARKOUR))
        {
            crowdSound = false;
            main.FieldManager.stadium.DeactivateBleachersSound();
        }
        else
        {
            PlayerEntry();
        }

        AudioData data = main.DataManager.audioData;
        if (data.soundOn)
        {
            MuteSound(false);
            ActuSoundVolume();
        }
        else
        {
            MuteSound(true);
        }
    }



    /// <summary>
    /// Puts the volume of all the sound audio sources to the desired volume
    /// </summary>
    private void ActuSoundVolume()
    {
        foreach (AudioSource a in soundSources)
        {
            a.volume = main.DataManager.audioData.soundVolume;
        }
    }

    /// <summary>
    /// Mutes the game sounds
    /// </summary>
    /// <param name="tmp"></param>
    public void MuteSound(bool tmp)
    {
        foreach (AudioSource a in soundSources)
        {
            a.mute = tmp;
        }
    }

    public void Lose()
    {
        if (crowdSound)
        {
            OuuhAudio();
            Invoke(nameof(BouhAudio), audioSource.clip.length);
        }
    }

    private void BouhAudio()
    {
        audioSource.clip = bouhClips[Random.Range(0, bouhClips.Length)];
        audioSource.Play();
    }

    private void OuuhAudio()
    {
        audioSource.clip = surpriseClips[Random.Range(0, surpriseClips.Length)];
        audioSource.Play();
    }

    public void PlayerEntry()
    {
        if (crowdSound)
        {
            audioSource.clip = playerEntryClips[Random.Range(0, playerEntryClips.Length)];
            audioSource.Play();
        }
    }
    public void TouchdownCelebration()
    {
        if (crowdSound)
        {
            audioSource.clip = touchdownCelebrationClips[Random.Range(0, touchdownCelebrationClips.Length)];
            audioSource.Play();
        }
    }

    public void Win()
    {
        audioSource.clip = winClips[Random.Range(0, winClips.Length)];
        audioSource.Play();
    }
}
