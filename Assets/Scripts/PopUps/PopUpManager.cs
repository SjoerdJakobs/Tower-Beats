using System.Collections.Generic;
using UnityEngine;

public class PopUpNames
{
    public const string TOWER_SHOP_MENU = "TowerShopPopUp";
    public const string TOWER_MENU = "TowerInfoPopUp";
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
    public void ShowPopUp(string popUpName, Vector3 position)
    {
        Camera cam = Camera.main;
        for (int i = 0; i < m_PopUps.Count; i++)
        {
            if (m_PopUps[i].name == popUpName)
            {
                m_PopUps[i].gameObject.SetActive(true);
                m_PopUps[i].transform.position = position; //Sets the position
            }
            else
            {
                //Disable the other pop ups
                m_PopUps[i].gameObject.SetActive(false);
            }
        }
    }
}