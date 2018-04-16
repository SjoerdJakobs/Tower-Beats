using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : Menu {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_RangeField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;

    public static TowerMenu s_Instance;

    private Tower m_Tower;
    public Tower Tower
    {
        get { return m_Tower; }
        set { m_Tower = value; }
    }

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
    /*public void ShowTowerMenu()
    {
        m_DamageField.text = m_Tower.TowerData.AttackDamage.ToString();
        m_RangeField.text = m_Tower.TowerData.AttackRange.ToString();
        m_SellValue.text = m_Tower.TowerData.SellValue.ToString();
        m_UpgradeCost.text = m_Tower.TowerData.UpgradeCost.ToString();
    }*/

    public void ShowTowerMenu()
    {
        m_DamageField.text = PlayerData.s_Instance.SelectedTower.TowerData.AttackDamage.ToString();
        m_RangeField.text = m_Tower.TowerData.AttackRange.ToString();
        m_SellValue.text = m_Tower.TowerData.SellValue.ToString();
        m_UpgradeCost.text = m_Tower.TowerData.UpgradeCost.ToString();
    }

    /// <summary>
    /// Sell a tower, its sell value is added to the players coins
    /// </summary>
    public void SellTower()
    {
        PlayerData.s_Instance.ChangeCoinAmount(m_Tower.TowerData.SellValue);
        Destroy(m_Tower.gameObject);
        Hide();
    }

    /// <summary>
    /// Upgrade a tower to the next level
    /// </summary>
    public void Upgrade()
    {
        if (m_Tower.TowerData.Level < m_Tower.TowerData.MaxLevel && PlayerData.s_Instance.Coins >= m_Tower.TowerData.UpgradeCost)
        {
            PlayerData.s_Instance.ChangeCoinAmount(-m_Tower.TowerData.UpgradeCost);
            m_Tower.TowerData = TowerConfig.s_Towers[m_Tower.TowerData.Type][m_Tower.TowerData.Level];
            Debug.Log("Level" + m_Tower.TowerData.Level);
            ShowTowerMenu();
        }
    }
}