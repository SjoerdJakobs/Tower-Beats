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

    private int BarsToAnimate(float damage)
    {
        float decimalValue = damage / m_HealthPerBar;
        float Rounded = Mathf.RoundToInt(damage / m_HealthPerBar);

        m_Modulo = decimalValue - Rounded;

        return (int)Rounded;
    }

    void AnimateHealthbar(float damage)
    {
        float animSpeed = 0.15f;

        for (int i = 0; i < BarsToAnimate(damage); i++)
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
    }

    void AnimateBarCallback()
    {
        m_EnemyHealthbar[m_CurrentHealthbar].gameObject.SetActive(false);
        m_EnemyHealthbar.RemoveAt(m_CurrentHealthbar);
        m_IsAnimating = false;
    }
}