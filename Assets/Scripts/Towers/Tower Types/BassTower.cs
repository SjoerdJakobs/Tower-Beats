using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassTower : Tower
{
    public override void Attack()
    {
        base.Attack();
        if (m_ReadyToAttack)
        {
            Debug.Log("TARGETED " + m_Target);
            m_Target.TakeDamage(TowerData.AttackDamage);
            m_ReadyToAttack = false;
            m_StartedCooldown = false;
        }

    }
}