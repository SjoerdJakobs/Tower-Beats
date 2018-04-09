using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour {

    [SerializeField] private Text m_DamageField;
    [SerializeField] private Text m_RangeField;
    [SerializeField] private Text m_SellValue;

    /// <summary>
    /// Shows the towers stats/info
    /// </summary>
    void ShowTowerInfo(TowerData tower)
    {
        m_DamageField.text = tower.AttackDamage.ToString();
        m_RangeField.text = tower.AttackRange.ToString();
        m_SellValue.text = (tower.Costs * 0.75f).ToString();
    }
}