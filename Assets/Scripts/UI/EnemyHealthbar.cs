using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class EnemyHealthbar : MonoBehaviour {

    [SerializeField] private Image m_EnemyHealthbar;

    private int m_CurrentHealthbar = 10;

    public void ChangeEnemyHealthUI(float newHealthValue)
    {
        if(m_CurrentHealthbar > 0)
        {
            m_EnemyHealthbar.fillAmount = newHealthValue;
        }
    }
}