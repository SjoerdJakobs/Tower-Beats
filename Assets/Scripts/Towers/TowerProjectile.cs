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
    private Vector3 m_Offset = new Vector3(0, 0.5f, 0);

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
        while (lerpValue * m_movSpeed <= 1)
        {
            if (!m_pause)
            {
                if (m_targetObj != null)
                {
                    transform.position = Vector3.Lerp(m_startPos, (m_targetObj.transform.position + m_Offset), lerpValue * m_movSpeed);
                    lastPos = m_targetObj.transform.position;
                    lerpValue += Time.deltaTime;

                    Vector3 difference = m_target.transform.position - transform.position;
                    float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, (rotationZ + 90));
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
        ReturnToPool();
    }
}
