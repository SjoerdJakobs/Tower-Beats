using UnityEngine;

public class TowerUtilities : MonoBehaviour {

    public delegate void UpgradeEvent();
    public static UpgradeEvent s_OnUpgrade;

    /// <summary>
    /// Sells a tower and returns 75% of the coins invested in the tower
    /// </summary>
    public void SellTower()
    {
        PlayerData.s_Instance.ChangeCoinAmount(HexGrid.s_Instance.SelectedTile.Tower.TowerData.SellValue);
        Destroy(HexGrid.s_Instance.SelectedTile.Tower.gameObject);
        HexGrid.s_Instance.SelectedTile.Tower = null;
        HexGrid.s_Instance.SelectedTile.CurrentState = TileState.OPEN;
        MenuManager.s_Instance.HideMenu(MenuNames.TOWER_MENU);
    }

    /// <summary>
    /// Upgrade a tower to the next level
    /// </summary>
    public void Upgrade()
    {
        if (HexGrid.s_Instance.SelectedTile.Tower.TowerData.Level < HexGrid.s_Instance.SelectedTile.Tower.TowerData.MaxLevel 
            && PlayerData.s_Instance.Coins >= HexGrid.s_Instance.SelectedTile.Tower.TowerData.UpgradeCost)
        {
            PlayerData.s_Instance.ChangeCoinAmount(-HexGrid.s_Instance.SelectedTile.Tower.TowerData.UpgradeCost);
            HexGrid.s_Instance.SelectedTile.Tower.TowerData = TowerConfig.s_Towers[HexGrid.s_Instance.SelectedTile.Tower.TowerData.Type][HexGrid.s_Instance.SelectedTile.Tower.TowerData.Level];
            if (s_OnUpgrade != null)
            {
                s_OnUpgrade();
            }
        }
    }
}
