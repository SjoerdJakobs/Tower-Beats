using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSongManagerScript : MonoBehaviour {

	public void GetCurrentSong()
    {
        Debug.Log(SongManager.s_Instance.GetCurrentSong().name);
    }

    public void GetNextSong()
    {
        if(SongManager.s_Instance.GetNextSong() != null)
            Debug.Log(SongManager.s_Instance.GetNextSong().name);
    }

    public void SkipSong()
    {
        SongManager.s_Instance.SkipSong();
    }
}
