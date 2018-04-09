using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager s_Instance;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
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

    public void ToggleMusic(bool state)
    {
        //Toggle Music
    }

    public void ToggleSFX(bool state)
    {
        //Toggle SFX
    }

    public void ToggleMaster(bool state)
    {
        //Toggle Master
    }

    public void SetMusicVolume(float value)
    {
        // Set Music Volume in Audio Mixer
    }

    public void SetSFXVolume(float value)
    {
        // Set SFX Volume in Audio Mixer
    }

    public void SetMasterVolume(float value)
    {
        // Set Master Volume in Audio Mixer
    }
}
