using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCheck : MonoBehaviour {
    
    public static PauseCheck s_Instance;

    [SerializeField]
    private bool m_onPause;

    public static event System.Action<bool> Pause;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        SongManager.s_OnPlaylistComplete += PauseOn;
    }

    private void Init()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseOff()
    {
        m_onPause = false;
        if (Pause != null)
        {
            Pause(m_onPause);
        }
    }

    public void PauseOn()
    {
        m_onPause = true;
        if (Pause != null)
        {
            Pause(m_onPause);
        }
    }

    public void TogglePause()
    {
        m_onPause = !m_onPause;
        if (Pause != null)
        {
            Resources.UnloadUnusedAssets();
            Pause(m_onPause);
        }
    }

    private void OnDisable()
    {
        SongManager.s_OnPlaylistComplete -= PauseOn;
    }
}
