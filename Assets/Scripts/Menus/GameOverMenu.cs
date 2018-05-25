using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Menu {

    //temp
	public void ResetPlaylist()
    {
        SongManager.s_OnPlaylistComplete();
    }
}
