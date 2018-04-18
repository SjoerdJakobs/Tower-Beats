using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUtilities : MonoBehaviour {

    /// <summary>
    /// Sells a tower and returns 75% of the coins invested in the tower
    /// </summary>
    public void SellTower()
    {
        PlayerData.s_Instance.ChangeCoinAmount(PlayerData.s_Instance.SelectedTower.TowerData.SellValue);
        Destroy(PlayerData.s_Instance.SelectedTower.gameObject);
        TowerMenu.s_Instance.Hide();
    }

    /// <summary>
    /// Upgrade a tower to the next level
    /// </summary>
    public void Upgrade()
    {
        if (PlayerData.s_Instance.SelectedTower.TowerData.Level < PlayerData.s_Instance.SelectedTower.TowerData.MaxLevel && PlayerData.s_Instance.Coins >= PlayerData.s_Instance.SelectedTower.TowerData.UpgradeCost)
        {
            PlayerData.s_Instance.ChangeCoinAmount(-PlayerData.s_Instance.SelectedTower.TowerData.UpgradeCost);
            PlayerData.s_Instance.SelectedTower.TowerData = TowerConfig.s_Towers[PlayerData.s_Instance.SelectedTower.TowerData.Type][PlayerData.s_Instance.SelectedTower.TowerData.Level];
            Debug.Log("Level" + PlayerData.s_Instance.SelectedTower.TowerData.Level);
            TowerMenu.s_Instance.ShowTowerMenu();
        }
    }
}
