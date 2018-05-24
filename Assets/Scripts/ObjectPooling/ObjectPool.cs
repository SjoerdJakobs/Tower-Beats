using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject ObjectPrefab { get; set; }
    //private Queue<GameObject> m_DeadList;
    private Queue<PoolObj> m_DeadList;

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
        switch (GenericObj)
        {
            case PooledSubObject.Default:
                m_GenericObj = null;
                break;
            case PooledSubObject.GameObject:
                m_GenericObj = ObjectPrefab;
                break;
            case PooledSubObject.VisualEffect:
                m_GenericObj = ObjectPrefab.GetComponent<VisualEffect>();
                break;
            case PooledSubObject.Enemy:
                m_GenericObj = ObjectPrefab.GetComponent<Enemy>();
                break;
            case PooledSubObject.Rigidbody:
                m_GenericObj = ObjectPrefab.GetComponent<Rigidbody>();
                break;
            default:
                m_GenericObj = null;
                break;
        }
        PoolObjDataStruct TempData = new PoolObjDataStruct(this,ObjectPrefab,m_GenericObj);
        ObjectPrefab.GetComponent<PoolObj>().ObjData = TempData;
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
            poolObj.ObjData = new PoolObjDataStruct(this, NewPoolGameObj, m_GenericObj);
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
            returnObject.ObjData.GameObj.SetActive(true);
            return (returnObject);
        }
        else
        {
            for (int i = 0; i < m_IncreaseIncrement; i++)
            {
                GameObject NewPoolGameObj = Instantiate(ObjectPrefab, transform);
                PoolObj poolObj = NewPoolGameObj.GetComponent<PoolObj>();
                poolObj.ObjData = new PoolObjDataStruct(this, NewPoolGameObj, m_GenericObj);
                m_DeadList.Enqueue(poolObj);

                m_ObjectsInPool++;
                NewPoolGameObj.SetActive(false);
            }

            m_ObjectsAliveInPool++;
            PoolObj returnObject = m_DeadList.Dequeue();
            returnObject.ObjData.GameObj.SetActive(true);
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
                    Destroy(returnObject.ObjData.GameObj);
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
