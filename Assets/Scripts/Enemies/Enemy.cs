using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour {

    public delegate void DestroyEvent(Enemy enemy);
    public static DestroyEvent s_OnDestroyEnemy;
    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;
    [SerializeField] private int m_CoinsToSteal;

    private int m_CurrentNodeIndex;

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
        if (s_OnDestroyEnemy != null)
        {
            s_OnDestroyEnemy(this);
        }
        Destroy(this.gameObject); // Will be replaced with Object pooling later
    }

    public void DamageObjective()
    {
        //If there is a instance of the CameraMovement script in the scene, shake the screen
        if (CameraMovement.s_Instance != null)
        {
            CameraMovement.s_Instance.Screenshake();
        }
        //If there is a instance of the ShowHitIndicator script in the scene, show the hit indicator
        if (ShowHitIndicator.s_Instance != null)
        {
            ShowHitIndicator.s_Instance.HitIndicator();
        }
        if (PlayerData.s_Instance.Lives > 0)
        {
            PlayerData.s_Instance.ChangeLivesAmount(-1);
            PlayerData.s_Instance.ChangeCoinAmount(-m_CoinsToSteal);
            
            Destroy(this.gameObject);
        }
    }


    //TEMP
    public void Move()
    {
        m_CurrentNodeIndex = 1;
        MoveToNextNode();
    }

    //TEMP
    private void MoveToNextNode()
    {
        if (m_CurrentNodeIndex < PathManager.s_Instance.CurrentPathNodes.Count)
        {
            transform.DOMove(PathManager.s_Instance.CurrentPathNodes[m_CurrentNodeIndex], m_MoveSpeed).SetEase(Ease.Linear).OnComplete(() => MoveToNextNode());
            m_CurrentNodeIndex++;
        }
        else
        {
            DamageObjective();
        }
    }
}