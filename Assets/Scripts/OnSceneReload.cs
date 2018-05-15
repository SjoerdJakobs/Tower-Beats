using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneReload : MonoBehaviour {

    public static OnSceneReload s_Instance;

    [SerializeField]
    private bool m_onPause;

    public static event System.Action Reload;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (s_Instance == null)
        {
            s_Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ye");
        Reload();
    }
}
