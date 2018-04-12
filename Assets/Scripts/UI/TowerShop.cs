using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour {

    [SerializeField] private List<Tower> m_Towers = new List<Tower>();

    public void PurchaseTower(string towerType)
    {
       if(TowerConfig.s_Towers[towerType][0].BuyCost <= PlayerData.s_Instance.Coins)
        {
            //Gets the cost from the towers data
            PlayerData.s_Instance.ChangeCoinAmount(-TowerConfig.s_Towers[towerType][0].BuyCost);
           
            //Spawns a tower
            Tower newTower;
            switch (towerType)
            {
                case TowerTypeTags.BASS_TOWER:
                    newTower = Instantiate(m_Towers[0]);
                    newTower.TowerData = TowerConfig.s_Towers[towerType][0];
                    break;
                case TowerTypeTags.DRUM_TOWER:
                    newTower = Instantiate(m_Towers[1]);
                    break;
                case TowerTypeTags.SYNTH_TOWER:
                    newTower = Instantiate(m_Towers[2]);
                    break;

            }
            
        }
    }
}