using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimProjectile : PoolObj
{
    private Vector3 m_StartPos;
    private Transform m_EndPos;
    private float m_MovSpeed;
    private Vector3 m_Offset = new Vector3(0, 0.5f, 0);

    private bool m_pause;

    private void Awake()
    {
        PauseCheck.Pause += TogglePause;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetNewVars(Vector3 newPos, Transform endPos, float newSpeed)
    {
        m_EndPos = endPos;
        m_StartPos = newPos;
        transform.position = newPos;
        m_MovSpeed = newSpeed;
        StartCoroutine(MoveToTarget());
    }

    /// <summary>
    /// Moves the projectile to a target
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToTarget()
    {
        Vector3 lastPos = Vector3.zero;
        float lerpValue = 0;
        while (lerpValue * m_MovSpeed <= 1)
        {
            if (!m_pause)
            {
                if (m_EndPos != null)
                {
                    transform.position = Vector3.Lerp(m_StartPos, (m_EndPos.position+ m_Offset), lerpValue * m_MovSpeed);
                    lastPos = m_EndPos.position;
                    lerpValue += Time.deltaTime;

                    Vector3 difference = m_EndPos.position - transform.position;
                    float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, (rotationZ));
                }
                else
                {
                    transform.position = Vector3.Lerp(m_StartPos, lastPos, lerpValue * m_MovSpeed);
                    lerpValue += Time.deltaTime;
                }
            }
            yield return new WaitForEndOfFrame();
        }
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
