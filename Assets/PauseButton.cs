using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public bool Paused { get; set; }

    [SerializeField]private GameObject m_PauseIcon;
    [SerializeField]private GameObject m_PlayIcon;

    private void Start()
    {
        HandleIcon();
    }

    public void TogglePause()
    {
        Paused = !Paused;
        PauseCheck.s_Instance.TogglePause();
        HandleIcon();
    }

    private void HandleIcon()
    {
        m_PauseIcon.SetActive(!Paused);
        m_PlayIcon.SetActive(Paused);
    }
}
