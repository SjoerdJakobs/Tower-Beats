using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassTower : Tower
{
    public enum States
    {
        STARTUP,
        ATTACKING,
        REMOVING,
        IDLE
    }

    [SerializeField]private States m_State = States.IDLE;
    private TowerProjectile m_towerProjectileData;
    private ObjectPool m_pool;

    [SerializeField] private VisualEffect m_VisualEffect;

    public override void Awake()
    {
        base.Awake();
        m_pool = ObjectPoolManager.s_Instance.GetObjectPool(attackProjectile, 20, 5, 5, 20, true);
        GetRMS.s_BassCue += Attack;
        GetRMS.s_OnBassLost += ResetAnimation;
        VisualEffect.s_OnEffectCompleted += EffectCompleted;
    }

    private void Start()
    {
        m_VisualEffect.SetLayer(GetComponent<Renderer>().sortingOrder - 1);
    }

    private void EffectCompleted(VisualEffect effect)
    {
        if(effect == m_VisualEffect)
        {
            switch (m_State)
            {
                case States.STARTUP:
                    m_VisualEffect.Init(EffectType.BassTurretFX_Attack, true);
                    m_State = States.ATTACKING;
                    break;
                case States.REMOVING:
                    m_VisualEffect.Init(EffectType.EMPTY, false);
                    m_State = States.IDLE;
                    break;
            }
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (m_ReadyToAttack && m_Target != null)
        {
            /*m_towerProjectileData = m_pool.GetFromPool().GetComponent<TowerProjectile>();
            m_towerProjectileData.SetNewVars(transform.position, m_Target, TowerData.AttackDamage, 5);*/
            //m_Target.TakeDamage(TowerData.AttackDamage);
            AoEDamage();
            if (m_State != States.ATTACKING && m_State != States.STARTUP)
            {
                m_VisualEffect.Init(EffectType.BassTurretFX_Spawn, false);
                m_State = States.STARTUP;
            }

            m_ReadyToAttack = false;
            m_StartedCooldown = false;
        }
        else if (m_Target == null)
        {
            if(m_State != States.IDLE && m_State == States.ATTACKING || m_State != States.IDLE && m_State == States.STARTUP)
            {
                m_VisualEffect.Init(EffectType.BassTurretFX_Disappear, false);
                m_State = States.REMOVING;
            }
        }
    }

    private void ResetAnimation()
    {
        if(m_State != States.IDLE && m_State != States.REMOVING)
        {
            m_VisualEffect.Init(EffectType.BassTurretFX_Disappear, false);
            m_State = States.REMOVING;
        }
    }


    private void AoEDamage()
    {
        for (int i = 0; i < m_EnemiesInRange.Count; i++)
        {
            m_EnemiesInRange[i].TakeDamage(TowerData.AttackDamage, "Bass");
        }
    }

    private void OnDestroy()
    {
        GetRMS.s_BassCue -= Attack;
        GetRMS.s_OnBassLost -= ResetAnimation;
    }
}