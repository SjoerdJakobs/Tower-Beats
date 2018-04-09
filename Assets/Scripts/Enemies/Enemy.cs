using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;
    [SerializeField] private int m_CoinsToSteal;

    void Death()
    {
        if(m_CurrentHealth <= 0)
        {
            //Give coins;
            PlayerData.s_Instance.Coins += m_CoinsToGive;
        }
    }

    void DamageObjective()
    {
        if (PlayerData.s_Instance.Lives > 0)
        {
            PlayerData.s_Instance.Lives--;
            PlayerData.s_Instance.Coins -= m_CoinsToSteal;
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