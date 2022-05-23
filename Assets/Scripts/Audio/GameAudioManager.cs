using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    private DataManager dataManager;
    
    
    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = DataManager.InstanceDataManager;
    }


    // ### Functions ###


    public void GenerateAudio()
    {
        if (dataManager.audioData.soundOn)
        {
            MuteSound(false);

            if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE || main.GameManager.gameData.gameMode == GameMode.DRILL)
            {
                StopAmbianceAudios();
            }

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
        foreach (AudioSource a in FindObjectsOfType<AudioSource>())
        {
            if (!a.CompareTag("MusicSource"))
            {
                a.volume = dataManager.audioData.soundVolume;
            }
        }
    }

    /// <summary>
    /// Mutes the game sounds
    /// </summary>
    /// <param name="tmp"></param>
    public void MuteSound(bool tmp)
    {
        foreach (AudioSource a in FindObjectsOfType<AudioSource>())
        {
            if (!a.CompareTag("MusicSource"))
            {
                a.mute = tmp;
            }
        }
    }


    public void BoohAudio()
    {
        foreach (AudioSource a in main.FieldManager.stadium.boohAS)
        {
            a.gameObject.SetActive(true);
        }
    }

    public void OuuhAudio()
    {
        foreach (AudioSource a in main.FieldManager.stadium.ouuhAS)
        {
            a.gameObject.SetActive(true);
        }
    }

    public void StopAmbianceAudios()
    {
        foreach (AudioSource a in main.FieldManager.stadium.entryAS)
            a.gameObject.SetActive(false);
        foreach (AudioSource a in main.FieldManager.stadium.exitAS)
            a.gameObject.SetActive(false);
        foreach (AudioSource a in main.FieldManager.stadium.bleachersAS)
            a.gameObject.SetActive(false);
    }
}
