using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class EnemyHealthbar : MonoBehaviour {

    [SerializeField] private List<Image> m_EnemyHealthbar = new List<Image>();

    private float m_HealthPerBar;

    private int m_CurrentHealthbar = 10;

    private bool m_IsAnimating;

    private float m_Modulo;
    private int m_BarsToAnimate = 0;

    private Sequence m_HealthbarSqn;

    private void Awake()
    {
        m_HealthbarSqn.SetId(10);
    }

    public void SetHealthbarValue(float value)
    {
        m_HealthPerBar = value / 10;
    }

    public void ChangeEnemyHealthUI(float newHealthValue, float damageTaken)
    {
        if(m_CurrentHealthbar > 0)
            AnimateHealthbar(damageTaken);
    }

    private void CalculateBarsToAnimate(float damage)
    {
        int Rounded = 0;

        if(m_Modulo >= 1)
        {
            m_Modulo -= 1;
            Rounded = 1;
        }

        float decimalValue = damage / m_HealthPerBar;
        Rounded += Mathf.FloorToInt(decimalValue);

        float numberToAdd = (decimalValue - Rounded);


        m_Modulo += CalculateAddedValue(numberToAdd);

        m_BarsToAnimate = Rounded;
    }

    private float CalculateAddedValue(float value)
    {
        if(value < 0){return Mathf.Abs(value);}
        else {return value;}
    }

    void AnimateHealthbar(float damage)
    {
        CalculateBarsToAnimate(damage);
        float animSpeed = 0.15f;

        for (int i = 0; i < m_BarsToAnimate; i++)
        {
            if (m_CurrentHealthbar > 0)
                m_CurrentHealthbar -= 1;

            if(!m_IsAnimating)
            {
                m_HealthbarSqn = DOTween.Sequence();

                m_IsAnimating = true;

                m_HealthbarSqn.Append(m_EnemyHealthbar[m_CurrentHealthbar].rectTransform.DOAnchorPosY(1.5f, animSpeed));
                m_HealthbarSqn.Join(m_EnemyHealthbar[m_CurrentHealthbar].DOFade(0f, animSpeed));
                m_HealthbarSqn.OnComplete(AnimateBarCallback);
            }
            else if(m_IsAnimating)
            {
                m_HealthbarSqn = DOTween.Sequence();

                m_IsAnimating = true;

                m_HealthbarSqn.AppendInterval(0.075f);

                m_HealthbarSqn.Append(m_EnemyHealthbar[m_CurrentHealthbar].rectTransform.DOAnchorPosY(1.5f, animSpeed));
                m_HealthbarSqn.Join(m_EnemyHealthbar[m_CurrentHealthbar].DOFade(0f, animSpeed));
                m_HealthbarSqn.OnComplete(AnimateBarCallback);
            }
        }
        m_BarsToAnimate = 0;
    }

    void AnimateBarCallback()
    {
        m_EnemyHealthbar[m_CurrentHealthbar].gameObject.SetActive(false);
        m_EnemyHealthbar.RemoveAt(m_CurrentHealthbar);
        m_IsAnimating = false;
    }
}