using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassTower : Tower
{
    private TowerProjectile m_towerProjectileData;
    private ObjectPool m_pool;

    [SerializeField] private VisualEffect m_VisualEffect;

    public override void Awake()
    {
        base.Awake();
        m_pool = ObjectPoolManager.s_Instance.GetObjectPool(attackProjectile, 20, 5, 5, 20, true);
        GetRMS.s_BassCue += Attack;
    }

    public override void Attack()
    {
        base.Attack();

        if (m_ReadyToAttack && m_Target != null)
        {
            m_towerProjectileData = m_pool.GetFromPool().GetComponent<TowerProjectile>();
            m_towerProjectileData.SetNewVars(transform.position, m_Target, TowerData.AttackDamage, 5);
            //m_Target.TakeDamage(TowerData.AttackDamage);
            //m_VisualEffect.Init(EffectType.ELECTRO_TURRET_ATTACK, true);
            m_ReadyToAttack = false;
            m_StartedCooldown = false;
        }
        else
        {
            //m_VisualEffect.Stop();
        }
    }

    private void OnDestroy()
    {
        GetRMS.s_BassCue -= Attack;
    }
}