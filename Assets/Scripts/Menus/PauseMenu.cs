public class PauseMenu : Menu
{    
    public static PauseMenu s_Instance;

    private void Awake()
    {
        Init();
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
}