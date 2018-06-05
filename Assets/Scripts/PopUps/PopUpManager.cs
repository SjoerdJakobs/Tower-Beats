using System.Collections.Generic;
using UnityEngine;

public class PopUpNames
{
    public const string TOWER_SHOP_MENU = "Tower Shop";
    public const string TOWER_MENU = "Tower Upgrades";
}

public class PopUpManager : MonoBehaviour {

    public static PopUpManager s_Instance;
    [SerializeField]private List<PopUp> m_PopUps = new List<PopUp>();


    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Shows a small pop up window
    /// </summary>
    /// <param name="popUpName">Pop up to open</param>
    /// <param name="position">Spawn position of the pop up</param>
    public void ShowPopUp(string popUpName, Tile tile)
    {
        for (int i = 0; i < m_PopUps.Count; i++)
        {
            if (m_PopUps[i].name.ToUpper() == popUpName.ToUpper())
            {
                if(m_PopUps[i].LastClickedFromTile != tile)
                    m_PopUps[i].Show(tile);
            }
            else
            {
                //Disable the other pop ups
                m_PopUps[i].Hide();
            }
        }
    }

    public void HidePopUp(string popUpName)
    {
        PopUp popup = m_PopUps.Find(x => x.name.ToUpper() == popUpName.ToUpper()) as PopUp;
        HidePopUp(popup);
    }

    public void HidePopUp(PopUp popup)
    {
        popup.ClearLastClickedTile();
        popup.Hide();
    }

    public void HideAll()
    {
        for (int i = 0; i < m_PopUps.Count; i++)
        {
            if (m_PopUps[i].isActiveAndEnabled)
                HidePopUp(m_PopUps[i]);
        }
    }

    public bool IsPopUpOpenOnTile(Tile tile)
    {
        for (int i = 0; i < m_PopUps.Count; i++)
        {
            if (m_PopUps[i].LastClickedFromTile == tile)
                return true;
        }
        return false;
    }
}