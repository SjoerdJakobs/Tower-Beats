using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour {

    public delegate void UpgradeTowerEvent(Tower towerToUpgrade);
    public static UpgradeTowerEvent OnUpgradeTower;

    void OnEnable()
    {
        OnUpgradeTower += UpgradeTower;
    }

    void UpgradeTower(Tower towerToUpgrade)
    {
        Debug.Log(towerToUpgrade.TowerData.Level + " out of " + towerToUpgrade.TowerData.MaxLevel);
        if(towerToUpgrade.TowerData.Level < towerToUpgrade.TowerData.MaxLevel && PlayerData.s_Instance.Coins >= towerToUpgrade.TowerData.UpgradeCost)
        {
            PlayerData.s_Instance.ChangeCoinAmount(-towerToUpgrade.TowerData.UpgradeCost);
            towerToUpgrade.TowerData = TowerConfig.s_Towers[towerToUpgrade.TowerData.Type][towerToUpgrade.TowerData.Level];
            Debug.Log("Level" + towerToUpgrade.TowerData.Level);
        }
    }

    void OnDisable()
    {
        OnUpgradeTower -= UpgradeTower;
    }
}
