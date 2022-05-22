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



    public void BoohAudio()
    {
        foreach (AudioSource a in boohAS)
        {
            a.gameObject.SetActive(true);
        }
    }

    public void OuuhAudio()
    {
        foreach (AudioSource a in ouuhAS)
        {
            a.gameObject.SetActive(true);
        }
    }

    public void StopAmbianceAudios()
    {
        foreach (AudioSource a in entryAS)
            a.gameObject.SetActive(false);
        foreach (AudioSource a in exitAS)
            a.gameObject.SetActive(false);
        foreach (AudioSource a in bleachersAS)
            a.gameObject.SetActive(false);
    }
}
