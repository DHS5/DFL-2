using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Playlist", menuName = "ScriptableObjects/Playlist", order = 1)]
public class PlaylistSO : ScriptableObject
{
    public AudioClip[] musics;
}
