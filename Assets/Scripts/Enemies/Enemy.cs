using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class Enemy : PoolObj
{

    public delegate void DestroyEvent(Enemy enemy);
    public static DestroyEvent s_OnDestroyEnemy;
    [SerializeField]
    private float m_MaxHealth = 20;
    public float CurrentHealth { get; set; }
    public bool IsAlive { get; set; }

    public SkeletonAnimation SkeletonAnims { get; set; }
    //private Tween m_dopath;
    private AnimationState m_Anim;
    private EnemyHealthbar m_EnemyHealthbar;
    private MeshRenderer m_Renderer;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;

    private int m_CurrentNodeIndex;

    private void Awake()
    {
        CurrentHealth = m_MaxHealth;
        SkeletonAnims = GetComponent<SkeletonAnimation>();
        m_Renderer = GetComponent<MeshRenderer>();
        GameManager.s_OnGameStop += Death;

        PauseCheck.Pause += TogglePause;

        m_EnemyHealthbar = GetComponent<EnemyHealthbar>();
    }

    public void RestoreHealth()
    {
        CurrentHealth = m_MaxHealth;
        m_EnemyHealthbar.ChangeEnemyHealthUI(CurrentHealth / m_MaxHealth);
    }

    public void TakeDamage(float damage, string towerType)
    {
        if (IsAlive)
        {
            CurrentHealth -= damage;
            m_EnemyHealthbar.ChangeEnemyHealthUI(CurrentHealth / m_MaxHealth);

            if (CurrentHealth <= 0)
            {
                Death(true);
            }
            else if (CurrentHealth > 0)
            {
                switch (towerType)
                {
                    case "Bass":
                        SkeletonAnims.AnimationState.SetAnimation(0, "HIT_Electricity", false);
                        SkeletonAnims.AnimationState.AddAnimation(0, "MOVE", true, 0);
                        break;
                    case "Drum":
                        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_HIT, false, new Vector2(transform.position.x, transform.position.y + 0.5f));
                        break;
                    case "Lead":
                        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_HIT, false, new Vector2(transform.position.x, transform.position.y + 0.5f));
                        break;
                }
            }
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
        DOTween.Kill(this);
        if (s_OnDestroyEnemy != null)
        {
            s_OnDestroyEnemy(this);
        }
        IsAlive = false;

        //If player kills the enemy
        if (killedByPlayer)
        {
            //Give coins
            PlayerData.s_Instance.ChangeCoinAmount(m_CoinsToGive);
        }
        SkeletonAnims.AnimationState.SetAnimation(0, "DEATH", false);
        SkeletonAnims.AnimationState.Complete += delegate
        {
            if (SkeletonAnims.AnimationName == "DEATH")
            {
                ReturnToPool();
            }
        };
    }

    public void DamageObjective()
    {
        Effects.s_Screenshake(0.2f, 20);

        if (PlayerData.s_Instance.Lives > 0)
        {
            PlayerData.s_Instance.ChangeLivesAmount(-1);

            DOTween.Kill(this);

            ReturnToPool();
        }
    }

    public void Move(Vector3 startPos)
    {
        if (IsAlive)
        {
            DOTween.Kill(this);
            transform.position = startPos;
            Vector3[] pathArray = MapLoader.s_Instance.GetWaypointsFromPath();
            transform.DOPath(pathArray, pathArray.Length / m_MoveSpeed, PathType.CatmullRom).SetEase(Ease.Linear).SetId(this).OnComplete(() => DamageObjective()).OnWaypointChange(UpdateEnemyLayering);
        }
    }

    private void UpdateEnemyLayering(int waypointIndex)
    {
        m_Renderer.sortingOrder = HexGrid.s_Instance.GridSize.y - MapLoader.s_Instance.Path[waypointIndex].PositionInGrid.y;
    }


    public void TogglePause(bool pause)
    {
        if (pause)
        {
            DOTween.Pause(this);
        }
        else
        {
            DOTween.Play(this);
        }
    }

    private void OnDestroy()
    {
        GameManager.s_OnGameStop -= Death;
    }

    void DeathRoutine()
    {

    }
}