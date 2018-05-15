using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerProjectile : PoolObj {

    private Vector3 m_startPos;
    private float m_damage;
    private float m_movSpeed;
    private Enemy m_target;
    private GameObject m_targetObj;
    private bool m_pause;

    private void Awake()
    {
        PauseCheck.Pause += TogglePause;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetNewVars(Vector3 newPos, Enemy newTarget, float newDamage, float newSpeed)
    {
        m_startPos = newPos;
        transform.position = newPos;
        m_target = newTarget;
        m_damage = newDamage;
        m_movSpeed = newSpeed;
        m_targetObj = newTarget.gameObject;
        StartCoroutine(MoveToTarget());

    }

    private IEnumerator MoveToTarget()
    {
        Vector3 lastPos = Vector3.zero;
        float lerpValue = 0;
        while (lerpValue <= 1)
        {
            if (!m_pause)
            {
                if (m_targetObj != null)
                {
                    transform.position = Vector3.Lerp(m_startPos, m_targetObj.transform.position, lerpValue);
                    lastPos = m_targetObj.transform.position;
                    lerpValue += Time.deltaTime;
                    transform.LookAt(m_target.transform.position);
                }
                else
                {
                    transform.position = Vector3.Lerp(m_startPos, lastPos, lerpValue * m_movSpeed);
                    lerpValue += Time.deltaTime;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        if(m_targetObj != null)
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ye");
        ReturnToPool();
    }
}
