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

    private Tile m_CurrentTile;

    public override void Show(Tile calledFrom)
    {
        CompareCostAndSetText(TowerConfig.s_Towers[TowerTypeTags.BASS_TOWER][0].BuyCost, m_BassTowerCost);
        CompareCostAndSetText(TowerConfig.s_Towers[TowerTypeTags.DRUM_TOWER][0].BuyCost, m_DrumTowerCost);
        CompareCostAndSetText(TowerConfig.s_Towers[TowerTypeTags.LEAD_TOWER][0].BuyCost, m_LeadTowerCost);

        m_CurrentTile = calledFrom;
        base.Show(calledFrom);
    }

    public override void Hide()
    {
        base.Hide();
    }

    void CompareCostAndSetText(float cost, Text textToColor)
    {
        float coins = PlayerData.s_Instance.Coins;
        textToColor.text = cost.ToString();
        if (coins >= cost)
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
        if(TowerConfig.s_Towers[towerType][0].BuyCost <= PlayerData.s_Instance.Coins && m_CurrentTile.CurrentState == TileState.TURRET_SPAWN)
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
            m_CurrentTile.CurrentState = TileState.OCCUPIED;
        }
    }

    void SpawnTower(string towerType,int indexInList)
    {
        EffectsManager.s_Instance.SpawnEffect(EffectType.TURRET_SPAWN, false, m_CurrentTile.transform.position);
        Tower newTower;
        newTower = Instantiate(m_Towers[indexInList]);
        newTower.TowerData = TowerConfig.s_Towers[towerType][0];
        newTower.transform.position = m_CurrentTile.transform.position;
        m_CurrentTile.Tower = newTower;

        PopUpManager.s_Instance.ShowPopUp(PopUpNames.TOWER_MENU, m_CurrentTile);

        int orderInLayer = (HexGrid.s_Instance.GridSize.y - m_CurrentTile.Y);

        newTower.GetComponent<Renderer>().sortingOrder = orderInLayer;
    }
}