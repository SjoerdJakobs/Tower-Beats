using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPopUp : PopUp {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;
    [SerializeField] private Text m_TowerName;
    [SerializeField] private Text m_TowerLevel;
    [SerializeField] private Text m_TargetType;
    [Space]
    [SerializeField] private TowerUtilities m_TowerUtilities;
    [SerializeField] private TowerUpgradesAnimation m_Animation;
    [Space]
    [SerializeField] private Color m_NotEnoughCoinsColor;
    [SerializeField] private Color m_NormalColor;

    private Tile m_CurrentTile;

    public override void Show(Tile calledFrom)
    {
        TowerUtilities.s_OnUpgrade += UpdateTowerInfo;
        PlayerData.s_OnUpdateCoins += OnPlayerCoinsUpdated;
        m_TowerUtilities.CurrentTile = calledFrom;
        m_CurrentTile = calledFrom;
        m_CurrentTile.SetTileVisualsState(TileVisualState.TURRET_SELECTED);
        base.Show(calledFrom);
        m_Animation.AnimateIn();
        UpdateTowerInfo();
    }

    public override void Hide()
    {
        TowerUtilities.s_OnUpgrade -= UpdateTowerInfo;
        m_TowerUtilities.CurrentTile = null;

        if (LastClickedFromTile != null)
            LastClickedFromTile.SetTileVisualsState(TileVisualState.TURRET_SPAWN);
        ClearLastClickedTile();
        m_Animation.AnimateOut(delegate {
            base.Hide();
        });

    }

    private void OnDisable()
    {
        TowerUtilities.s_OnUpgrade -= UpdateTowerInfo;
    }

    private void OnDestroy()
    {
        TowerUtilities.s_OnUpgrade -= UpdateTowerInfo;
    }

    private void OnPlayerCoinsUpdated(float value)
    {
        if(m_CurrentTile.Tower != null)
            UpdateTowerInfo();
    }

    /// <summary>
    /// Shows the towers stats/info
    // </summary>
    private void UpdateTowerInfo()
    {
        Tower tower = m_CurrentTile.Tower;

        m_DamageField.text = tower.TowerData.AttackDamage.ToString();
        m_SellValue.text = tower.TowerData.SellValue.ToString();
        m_TowerName.text = (tower.TowerData.Type.ToString() + " Turret");
        m_TowerLevel.text = ("Level " + tower.TowerData.Level.ToString() + (tower.TowerData.Level >= 3 ? " (MAX)" : ""));
        m_TargetType.text = tower.TargetType.ToString();

        if (m_UpgradeCost == null) return;

        if (tower.TowerData.Level < tower.TowerData.MaxLevel)
        {
            if (PlayerData.s_Instance.Coins >= tower.TowerData.UpgradeCost)
                m_UpgradeCost.color = m_NormalColor;
            else
                m_UpgradeCost.color = m_NotEnoughCoinsColor;

            m_UpgradeCost.text = tower.TowerData.UpgradeCost.ToString();
        }
        else
        {
            m_UpgradeCost.color = m_NormalColor;
            m_UpgradeCost.text = "MAX";
        }
    }
}