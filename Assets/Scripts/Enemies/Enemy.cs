using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class Enemy : MonoBehaviour {

    public delegate void DestroyEvent(Enemy enemy);
    public static DestroyEvent s_OnDestroyEnemy;
    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;
    private bool m_IsAlive = true;

    //TEMP
    private Tween m_dopath;
    private SkeletonAnimation m_SkeletonAnims;
    private AnimationState m_Anim;
    private EnemyHealthbar m_EnemyHealthbar;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;
    [SerializeField] private int m_CoinsToSteal;

    private int m_CurrentNodeIndex;

    private void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
        m_SkeletonAnims = GetComponent<SkeletonAnimation>();
        SongManager.s_OnPlaylistComplete += Death;
        PauseCheck.Pause += TogglePause;

        m_EnemyHealthbar = GetComponent<EnemyHealthbar>();

        m_EnemyHealthbar.SetHealthbarValue(m_MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_HIT, false, transform.position);
        m_CurrentHealth -= damage;
        m_EnemyHealthbar.ChangeEnemyHealthUI(m_CurrentHealth, damage);

        if (m_CurrentHealth <= 0)
        {
            DOTween.Kill(10, true);
            Death(true);
        }
        else if(m_CurrentHealth > 0)
        {
            m_SkeletonAnims.AnimationState.SetAnimation(0, "HIT_Electricity", false);
            m_SkeletonAnims.AnimationState.AddAnimation(0, "MOVE", true,0);

        }
    }

    /// <summary>
    /// This gets added to the s_OnPlayListComplete delegate and won't give the player any coins for enemies that died this way.
    /// </summary>
    public void Death()
    {
        Death(false);
    }

    public void Death(bool killedByPlayer)
    {
        PausePath();
        if(s_OnDestroyEnemy != null)
        {
            s_OnDestroyEnemy(this);
        }
        m_IsAlive = false;
        
        //If player kills the enemy
        if (killedByPlayer)
        {
            //Give coins
            PlayerData.s_Instance.ChangeCoinAmount(m_CoinsToGive);
        }
        m_SkeletonAnims.AnimationState.SetAnimation(0, "DEATH", false);
        m_SkeletonAnims.AnimationState.Complete += delegate
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
        };
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
        if (m_IsAlive)
        {
            Vector3[] pathArray = PathManager.s_Instance.CurrentPathNodes.ToArray();
            m_dopath = transform.DOPath(pathArray, pathArray.Length / m_MoveSpeed, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => DamageObjective());
        }
        //Invoke("PausePath",1);
        //Invoke("PlayPath",2);
        //dopath.Play();
        //m_CurrentNodeIndex = 1;

        //MoveToNextNode();
    }


    //TEMP
    private void PausePath()
    {
        m_dopath.Pause();
    }

    public void TogglePause(bool pause)
    {
        if(pause)
        {
            m_dopath.Pause();
        }
        else
        {
            m_dopath.Play();
        }
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

    private void OnDestroy()
    {
        SongManager.s_OnPlaylistComplete -= Death;
    }

    void DeathRoutine()
    {
        
    }

}