using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPopUp : PopUp {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;
    [SerializeField] private Text m_TowerName;
    [SerializeField] private Text m_TowerLevel;
    [Space]
    [SerializeField] private TowerUtilities m_TowerUtilities;

    private Tile m_CurrentTile;

    public override void Show(Tile calledFrom)
    {
        TowerUtilities.s_OnUpgrade += ShowTowerInfo;
        m_TowerUtilities.CurrentTile = calledFrom;
        m_CurrentTile = calledFrom;
        base.Show(calledFrom);
        ShowTowerInfo();
    }

    public override void Hide()
    {
        TowerUtilities.s_OnUpgrade -= ShowTowerInfo;
        base.Hide();
    }

    /// <summary>
    /// Shows the towers stats/info
    // </summary>
    private void ShowTowerInfo()
    {
        Tower tower = m_CurrentTile.Tower;
        m_DamageField.text = tower.TowerData.AttackDamage.ToString();
        m_SellValue.text = tower.TowerData.SellValue.ToString();
        m_TowerName.text = (tower.TowerData.Type.ToString() + " Turret");
        m_TowerLevel.text = ("Level " + tower.TowerData.Level.ToString());
        if (tower.TowerData.Level < tower.TowerData.MaxLevel)
        {
            m_UpgradeCost.text = tower.TowerData.UpgradeCost.ToString();
        }
        else
        {
            m_UpgradeCost.text = "MAX";
        }
    }
}