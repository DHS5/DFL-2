using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class TutoUIManager : MonoBehaviour
{
    private MenuMainManager main;

    [Header("UI components")]
    [SerializeField] private TMP_Dropdown tutoDropdown;
    [SerializeField] private VideoPlayer videoPlayer;


    [Header("Content")]
    [SerializeField] private VideoClip[] tutoVideos;


    // ### Properties ###
    public int TutoNumber
    {
        get { return tutoDropdown.value; }
        set 
        {
            main.DataManager.gameData.tutoNumber = value;
            videoPlayer.clip = tutoVideos[value];
        }
    }

    // ### Functions ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }

    private void Start()
    {
        TutoNumber = 0;
    }
}
