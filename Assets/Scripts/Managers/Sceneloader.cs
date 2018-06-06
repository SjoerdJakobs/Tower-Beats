﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Sceneloader : MonoBehaviour
{
    public static Sceneloader s_Instance;
    public delegate void OnSceneLoaded();
    public static OnSceneLoaded s_OnSceneLoaded;
    private static bool s_AddedCallback;

    private bool m_Loading;
    private bool m_Fading;
    private Image m_Fader;

    private void OnEnable()
    {
        if (!s_AddedCallback)
        {
            SceneManager.sceneLoaded += OnSceneLoadedCallback;
            s_AddedCallback = true;
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

        m_Fader = GetComponentInChildren<Image>();
    }

    private void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        m_Loading = false;
        Fade(false, 0.5f, Ease.InOutSine);
        if(scene.name == "Game")
        {
            if(s_OnSceneLoaded != null)
                s_OnSceneLoaded();
        }
    }

    /// <summary>
    /// Loads a scene by name
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        if (m_Loading) return;

        m_Loading = true;
        Fade(true, 0.5f, Ease.InOutSine, () => {
            SceneManager.LoadSceneAsync(sceneName);
        });
    }

    /// <summary>
    /// Reloads the current scene (Used when pressing the "Retry" button after failing a level
    /// </summary>
    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    private void Fade(bool state, float duration, Ease easing, Action onComplete = null)
    {
        if (m_Fading) return;
        m_Fading = true;
        m_Fader.DOFade(Convert.ToInt32(state), duration).SetEase(easing).OnComplete(delegate { m_Fading = false; if(onComplete != null) onComplete(); });
    }
}