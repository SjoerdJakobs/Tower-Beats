using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlaylist : MonoBehaviour {

    [SerializeField]private AudioClip[] m_Playlist;
    
    public void SelectLevel()
    {
        SongManager.s_Instance.Songs = m_Playlist;

        for (int i = 0; i < SongManager.s_Instance.Songs.Length; i++)
        {
            //Debug.Log(SongManager.s_Instance.Songs[i].name);
        }

        Sceneloader.s_Instance.LoadScene("Lorenzo");
    }
}
