using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


[CreateAssetMenu(fileName = "TutosVideos", menuName = "ScriptableObjects/Playlist/Videos", order = 1)]
public class VideoPlaylistSO : ScriptableObject
{
    public VideoClip[] videos;
}
