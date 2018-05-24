using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumTower : Tower
{
    private TowerProjectile m_towerProjectileData;
    private ObjectPool m_pool;
    public override void Awake()
    {
        base.Awake();
        m_pool = ObjectPoolManager.s_Instance.GetObjectPool(attackProjectile, 20, 5, 5, 20, true);
        GetRMS.s_DrumCue += Attack;
    }

    public override void Attack()
    {
        base.Attack();
        if (m_ReadyToAttack && m_Target != null)
        {

            m_towerProjectileData = m_pool.GetFromPool().GetComponent<TowerProjectile>();
            m_towerProjectileData.SetNewVars(transform.position, m_Target, TowerData.AttackDamage, 5);
            //m_Target.TakeDamage(TowerData.AttackDamage);
            m_ReadyToAttack = false;
            m_StartedCooldown = false;
        }
    }

    private void OnDestroy()
    {
        GetRMS.s_DrumCue -= Attack;
    }
}