using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour {

    public delegate void DestroyEvent(Enemy enemy);
    public static DestroyEvent s_OnDestroyEnemy;
    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    //TEMP
    private Tween m_dopath;

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
        if (m_CurrentHealth <= 0)
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
        Effects.s_Screenshake(0.2f,20);

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
        Vector3[] pathArray = PathManager.s_Instance.CurrentPathNodes.ToArray();
        m_dopath = transform.DOPath(pathArray, pathArray.Length * m_MoveSpeed).SetEase(Ease.Linear).OnComplete(() => DamageObjective());
        Invoke("PausePath",1);
        Invoke("PlayPath",2);
        //dopath.Play();
        //m_CurrentNodeIndex = 1;

        //MoveToNextNode();
    }


    //TEMP
    private void PausePath()
    {
        m_dopath.Pause();
    }

    //TEMP
    private void PlayPath()
    {
        m_dopath.Play();
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