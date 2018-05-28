using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : MonoBehaviour {
    
    public Transform m_ShootPos;
    [SerializeField]
    private float m_Damage;
    [SerializeField]
    private Transform m_Target;


    [SerializeField]
    private float m_RaySpeed = 3;
    private float m_Distance;
    private float m_Offset;
    private float m_AnimCooldown = 1.1f;
    private float m_AnimCooldownCounter;

    [SerializeField]
    private AnimProjectile m_AnimProjectile;

    private ObjectPool m_pool;

    private Material m_Mat;

    private bool m_Pause;
    private bool m_ShootAnim;

    private void Awake()
    {
        PauseCheck.Pause += TogglePause;
    }

    // Use this for initialization
    void Start ()
    {
        m_pool = ObjectPoolManager.s_Instance.GetObjectPool(m_AnimProjectile.gameObject, 15, 5, 5, 20, false, PooledSubObject.AnimProjectile);
        m_Mat = GetComponent<Renderer>().material;
	}

    public void SetTarget(Enemy enemyTarget)
    {
        m_Target = enemyTarget.transform;
    }

    public void TogglePause(bool pause)
    {
        m_Pause = pause;
    }

    private void ShootAnim()
    {/*
        if(!m_Pause)
        {
            m_AnimCooldownCounter += Time.deltaTime;
            if(m_AnimCooldownCounter >= m_AnimCooldown)
            {
                AnimProjectile newAnim = m_pool.GetFromPool().GenericObj as AnimProjectile;
                newAnim.SetNewVars(m_ShootPos, BeamEnd, 0.5f);
                m_AnimCooldownCounter = 0;
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target != null && m_Distance < 3)
        {
            if (!m_ShootAnim)
            {
                m_ShootAnim = true;
            }

            m_Offset += Time.deltaTime;

            if (m_Offset >= 1)
            {
                m_Offset = 0;
            }

            transform.localScale = new Vector3(1, m_Distance, 1);
            transform.position = Vector3.Lerp(new Vector3(m_ShootPos.position.x, m_ShootPos.position.y, -1), new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, -1), 0.5f);

            m_Distance = Vector3.Distance(new Vector3(m_ShootPos.position.x, m_ShootPos.position.y, -1), new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, -1));
            m_Mat.mainTextureScale = new Vector2(1, m_Distance);

            if (!m_Pause)
            {
                ShootAnim();
                m_Mat.mainTextureOffset = new Vector2(1, m_Offset * m_RaySpeed);
            }

            Vector3 difference = m_Target.transform.position - m_ShootPos.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, (rotationZ + 90));
        }
        else if (m_Distance > 0)
        {
            m_ShootAnim = false;
            m_Distance = 0;
            transform.localScale = new Vector3(1, 0, 1);
        }
    }
}
