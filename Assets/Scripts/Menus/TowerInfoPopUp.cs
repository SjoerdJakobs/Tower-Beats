using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPopUp : PopUp {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_RangeField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;

    private void OnEnable()
    {
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

    public void OnDisable()
    {
        TowerUtilities.s_OnUpgrade -= ShowTowerInfo;
    }
}