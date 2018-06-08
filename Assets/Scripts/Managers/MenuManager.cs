using System.Collections.Generic;
using UnityEngine;

public struct MenuNames
{
    public const string SETTINGS_MENU = "SettingsMenu";
    public const string VICTORY_MENU = "VictoryMenu";
    public const string CREDITS_MENU = "CreditsMenu";
    public const string TOWER_SHOP_MENU = "TowerShopMenu";
    public const string TOWER_MENU = "TowerMenu";
    public const string GAME_OVER_MENU = "GameOverMenu";
    public const string PAUSE_MENU = "PauseMenu";
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager s_Instance;

    public delegate void MenuOpened(Menu menu);
    public static MenuOpened s_OnMenuOpened;

    public delegate void MenuClosed(Menu menu);
    public static MenuClosed s_OnMenuClosed;

    public static bool s_IsPaused;

    [SerializeField] private List<Menu> m_Menus = new List<Menu>();
    private Menu m_CurrentOpenMenu;

    public bool IsAnyMenuOpen
    {
        get { return (m_CurrentOpenMenu != null) ? true : false; }
    }

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

    /// <summary>
    /// Shows a menu determined by the string passed as parameter
    /// </summary>
    /// <param name="menuName"></param>
    public void ShowMenu(string menuName)
    {
        for (int i = 0; i < m_Menus.Count; i++)
        {
            if (m_Menus[i].name == menuName)
                ShowMenu(m_Menus[i]);
        }
    }

    /// <summary>
    /// Shows a menu, component has to inherit from Menu
    /// </summary>
    /// <param name="menu">The menu to open</param>
    public void ShowMenu(Menu menu)
    {
        if (IsAnyMenuOpen)
        {
            m_CurrentOpenMenu.Hide();
            m_CurrentOpenMenu = null;
        }
        menu.Show();
        m_CurrentOpenMenu = menu;
        if (s_OnMenuOpened != null) s_OnMenuOpened(menu);
    }

    public void ShowMenu(string menuName,Vector3 position)
    {
        Camera cam = Camera.main;
        for (int i = 0; i < m_Menus.Count; i++)
        {
            if (m_Menus[i].name == menuName)
            {
                ShowMenu(m_Menus[i]);
                m_Menus[i].transform.position = cam.WorldToScreenPoint(position);
            }
        }
    }

    public void HideMenu(string menuName)
    {
        for (int i = 0; i < m_Menus.Count; i++)
        {
            if(m_Menus[i].name == menuName)
            {
                HideMenu(m_Menus[i]);
            }
        }
    }

    public void HideMenu(Menu menu)
    {
        menu.Hide();
    }

    public void ToggleMenu(Menu menu)
    {
        if (IsAnyMenuOpen)
        {
            if (m_CurrentOpenMenu != menu)
            {
                m_CurrentOpenMenu.Hide();
                menu.Show();
                m_CurrentOpenMenu = menu;
                if (s_OnMenuOpened != null) s_OnMenuOpened(menu);
            }
            else
            {
                m_CurrentOpenMenu = null;
                menu.Hide();
            }
        }
        else
        {
            menu.Show();
            m_CurrentOpenMenu = menu;
            if (s_OnMenuOpened != null) s_OnMenuOpened(menu);
        }
    }

    public void LoadScene(string sceneName)
    {
        Sceneloader.s_Instance.LoadScene(sceneName);
    }

    public void ReloadGameScene()
    {
        Sceneloader.s_Instance.LoadGameSceneWithLevel(Sceneloader.s_Instance.m_LevelToLoad);
    }
}