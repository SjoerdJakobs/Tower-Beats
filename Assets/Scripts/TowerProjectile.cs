using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerProjectile : PoolObj {

    private Vector3 m_startPos;
    private float m_damage;
    private float m_movSpeed;
    private Enemy m_target;
    private bool m_pause;

    private void Awake()
    {
        PauseCheck.Pause += TogglePause;
    }

    public void SetNewVars(Vector3 newPos, Enemy newTarget, float newDamage, float newSpeed)
    {
        m_startPos = newPos;
        transform.position = newPos;
        m_target = newTarget;
        m_damage = newDamage;
        m_movSpeed = newSpeed;
        StartCoroutine(MoveToTarget());

    }

    private IEnumerator MoveToTarget()
    {
        float lerpValue = 0;
        while (lerpValue <= 1)
        {
            if(!m_pause)
            {
                Vector3.Lerp(m_startPos, m_target.transform.position, lerpValue*m_movSpeed);
                lerpValue += Time.deltaTime;
                transform.LookAt(m_target.transform.position);
            }
            yield return new WaitForEndOfFrame();
        }
        if(m_target != null)
        {
            m_target.TakeDamage(m_damage);
        }
        //impact
        //yield return new WaitForSeconds(impact anim time?);
        ReturnToPool();
    }

    public void TogglePause(bool pause)
    {
        m_pause = pause;
    }
}
