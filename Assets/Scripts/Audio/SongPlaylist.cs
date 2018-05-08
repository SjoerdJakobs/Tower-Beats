using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Song
{
    public AudioClip Drum;
    public AudioClip Bass;
    public AudioClip Synth;
}

public class SongPlaylist : MonoBehaviour {

    [SerializeField]private Song[] m_Playlist;
    
    public void SelectLevel()
    {
        SongManager.s_Instance.Songs = m_Playlist;

        Sceneloader.s_Instance.LoadScene("Lorenzo");
    }
}
