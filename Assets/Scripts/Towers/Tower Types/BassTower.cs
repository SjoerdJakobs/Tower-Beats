using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassTower : Tower
{
    public override void Awake()
    {
        base.Awake();
        GetRMS.s_BassCue += Attack;
    }

    public override void Attack()
    {
        base.Attack();

        if (m_ReadyToAttack && m_Target != null)
        {
            m_Target.TakeDamage(TowerData.AttackDamage);
            m_ReadyToAttack = false;
            m_StartedCooldown = false;
        }
    }

    private void OnDestroy()
    {
        GetRMS.s_BassCue -= Attack;
    }
}