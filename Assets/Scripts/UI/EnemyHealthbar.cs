using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthbar : MonoBehaviour {

    [SerializeField] private Image m_EnemyHealthbar;

    public void ChangeEnemyHealthUI(float healthbarValue)
    {
        m_EnemyHealthbar.DOFillAmount(healthbarValue, 0.2f);
    }
}