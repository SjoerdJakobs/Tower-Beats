using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadTower : Tower
{
    private GameObject m_Laser;
    private TowerLaser m_LaserData;
    private ObjectPool m_pool;

    public Transform laserOrgin;

    public override void Awake()
    {
        base.Awake();
        if(m_Laser == null)
        {
            m_Laser = Instantiate(attackProjectile);
        }
        m_LaserData = m_Laser.GetComponent<TowerLaser>();
        m_Laser.transform.position = transform.position + new Vector3(0.5f, 1, 0);
        m_LaserData.ShootPos = laserOrgin;
        //m_pool = ObjectPoolManager.s_Instance.GetObjectPool(attackProjectile, 20, 5, 5, 20, true,PooledSubObject.TowerProjectile);
        GetRMS.s_LeadCue += Attack;
    }

    public override void Attack()
    {
        base.Attack();

        if (m_ReadyToAttack && m_Target != null)
        {
            m_LaserData.SetTarget(m_Target);
            Debug.Log("Damage");
            //m_towerProjectileData.SetNewVars(transform.position, m_Target, TowerData.AttackDamage, 5);
            m_Target.TakeDamage(TowerData.AttackDamage, "Lead");
            m_ReadyToAttack = false;
            m_StartedCooldown = false;
        }
    }

    private void OnDestroy()
    {
        GetRMS.s_LeadCue -= Attack;
    }
}