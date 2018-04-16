using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : Menu {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_RangeField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;

    public static TowerMenu s_Instance;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
    }
    /// <summary>
    /// Shows the towers stats/info
    // </summary>
    public void ShowTowerMenu()
    {
        Tower tower = PlayerData.s_Instance.SelectedTower;
        m_DamageField.text = tower.TowerData.AttackDamage.ToString();
        m_RangeField.text = PlayerData.s_Instance.SelectedTower.TowerData.AttackRange.ToString();
        m_SellValue.text = tower.TowerData.SellValue.ToString();
        m_UpgradeCost.text = tower.TowerData.UpgradeCost.ToString();
    }
}