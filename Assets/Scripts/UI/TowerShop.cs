using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour {

    [SerializeField] private List<Tower> m_Towers = new List<Tower>();


    void BuyTower(Tower towerToBuy)
    {        
        if (towerToBuy.TowerData.BuyCost <= PlayerData.s_Instance.Coins)
        {
            PlayerData.s_Instance.Coins -= towerToBuy.TowerData.BuyCost;
        }
    }
}
