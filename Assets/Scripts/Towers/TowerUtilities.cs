using UnityEngine;

public class TowerUtilities : MonoBehaviour {

    public delegate void UpgradeEvent();
    public static UpgradeEvent s_OnUpgrade;

    public Tile CurrentTile { get; set; }

    private void OnDisable()
    {
        HideHighlightedTileVisuals();
    }

    private void HideHighlightedTileVisuals()
    {
        Tile selectedTile = CurrentTile;
        if (selectedTile.CurrentState == TileState.OCCUPIED)
            selectedTile.SetTileVisualsState(TileVisualState.TURRET_SPAWN);
    }

    /// <summary>
    /// Sells a tower and returns 75% of the coins invested in the tower
    /// </summary>
    public void SellTower()
    {
        PlayerData.s_Instance.ChangeCoinAmount(CurrentTile.Tower.TowerData.SellValue);
        CurrentTile.Tower.Sell();
        CurrentTile.Tower = null;
        CurrentTile.CurrentState = TileState.TURRET_SPAWN;
        PopUpManager.s_Instance.HidePopup(PopUpNames.TOWER_MENU);
        HideHighlightedTileVisuals();
    }

    /// <summary>
    /// Upgrade a tower to the next level
    /// </summary>
    public void Upgrade()
    {
        if (CurrentTile.Tower.TowerData.Level < CurrentTile.Tower.TowerData.MaxLevel 
            && PlayerData.s_Instance.Coins >= CurrentTile.Tower.TowerData.UpgradeCost)
        {
            PlayerData.s_Instance.ChangeCoinAmount(-CurrentTile.Tower.TowerData.UpgradeCost);
            CurrentTile.Tower.TowerData = TowerConfig.s_Towers[CurrentTile.Tower.TowerData.Type][CurrentTile.Tower.TowerData.Level];
            if (s_OnUpgrade != null)
            {
                s_OnUpgrade();
            }
        }
        HideHighlightedTileVisuals();
    }
}