using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;
    [SerializeField] private int m_CoinsToSteal;

    private void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        m_CurrentHealth -= damage;
        if(m_CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        //Give coins;
        PlayerData.s_Instance.ChangeCoinAmount(m_CoinsToGive);
        Destroy(this.gameObject); // Will be replaced with Object pooling later
    }

    public void DamageObjective()
    {
        if (PlayerData.s_Instance.Lives > 0)
        {
            PlayerData.s_Instance.ChangeLivesAmount(-1);
            PlayerData.s_Instance.ChangeCoinAmount(-m_CoinsToSteal);
            Destroy(this.gameObject);
        }
        else
        {
            //Gameover
        }

    }

    void MoveToNextWaypoint()
    {

    }
}