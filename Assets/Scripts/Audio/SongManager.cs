using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Song
{
    public string Title;
    public string Artist;
    public float BPM;
    public AudioClip Audio;
}

public class SongManager : MonoBehaviour
{
    public static SongManager s_Instance;

    public delegate void StartPlayingSong(Song song);
    public static StartPlayingSong s_OnStartPlayingSong;

    public delegate void StopPlayingSong(Song song);
    public static StopPlayingSong s_OnStopPlayingSong;

    private bool m_SongPlaying;
    public bool SongPlaying { get { return m_SongPlaying; } }

    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private List<Song> m_Songs = new List<Song>();
    public List<Song> Songs { get { return m_Songs; } }

    private void Init()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);
    }

	private void Awake ()
    {
        Init();
        Song song = GetSongBySongTitle("People Say");
        m_AudioSource.clip = song.Audio;
        m_AudioSource.Play();
        if (s_OnStartPlayingSong != null) s_OnStartPlayingSong(song);
        m_SongPlaying = true;
    }

    public string GetArtistNameBySongTitle(string title)
    {
        return m_Songs.Find(x => x.Title == title).Artist;
    }

    public float GetBPMBySongTitle(string title)
    {
        return m_Songs.Find(x => x.Title == title).BPM;
    }

    public Song GetSongBySongTitle(string title)
    {
        return m_Songs.Find(x => x.Title == title);
    }

    public AudioClip GetAudioBySongTitle(string title)
    {
        return m_Songs.Find(x => x.Title == title).Audio;
    }

    public float GetIntervalByBPM(float BPM)
    {
        return (60 / BPM);
    }
}
