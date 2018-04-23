using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneloader : MonoBehaviour
{
    public static Sceneloader s_Instance;
    public delegate void OnSceneLoaded();
    public static OnSceneLoaded s_OnSceneLoaded;
    private static bool s_AddedCallback;

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
            //DontDestroyOnLoad(this.gameObject);
        }
        //else
        //Destroy(gameObject);
    }

    private void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Lorenzo")
        {
            s_OnSceneLoaded();
        }
    }

    /// <summary>
    /// Loads a scene by name
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    /// <summary>
    /// Reloads the current scene (Used when pressing the "Retry" button after failing a level
    /// </summary>
    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}