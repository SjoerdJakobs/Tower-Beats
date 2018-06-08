using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool m_IsMenuOpen;
    public bool IsMenuOpen { get { return m_IsMenuOpen; } set { m_IsMenuOpen = value; } }

    public virtual void Show()
    {
        m_IsMenuOpen = true;
        gameObject.SetActive(true);
        MenuManager.s_IsPaused = true;
    }

    public virtual void Hide()
    {
        m_IsMenuOpen = false;
        gameObject.SetActive(false);

        if (MenuManager.s_OnMenuClosed != null) MenuManager.s_OnMenuClosed(this);
        MenuManager.s_IsPaused = false;
    }
}