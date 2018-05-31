using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : MonoBehaviour {

    [SerializeField]
    private GameObject m_HitObjectPrefab;
    private GameObject m_HitObject;
    [SerializeField]
    private Transform m_Target;
    [SerializeField]
    public Transform ShootPos;
    private Vector3 m_Difference;

    [SerializeField]
    private float m_RaySpeed = 3;
    private float m_IntervalValue;
    private float m_Distance;
    private float m_Offset;
    private float m_AnimCooldown = 1.1f;
    private float m_AnimCooldownCounter;
    private float m_RotationZ;
    private float m_XScaleOrginValue;
    private float m_XScaleModTimer;
    private float m_XScaleModValue;

    private Material m_Mat;

    private bool m_Pause;

    private void Awake()
    {
        PauseCheck.Pause += TogglePause;
    }

    // Use this for initialization
    void Start ()
    {
        m_HitObject = Instantiate(m_HitObjectPrefab);
        m_HitObject.SetActive(false);
        m_XScaleOrginValue = 0.4f;
        m_Mat = GetComponent<Renderer>().material;
	}

    public void SetTarget(Enemy enemyTarget, float interval)
    {
        m_Target = enemyTarget.transform;
        m_IntervalValue = interval;
        //print(interval);
    }

    public void TogglePause(bool pause)
    {
        m_Pause = pause;
    }


    void Update()
    {
        if (m_Target != null && m_IntervalValue > -0.2f)
        {
            m_IntervalValue -= Time.deltaTime;

            m_Offset += Time.deltaTime;

            if (m_Offset >= 1)
            {
                m_Offset = 0;
            }

            m_XScaleModTimer += Time.deltaTime;
            if (m_XScaleModTimer >= 0.06f)
            {
                m_XScaleModValue = m_XScaleOrginValue + Random.Range(-0.08f, 0.08f);
                m_XScaleModTimer = 0;
            }

            transform.localScale = new Vector3(m_XScaleModValue, m_Distance, 1);
            transform.position = Vector3.Lerp(new Vector3(ShootPos.position.x, ShootPos.position.y, -1), new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, -1), 0.5f);

            m_Distance = Vector3.Distance(new Vector3(ShootPos.position.x, ShootPos.position.y, -1), new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, -1));
            m_Mat.mainTextureScale = new Vector2(1, m_Distance);

            if (!m_Pause)
            {
                m_Mat.mainTextureOffset = new Vector2(1, m_Offset * m_RaySpeed);
            }

            m_Difference = m_Target.transform.position - ShootPos.position;
            m_RotationZ = Mathf.Atan2(m_Difference.y, m_Difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, (m_RotationZ + 90));

            if(!m_HitObject.activeInHierarchy)
            {
                m_HitObject.SetActive(true);
            }
            m_HitObject.transform.position = m_Target.position;
        }
        else if (m_Distance > 0)
        {
            if(m_HitObject.activeInHierarchy)
            {
                m_HitObject.SetActive(false);
            }
            m_Distance = 0;
            transform.localScale = new Vector3(1, 0, 1);
        }
    }
}
