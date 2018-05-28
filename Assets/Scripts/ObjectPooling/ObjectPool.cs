using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject ObjectPrefab { get; set; }
    //private Queue<GameObject> m_DeadList;
    private Queue<PoolObj> m_DeadList;
    private PooledSubObject m_objEnum;
    private Object m_GenericObj;

    private int m_ObjectsInPool;
    private int m_ObjectsAliveInPool;

    private int m_CleanThreshold;
    private int m_IncreaseIncrement;
    private int m_anagerTicksBeforeClean;
    private int m_CurrentTick;

    private bool m_NotAGameObject;

    private bool m_isAboveThreshold;

    public void Init(PooledSubObject GenericObj, int PoolStartSize = 20, int IncreaseIncrement = 5, int ManagerTicksBeforeClean = 5, int CleanThreshold = 20)
    {
        m_objEnum = GenericObj;
        PoolObj TempData = ObjectPrefab.GetComponent<PoolObj>();
        TempData.Pool = this;
        this.m_CleanThreshold = CleanThreshold;
        this.m_IncreaseIncrement = IncreaseIncrement;
        this.m_anagerTicksBeforeClean = ManagerTicksBeforeClean;

        //temp
        m_ObjectsAliveInPool = 0;
        //m_DeadList = new Queue<GameObject>();
        m_DeadList = new Queue<PoolObj>();

        for (int i = 0; i < PoolStartSize; i++)
        {
            GameObject NewPoolGameObj = Instantiate(ObjectPrefab, transform);
            PoolObj poolObj = NewPoolGameObj.GetComponent<PoolObj>();
            switch (GenericObj)
            {
                case PooledSubObject.Default:
                    poolObj.GenericObj = null;
                    break;
                case PooledSubObject.GameObject:
                    poolObj.GenericObj = poolObj.gameObject;
                    break;
                case PooledSubObject.VisualEffect:
                    poolObj.GenericObj = poolObj.gameObject.GetComponent<VisualEffect>();
                    break;
                case PooledSubObject.Enemy:
                    poolObj.GenericObj = poolObj.gameObject.GetComponent<Enemy>();
                    break;
                case PooledSubObject.Rigidbody:
                    poolObj.GenericObj = poolObj.gameObject.GetComponent<Rigidbody>();
                    break;
                case PooledSubObject.TowerProjectile:
                    poolObj.GenericObj = poolObj.gameObject.GetComponent<TowerProjectile>();
                    break;
                case PooledSubObject.AnimProjectile:
                    poolObj.GenericObj = poolObj.gameObject.GetComponent<AnimProjectile>();
                    break;
                default:
                    m_GenericObj = null;
                    break;
            }
            m_DeadList.Enqueue(poolObj);

            m_ObjectsInPool++;
            NewPoolGameObj.SetActive(false);
        }
    }

    public PoolObj GetFromPool() 
    {
        if (m_ObjectsAliveInPool < m_ObjectsInPool)
        {
            m_ObjectsAliveInPool++;
            PoolObj returnObject = m_DeadList.Dequeue();
            returnObject.gameObject.SetActive(true);
            return (returnObject);
        }
        else
        {
            for (int i = 0; i < m_IncreaseIncrement; i++)
            {
                GameObject NewPoolGameObj = Instantiate(ObjectPrefab, transform);
                PoolObj poolObj = NewPoolGameObj.GetComponent<PoolObj>();
                switch (m_objEnum)
                {
                    case PooledSubObject.Default:
                        poolObj.GenericObj = null;
                        break;
                    case PooledSubObject.GameObject:
                        poolObj.GenericObj = poolObj.gameObject;
                        break;
                    case PooledSubObject.VisualEffect:
                        poolObj.GenericObj = poolObj.gameObject.GetComponent<VisualEffect>();
                        break;
                    case PooledSubObject.Enemy:
                        poolObj.GenericObj = poolObj.gameObject.GetComponent<Enemy>();
                        break;
                    case PooledSubObject.Rigidbody:
                        poolObj.GenericObj = poolObj.gameObject.GetComponent<Rigidbody>();
                        break;
                    case PooledSubObject.TowerProjectile:
                        poolObj.GenericObj = poolObj.gameObject.GetComponent<TowerProjectile>();
                        break;
                    case PooledSubObject.AnimProjectile:
                        poolObj.GenericObj = poolObj.gameObject.GetComponent<AnimProjectile>();
                        break;
                    default:
                        m_GenericObj = null;
                        break;
                }
                m_DeadList.Enqueue(poolObj);

                m_ObjectsInPool++;
                NewPoolGameObj.SetActive(false);
            }

            m_ObjectsAliveInPool++;
            PoolObj returnObject = m_DeadList.Dequeue();
            returnObject.gameObject.SetActive(true);
            return (returnObject);
        }
    }

    public void OnTick()
    {
        if (m_CurrentTick >= m_anagerTicksBeforeClean)
        {
            if (m_ObjectsInPool - m_ObjectsAliveInPool > m_CleanThreshold)
            {
                for (int i = 0; i < m_CleanThreshold; i++)
                {
                    PoolObj returnObject = m_DeadList.Dequeue();
                    Destroy(returnObject.gameObject);
                    m_ObjectsInPool--;
                }
                m_CurrentTick = 5;
            }
            else
            {
                m_CurrentTick = 0;
            }
        }        
        m_CurrentTick++;
    }

    public void AddObjectToPool(PoolObj obj)
    {
        MonoBehaviour addObj = obj as MonoBehaviour;
        addObj.gameObject.SetActive(false);
        m_DeadList.Enqueue(obj);
        m_ObjectsAliveInPool--;
    }
}
