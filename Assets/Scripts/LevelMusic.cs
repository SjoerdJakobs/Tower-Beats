using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour {

    public static LevelMusic s_Instance;

    [SerializeField]private AudioClip[] m_Songs;
    public AudioClip[] Songs
    {
        get { return m_Songs; }
        set { m_Songs = value; }
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
    }
}
