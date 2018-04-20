using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour {

    public delegate void SongChangeEvent(int currentSong,int maxSongs,string songName);
    public static SongChangeEvent s_OnChangeSong;

    public static SongManager s_Instance;

    [SerializeField]private AudioClip[] m_Songs;
    public AudioClip[] Songs
    {
        get { return m_Songs; }
        set { m_Songs = value; }
    }

    private AudioClip m_NextSong;

    private int m_SongNumber = 0;

    private AudioSource m_SongAudioSource;
    public AudioSource SongAudioSource { get { return m_SongAudioSource; } }

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        Destroy(gameObject);

        Sceneloader.s_OnSceneLoaded += CreateAudioSource;
    }

    public void CreateAudioSource()
    {
        if (m_SongAudioSource == null)
            m_SongAudioSource = gameObject.AddComponent<AudioSource>();

        PlayNextSongInPlaylist(m_SongNumber);
    }

    private void PlayNextSongInPlaylist(int songNumber)
    {
        if (songNumber < m_Songs.Length)
        {
            m_SongNumber = songNumber;
            m_SongAudioSource.clip = m_Songs[songNumber];
            m_SongAudioSource.Play();
            if (s_OnChangeSong != null)
            {
                s_OnChangeSong(m_SongNumber, m_Songs.Length, m_Songs[m_SongNumber].name);
            }
            StartCoroutine(QueueSong(songNumber + 1, songLength: m_SongAudioSource.clip.length));
        }
        else
            m_NextSong = null;
    }

    IEnumerator QueueSong(int songNumber, float songLength = 0)
    {
        if (songNumber < m_Songs.Length)
        {
            m_NextSong = m_Songs[songNumber];
            yield return new WaitForSeconds(songLength);
            PlayNextSongInPlaylist(songNumber);
        }
        else
            Debug.Log("End of playlist");
    }

    public AudioClip GetCurrentSong()
    {
        return m_SongAudioSource.clip;
    }

    public AudioClip GetNextSong()
    {
        if(m_SongNumber < m_Songs.Length -1)
        {
            return m_NextSong;
        }
        else
        {
            Debug.Log("This is the final song in the playlist");
            return null;
        }
    }

    public void SkipSong()
    {
        PlayNextSongInPlaylist(m_SongNumber + 1);
    }

    private void OnDisable()
    {
        Sceneloader.s_OnSceneLoaded -= CreateAudioSource;
    }
}
