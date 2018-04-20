using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : Menu {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_RangeField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;

    public override void Show()
    {
        base.Show();
        TowerUtilities.s_OnUpgrade += ShowTowerInfo;
        ShowTowerInfo();
    }

    /// <summary>
    /// Shows the towers stats/info
    // </summary>
    private void ShowTowerInfo()
    {
        Tower tower = HexGrid.s_Instance.SelectedTile.Tower;
        m_DamageField.text = tower.TowerData.AttackDamage.ToString();
        m_RangeField.text = tower.TowerData.AttackRange.ToString();
        m_SellValue.text = tower.TowerData.SellValue.ToString();
        m_UpgradeCost.text = tower.TowerData.UpgradeCost.ToString();
    }

    public override void Hide()
    {
        TowerUtilities.s_OnUpgrade -= ShowTowerInfo;
        base.Hide();
    }
}