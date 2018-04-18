using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthbar : MonoBehaviour {


    [SerializeField] private Image m_EnemyHealthbar;

    void ChangeEnemyHealthUI(float currentEnemyHealth)
    {
        m_EnemyHealthbar.DOFillAmount(currentEnemyHealth, 0.2f);
    }
}