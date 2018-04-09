using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;

    void Death()
    {
        if(m_CurrentHealth <= 0)
        {
            //Give coins;
            PlayerData.s_Instance.Coins += m_CoinsToGive;
        }
    }
}