using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Songs class.
/// </summary>
[Serializable]
public class Song
{
    public Song(string songName)
    {
        Songname = songName;
    }

    public string Songname;
}

public class SongPlaylist : MonoBehaviour {

    /// <summary>
    /// Playlist of songs.
    /// </summary>
    [SerializeField]private List<Song> m_Playlist;
    
    /// <summary>
    /// Selected the current selected level.
    /// </summary>
    public void SelectLevel()
    {
        SongManager.s_Instance.Songs = m_Playlist;

        Sceneloader.s_Instance.LoadScene("Game");
    }
}
