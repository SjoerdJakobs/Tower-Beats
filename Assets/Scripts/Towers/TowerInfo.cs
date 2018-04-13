using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_RangeField;
    [SerializeField] private Text m_SellValue;
    [SerializeField] private Text m_UpgradeCost;

    /// <summary>
    /// Shows the towers stats/info
    /// </summary>
    void ShowTowerInfo(TowerData tower)
    {
        m_DamageField.text = tower.AttackDamage.ToString();
        m_RangeField.text = tower.AttackRange.ToString();
        m_SellValue.text = (tower.Value * 0.75f).ToString();
        m_UpgradeCost.text = tower.UpgradeCost.ToString();
    }
}