using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopPopUp : PopUp {

    [SerializeField] private List<Tower> m_Towers = new List<Tower>();

    [SerializeField] private Color m_AvailableColor;
    [SerializeField] private Color m_UnavailableColor;
    [SerializeField] private Text m_BassTowerCost;
    [SerializeField] private Text m_DrumTowerCost;
    [SerializeField] private Text m_LeadTowerCost;

    private void OnEnable()
    {
        float coins = PlayerData.s_Instance.Coins;

        CompareCostAndSetTextColor(coins, TowerConfig.s_Towers[TowerTypeTags.BASS_TOWER][0].BuyCost, m_BassTowerCost);
        CompareCostAndSetTextColor(coins, TowerConfig.s_Towers[TowerTypeTags.DRUM_TOWER][0].BuyCost, m_DrumTowerCost);
        CompareCostAndSetTextColor(coins, TowerConfig.s_Towers[TowerTypeTags.LEAD_TOWER][0].BuyCost,m_LeadTowerCost);
    }

    void CompareCostAndSetTextColor(float coins, float cost, Text textToColor)
    {
        if(coins >= cost)
        {
            textToColor.color = m_AvailableColor;
        }else
        {
            textToColor.color = m_UnavailableColor;
        }
    }

    public void PurchaseTower(string towerType)
    {
        //If player has enough coins
        if(TowerConfig.s_Towers[towerType][0].BuyCost <= PlayerData.s_Instance.Coins && HexGrid.s_Instance.SelectedTile.CurrentState == TileState.OPEN)
        {
            //Gets the buy cost from the towers data
            PlayerData.s_Instance.ChangeCoinAmount(-TowerConfig.s_Towers[towerType][0].BuyCost);

            //Spawns a tower of the type (parameter) passed
            switch (towerType)
            {
                case TowerTypeTags.BASS_TOWER:
                    SpawnTower(towerType, 0);
                    break;
                case TowerTypeTags.DRUM_TOWER:
                    SpawnTower(towerType, 1);
                    break;
                case TowerTypeTags.LEAD_TOWER:
                    SpawnTower(towerType, 2);
                    break;
            }
            HexGrid.s_Instance.SelectedTile.CurrentState = TileState.OCCUPIED;
            Debug.Log(HexGrid.s_Instance.SelectedTile.CurrentState);
        }
    }

    void SpawnTower(string towerType,int indexInList)
    {
        Tower newTower;
        newTower = Instantiate(m_Towers[indexInList]);
        newTower.TowerData = TowerConfig.s_Towers[towerType][0];
        newTower.transform.position = HexGrid.s_Instance.SelectedTile.transform.position;
        HexGrid.s_Instance.SelectedTile.Tower = newTower;

        int orderInLayer = (HexGrid.s_Instance.GridSize.y - HexGrid.s_Instance.SelectedTile.Y);

        newTower.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
    }
}