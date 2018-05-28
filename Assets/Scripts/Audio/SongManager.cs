using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour {

    public delegate void SongChangeEvent(int currentSong,int maxSongs,string songName);
    //An event which gets called everytime the song changes.
    public static SongChangeEvent s_OnChangeSong;

    public delegate void PlaylistCompletion();
    //A delegate which gets called when all of the songs in the playlist are done playing(when the level is done).
    public static PlaylistCompletion s_OnPlaylistComplete;

    public static SongManager s_Instance;

    //All of the songs which are in the playlist
    private Song[] m_Songs;

    //Bass, Drum & Synth Audio sources. In that specific order.
    [SerializeField]private AudioSource[] m_SongAudioSources;

    //Number of the currently playing song in the playlist.
    private int m_SongNumber = 0;
    
    private Coroutine m_SongQueue;

    //List of remaining tracks the song.
    [SerializeField]private List<GameObject> m_RemainingTracksObjects = new List<GameObject>();

    public AudioSource[] SongAudioSources { get { return m_SongAudioSources; } }
    public Song[] Songs
    {
        get { return m_Songs; }
        set { m_Songs = value; }
    }

    private AudioClip m_Bass;
    private AudioClip m_Drum;
    private AudioClip m_Lead;

    private AudioClip[] m_RemainingTracks;


    public int SongNumber
    {
        get { return m_SongNumber; }
        set { m_SongNumber = value; }
    }

    void LoadSong(string songName)
    {
        RemoveExcessiveTracks();
        //Gets the songs tracks from the resource / audio folder
        //Tracks are put in the Resources/Audio folder under the song name
        for (int i = 0; i < Songs.Length; i++)
        {
            string songPath = "Audio/" + songName + "/" + songName;
            string miscTracks = "Audio/" + songName + "/MiscTracks";

            m_Bass = Resources.Load<AudioClip>(songPath + " Bass");
            m_Drum = Resources.Load<AudioClip>(songPath + " Drum");
            m_Lead = Resources.Load<AudioClip>(songPath + " Lead");

            //ResourceRequest remainingClips = Resources.LoadAsync(miscTracks);
            m_RemainingTracks = Resources.LoadAll<AudioClip>(miscTracks);
        }
    }

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        Destroy(gameObject);

        GameManager.s_OnGameStart += StartPlaylist;
        Sceneloader.s_OnSceneLoaded += SetSongUI;
        s_OnPlaylistComplete += OnLevelComplete;
    }

    /// <summary>
    /// Starts the playlist.
    /// </summary>
    private void StartPlaylist()
    {
        PlayNextSongInPlaylist(m_SongNumber);
    }

    /// <summary>
    /// Plays the next song in the playlist.
    /// Changes the UI information with s_OnChangeSong
    /// </summary>
    /// <param name="songNumber">Number of the currently playing song in the playlist. Increases in this function.</param>
    private void PlayNextSongInPlaylist(int songNumber)
    {
        if (m_Songs != null && songNumber < m_Songs.Length)
        {
            m_SongNumber = songNumber;

            SetSongTracks(songNumber);
            StartSong();
            SetSongUI();

            RefreshQueue(songNumber + 1, songLength: m_SongAudioSources[0].clip.length);
        }
        else
        {
            if (s_OnPlaylistComplete != null)
            {
                if(songNumber > 0)
                {
                    s_OnPlaylistComplete();
                    MenuManager.s_Instance.ShowMenu(MenuNames.VICTORY_MENU);
                }

            }
        }
    }

    void SetSongUI()
    {
        if (s_OnChangeSong != null && m_Songs != null)
        {
            s_OnChangeSong((m_SongNumber + 1), m_Songs.Length, m_Songs[m_SongNumber].Songname);
        }
    }

    /// <summary>
    /// Sets the Bass, Drum & Synth of the audio sources to those of the current song.
    /// </summary>
    /// <param name="songNumber">Number of the currently playing song in the playlist.</param>
    private void SetSongTracks(int songNumber)
    {
        LoadSong(m_Songs[songNumber].Songname);
        m_SongAudioSources[0].clip = m_Bass;
        m_SongAudioSources[1].clip = m_Drum;
        m_SongAudioSources[2].clip = m_Lead;

        for (int i = 0; i < m_RemainingTracks.Length; i++)
        {
            GameObject sourceParent = new GameObject();
            sourceParent.name = m_RemainingTracks[i].name;
            sourceParent.transform.SetParent(transform);

            AudioSource source = sourceParent.AddComponent<AudioSource>();
            source.clip = m_RemainingTracks[i];
            source.Play();
            m_RemainingTracksObjects.Add(sourceParent);
        }
    }

    /// <summary>
    /// Plays all of the audio sources, which combined, define the current song.
    /// </summary>
    private void StartSong()
    {
        for (int i = 0; i < m_SongAudioSources.Length; i++)
        {
            m_SongAudioSources[i].Play();
        }
    }

    /// <summary>
    /// Removes all left over tracks which are not Drum/Bass/Synth
    /// </summary>
    private void RemoveExcessiveTracks()
    {
        for (int i = 0; i < m_RemainingTracksObjects.Count; i++)
        {
            m_RemainingTracks[i] = null;
            Destroy(m_RemainingTracksObjects[i]);
            
        }

        m_RemainingTracksObjects.Clear();
    }

    /// <summary>
    /// Refreshes the queue.
    /// </summary>
    /// <param name="songNumber"></param>
    /// <param name="songLength"></param>
    private void RefreshQueue(int songNumber, float songLength = 0)
    {
        if(m_SongQueue != null)
            StopCoroutine(m_SongQueue);

        if (songNumber < m_Songs.Length)
        {
            m_SongQueue = StartCoroutine(QueueSong(songNumber, songLength: m_SongAudioSources[songNumber].clip.length));
        }
        else
        {
            m_SongQueue = StartCoroutine(QueueSong(songNumber, songLength: m_SongAudioSources[songNumber - 1].clip.length));
            Debug.Log("No more songs in playlist.");
        }
    }

    /// <summary>
    /// Puts the next song in the queue.
    /// </summary>
    /// <param name="songNumber">Number of the currently playing song in the playlist.</param>
    /// <param name="songLength">Length of the currently playing song.</param>
    /// <returns></returns>
    IEnumerator QueueSong(int songNumber, float songLength = 0)
    {
        if (songNumber < m_Songs.Length)
        {
            yield return new WaitForSeconds(songLength);
            PlayNextSongInPlaylist(songNumber);
        }
        else
        {

            yield return new WaitForSeconds(songLength);
            s_OnPlaylistComplete();
        }
    }

    /// <summary>
    /// Gets called when the level is completed.
    /// Clears the song queue.
    /// </summary>
    private void OnLevelComplete()
    {
        m_SongNumber = 0;
        m_SongQueue = null;
        m_SongAudioSources[0].clip = null;
        m_SongAudioSources[1].clip = null;
        m_SongAudioSources[2].clip = null;
        RemoveExcessiveTracks();
    }

    public void SkipSong()
    {
        PlayNextSongInPlaylist(m_SongNumber + 1);
    }

    private void OnDisable()
    {
        GameManager.s_OnGameStart -= StartPlaylist;
        Sceneloader.s_OnSceneLoaded -= SetSongUI;
        Sceneloader.s_OnSceneLoaded -= StartPlaylist;
        s_OnPlaylistComplete -= OnLevelComplete;
    }
}
