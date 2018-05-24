using System;
using UnityEngine;

[Serializable]
public class Song
{
    public string Songname;
}

public class SongPlaylist : MonoBehaviour {

    [SerializeField]private Song[] m_Playlist;
    
    public void SelectLevel()
    {
        SongManager.s_Instance.Songs = m_Playlist;

        Sceneloader.s_Instance.LoadScene("Game");
    }
}
